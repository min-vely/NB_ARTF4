using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utility;
using UnityEngine;

#region Item Class
/// <summary>
/// 게임 내의 개별 아이템을 표현하는 클래스입니다.
/// </summary>
public class Item : MonoBehaviour
{
    #region Field
    private string _id; // 아이디
    private string _name; //이름
    private string _category; //속성
    private string _description; // 설명
    private bool _isActive; // 활성상태
    private float _power; // 아이템이 주는 효과
    private float _duration; // 아이템 지속시간
    #endregion
    #region Initialization
    public void Initialize(string id, string name, string category, string description, float power, float duration)
    {
        _id = id;
        _name = name;
        _category = category;
        _description = description;
        _power = power;
        _duration = duration;
        _isActive = false; // 기본적으로 아이템은 비활성 상태로 시작
    }
    #endregion
    #region Property
    /// <summary>
    /// 아이템의 고유 식별자를 반환합니다.
    /// </summary>
    public string Id => _id;

    /// <summary>
    /// 아이템의 이름을 반환합니다.
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// 아이템의 카테고리를 반환합니다.
    /// </summary>
    public string Category => _category;

    /// <summary>
    /// 아이템에 대한 설명을 반환합니다.
    /// </summary>
    public string Description => _description;

    /// <summary>
    /// 아이템이 주는  효과를 반환합니다.
    /// </summary>
    public float Power => _power;

    /// <summary>
    /// 아이템의 효과가 지속되는 시간입니다.
    /// </summary>
    /// <returns></returns>
    public float Duration
    {
        get => _duration;
        set
        {
            _duration = value;
        }
    }

    /// <summary>
    /// 아이템의 활성상태를 반환합니다.
    /// </summary>
    public bool IsActivate
    {
        get => _isActive;
        set
        {
            _isActive = value;
        }
    }
    #endregion
}
#endregion

#region ItemManager Class
/// <summary>
/// 아이템을 관리하는 매니저 클래스입니다.
/// </summary>
public class ItemManager : MonoBehaviour
{
    #region Field
    /// <summary>
    /// 아이템의 목록을 저장하는 딕셔너리입니다.
    /// </summary>
    private Dictionary<string, Item> _items = new Dictionary<string, Item>();
    #endregion

    #region Item management methods
    // 아이템의 효과를 플레이어에 적용하거나 제거하는 메서드
    // apply가 true이면 효과를 적용하고, false이면 효과를 제거합니다.
    private void UpdatePlayerWithItemEffect(Item item, bool apply)
    {
        float factor = apply ? item.Power : 1 / item.Power;

        switch (item.Id)
        {
            case "0":
                // Main.Controller.SetMoveSpeed(_player.GetSpeed() * factor); 
                break;
            case "1":
                // Main.PlayerController.SetJumpForce(_player.GetJumpForc() * factor);
                break;
        }
    }

    // 아이템의 효과를 플레이어에게 적용하는 메서드
    private void ApplyItemsEffectToPlayer(Item item)
    {
        UpdatePlayerWithItemEffect(item, true);
    }

    // 아이템의 효과를 원래대로 되돌리는 메서드
    private void RemoveItemsEffectFromPlayer(Item item)
    {
        UpdatePlayerWithItemEffect(item, false);
    }

    public IEnumerator ActivateItem(Item item)
    {
        // 아이템을 활성화합니다.
        item.IsActivate = true;
        ApplyItemsEffectToPlayer(item);
        Debug.Log($"{item.Name} 아이템이 활성화되었습니다.");

        // 아이템의 지속 시간만큼 대기합니다.
        yield return new WaitForSeconds(item.Duration);

        // 아이템을 비활성화합니다.
        item.IsActivate = false;
        Debug.Log($"{item.Name} 아이템이 비활성화되었습니다.");

        // 아이템을 목록에서 제거합니다.
        RemoveItem(item);
    }
    /// <summary>
    /// 아이템을 추가하는 메서드입니다.
    /// </summary>
    public void AddItem(Item item)
    {
        if (_items.ContainsKey(item.Id))
        {
            // 기존 아이템의 활성 상태를 업데이트합니다.
            _items[item.Id].Duration += _items[item.Id].Duration;
        }
        else 
        {
            foreach (var hadItem in _items)
            {
                if (hadItem.Value.IsActivate)
                {
                    RemoveItem(hadItem.Value); 
                }
            }
            _items.Add(item.Id, item);
            _items[item.Id].IsActivate = true;
        }

        // 아이템을 추가하고 활성화 코루틴을 시작합니다.
        StartCoroutine(ActivateItem(item));
    }

    /// <summary>
    /// 아이템을 제거하는 메서드입니다.
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (_items.ContainsKey(item.Id))
        {
            item.IsActivate = false;
            _items.Remove(item.Id);
            RemoveItemsEffectFromPlayer(item);
        }
        else
        {
            System.Console.WriteLine($"아이템을 찾을 수 없습니다: {item.Id}");
        }
    }

    public Item LoadAndCreateItems(string jsonFilePath,Transform transform)
    {
        // JSON 파일의 내용을 읽어옵니다.
        string jsonText = System.IO.File.ReadAllText(jsonFilePath);

        // JSON 데이터를 직접 파싱합니다.
        var json = JsonUtility.FromJson<Dictionary<string, object>>(jsonText);

        // InstantiatePrefab 함수를 이용하여 프리팹을 인스턴스화하고, 인스턴스화된 게임 오브젝트에 Item 컴포넌트를 추가합니다.
        // TODO : 제이슨 데이터 완성시에 키값 조정 필요 
        GameObject itemObject = Main.Resource.InstantiatePrefab(json["_name"].ToString(),transform);
        
        // 아이템 객체를 초기화합니다.
        Item item = SceneUtility.GetAddComponent<Item>(itemObject);

        item.Initialize(
                            id: json["_id"].ToString(),
                            name: json["_name"].ToString(),
                            category: json["_category"].ToString(),
                            description: json["_description"].ToString(),
                            power: float.Parse(json["_power"].ToString()),
                            duration: float.Parse(json["_duration"].ToString())
                            );
      
        return item;
    }
    #endregion
}
#endregion