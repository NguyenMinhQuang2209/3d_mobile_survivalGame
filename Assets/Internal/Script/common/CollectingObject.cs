using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingObject : MonoBehaviour
{
    [SerializeField] private List<CollectingItem> collectingItems = new();

    readonly List<RemainCollectingItem> remainItems = new();
    public bool AddItem()
    {
        bool itemRemain = false;
        for (int i = 0; i < collectingItems.Count; i++)
        {
            CollectingItem collectingItem = collectingItems[i];
            int quantity = Random.Range(Mathf.Min(collectingItem.quantity.x, collectingItem.quantity.y),
                Mathf.Max(collectingItem.quantity.x, collectingItem.quantity.y));
            int remain = InventoryController.instance.AddItem(collectingItem.item, quantity);
            if (remain > 0)
            {
                remainItems.Add(new(collectingItem.item, remain));
                itemRemain = true;
            }
        }
        return itemRemain;
    }
}
[System.Serializable]
public class CollectingItem
{
    public InventoryItem item;
    public Vector2Int quantity;
}
public class RemainCollectingItem
{
    public InventoryItem item;
    public int quantity = 1;
    public RemainCollectingItem(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}