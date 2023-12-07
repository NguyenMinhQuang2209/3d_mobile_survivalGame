using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagController : MonoBehaviour
{
    [SerializeField] private DropItem bag;
    public static BagController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void SpawnBag(List<DropItemInfor> items, Vector3 spawnPosition)
    {
        DropItem tempBag = Instantiate(bag, spawnPosition, Quaternion.identity);
        tempBag.MyInitialized(items);
    }
    public void SpawnBag(List<InventoryItemByName> items, Vector3 spawnPosition)
    {
        List<DropItemInfor> tempItems = new();
        foreach (InventoryItemByName item in items)
        {
            GameObject tempItem = PrefabController.instance.GetGameObject(item.itemName);
            if (tempItem.TryGetComponent<InventoryItem>(out var inventoryItem))
            {
                tempItems.Add(new(inventoryItem, item.quantity));
            }
        }
        SpawnBag(tempItems, spawnPosition);
    }

}
[System.Serializable]
public class InventoryItemByName
{
    public ItemName itemName;
    public int quantity;
    public InventoryItemByName(ItemName itemName, int quantity)
    {
        this.itemName = itemName;
        this.quantity = quantity;
    }
}