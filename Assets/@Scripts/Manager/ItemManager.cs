using System.Collections.Generic;
using Scripts.Utility;
using UnityEngine;

namespace Scripts.Manager
{
    public class ItemManager
    {
        private readonly List<Vector3> _itemPosition = new ()
        {
            new Vector3(-2.26f, 1.56f, 46.34f),
            new Vector3(0,3.39f, 16.63f),
            new Vector3(0,3.39f, 16.63f)
        };

        public void Initialized()
        {
            SetUpItemData();
        }

        private void SetUpItemData()
        {
            ItemDataContainer itemContainer = Main.Data.ItemDataContainerLoader();
            Transform transform = Main.Game.ItemTransform().transform;
            int count = 0;
            foreach (var itemPair in itemContainer.Items)
            {
                ItemData data = itemPair.Value;
                GameObject itemGameObject = Main.Resource.InstantiatePrefab($"{data.id}.prefab", transform);
                Item itemObject = SceneUtility.GetAddComponent<Item>(itemGameObject);
                itemObject.Initialize(data.id, data.name, data.category, data.description, data.power, data.duration);
                itemObject.transform.position = _itemPosition[count];
                count = count >= 3 ? 0 : count;
                count++;
            }
        }
    }
}