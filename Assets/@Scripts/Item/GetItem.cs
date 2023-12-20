using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private ItemSpawnInfo _itemSpawnInfo;
    private int _id;
    private Item _item;
    private float fallTime = 1f;
    // Start is called before the first frame update
   private void Start()
    {
        Main.Obstacle.OnInitializedObstacle += InitializedObstacle;
        _itemSpawnInfo = gameObject.GetComponent<ItemSpawnInfo>();
        _id = _itemSpawnInfo.Id;
        _item = Main.Item.FieldItems[_id];
    }
    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Fall(fallTime));
                Main.Item.AddItem(_item);
            }
        }
    }
    private IEnumerator Fall(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    private void InitializedObstacle()
    {
        gameObject.SetActive(true);
    }
}
