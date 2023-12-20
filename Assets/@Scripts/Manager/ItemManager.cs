using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#region Item Class

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
    private Dictionary<int, Item> _hadItems = new Dictionary<int, Item>();
    // 필드 아이템 데이터를 저장하는 딕셔너리입니다.
    public Dictionary<int, Item> FieldItems = new Dictionary<int, Item>();

    // 아이템 프리팹을 저장하는 딕셔너리입니다.
    private Dictionary<int, GameObject> _itemPrefabs = new Dictionary<int, GameObject>();

    // 데이터 매니저 인스턴스입니다.
    private DataManager _dataManager;

    #endregion

    public void Initialized()
    {
        _dataManager = Main.DataManager;
        // JSON 파일에서 아이템 데이터를 로드합니다.
        LoadItemDataFromJson();

        // 아이템 프리팹을 로드합니다.
        LoadItemPrefabs();
    }
    public void ddd()
    {
        GameObject sectorObject = Main.Resource.InstantiatePrefab("ItemManager.prefab");
    }
    #region Item management methods

    /// <summary>
    /// 아이템의 효과를 플레이어에 적용하거나 제거하는 메서드
    /// apply가 true이면 효과를 적용하고, false이면 효과를 제거합니다.
    /// </summary>
    /// <param name="item">효과를 적용할 아이템</param>
    /// <param name="apply">효과를 적용할 것인지 불값을 받습니다.</param>
    private void UpdatePlayerWithItemEffect(Item item, bool apply)
    {
        float factor = apply ? item.Power : 1 / item.Power;

        switch (item.Id)
        {
            case 0:
                // Main.PlayerControl.SetMoveSpeed(_player.GetSpeed() * factor); 
                break;
            case 1:
                // Main.PlayerController.SetJumpForce(_player.GetJumpForc() * factor);
                break;
        }
    }

    /// <summary>
    ///  아이템의 효과를 플레이어에게 적용하는 메서드
    /// </summary>
    /// <param name="item">적용할 아이템</param>
    private void ApplyItemsEffectToPlayer(Item item)
    {
        UpdatePlayerWithItemEffect(item, true);
    }

    /// <summary>
    ///  아이템의 효과를 원래 대로 되돌리는 메서드
    /// </summary>
    /// <param name="item">적용할 아이템</param>
    private void RemoveItemsEffectFromPlayer(Item item)
    {
        UpdatePlayerWithItemEffect(item, false);
    }
    /// <summary>
    /// 아이템의 효과를 적용시키는 메서드
    /// </summary>
    /// <param name="item">적용할 아이템</param>
    /// <returns></returns>
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
        if (_hadItems.ContainsKey(item.Id))
        {
            // 기존 아이템의 활성 상태를 업데이트합니다.
            _hadItems[item.Id].Duration += _hadItems[item.Id].Duration;
        }
        else
        {
            foreach (var hadItem in _hadItems)
            {
                if (hadItem.Value.IsActivate)
                {
                    RemoveItem(hadItem.Value);
                }
            }
            _hadItems.Add(item.Id, item);
            _hadItems[item.Id].IsActivate = true;
        }

        // 아이템을 추가하고 활성화 코루틴을 시작합니다.
        StartCoroutine(ActivateItem(item));
    }

    /// <summary>
    /// 아이템을 제거하는 메서드입니다.
    /// </summary>
    public void RemoveItem(Item item)
    {
        if (_hadItems.ContainsKey(item.Id))
        {
            item.IsActivate = false;
            _hadItems.Remove(item.Id);
            RemoveItemsEffectFromPlayer(item);
        }
        else
        {
            System.Console.WriteLine($"아이템을 찾을 수 없습니다: {item.Id}");
        }
    }


    #endregion
    

    // JSON 파일에서 아이템 데이터를 로드하는 메서드입니다.
    private void LoadItemDataFromJson()
    {
        // JSON 파일을 로드하여 아이템 데이터 컨테이너를 가져옵니다.
        ItemDataContainer itemDataContainer = _dataManager.itemLoader();

        // 아이템 데이터 컨테이너에서 아이템 데이터를 가져와 필드 아이템 딕셔너리에 저장합니다.
        foreach (var data in itemDataContainer.Items)
        {
            var itemData = data.Value;
            Item item = new();

            Debug.Log(data.Value.name);
            item.Initialize(data.Key, itemData.name, itemData.category, itemData.description, itemData.power, itemData.duration);
            FieldItems.TryAdd(item.Id, item);
        }
    }
  
    // 아이템 프리팹을 로드하는 메서드입니다.
    private void LoadItemPrefabs()
    {
        // 필드 아이템 딕셔너리에서 아이템 데이터를 가져와 아이템 프리팹을 로드하고 아이템 프리팹 딕셔너리에 저장합니다.
        foreach (var item in FieldItems)
        {
            string keyName = item.Key.ToString();
            // 아이템 프리팹을 로드합니다.
            GameObject itemPrefab = Main.Resource.Load<GameObject>($"{keyName}.prefab");
            Item itemObj = SceneUtility.GetAddComponent<Item>(itemPrefab);
            int id = itemPrefab.GetComponent<ItemSpawnInfo>().Id;
            var itemData = FieldItems[id];
            itemObj.Initialize(itemData.Id, itemData.Name, itemData.Category, itemData.Description, itemData.Power, itemData.Duration);

            // 아이템 프리팹 딕셔너리에 아이템 프리팹을 저장합니다.
            _itemPrefabs.TryAdd(item.Key, itemPrefab);
        }
    }

    // 아이템 스포너에서 아이템을 인스턴스화하는 메서드입니다.
    public void InstantiateItemsFromSpawner()
    {
        // 아이템 스포너를 로드합니다.
        GameObject spawner = Main.Resource.Load<GameObject>("ItemPos.prefab");

        // 아이템 스포너에서 스폰 포인트를 가져옵니다.
        ItemSpawnInfo[] spawnPoints = spawner.GetComponentsInChildren<ItemSpawnInfo>();

        // 각 스폰 포인트에서 아이템 ID를 가져와 해당 ID를 가진 아이템 프리팹을 인스턴스화합니다.
        foreach (var spawnPoint in spawnPoints)
        {
            int itemId = spawnPoint.GetComponent<ItemSpawnInfo>().Id;

            if (_itemPrefabs.ContainsKey(itemId))
            {
                // 아이템 프리팹을 가져옵니다.
                GameObject itemPrefab = _itemPrefabs[itemId];

                // 아이템 프리팹을 인스턴스화합니다.
                Main.Resource.InstantiatePrefab($"{itemPrefab.name}.prefab");
            }
            else
            {
                Debug.LogError($"아이템 프리팹을 찾을 수 없습니다: {itemId}");
            }
        }
    }
}
#endregion
