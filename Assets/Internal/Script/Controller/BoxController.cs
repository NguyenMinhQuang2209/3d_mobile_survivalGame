using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public static BoxController instance;

    private List<InventoryItem> multipleBoxItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        int slot = InventoryController.instance.GetMaxSlot();
        for (int i = 0; i < slot; i++)
        {
            multipleBoxItems.Add(null);
        }
    }
    public bool AddItemToBoxStore(InventoryItem item)
    {
        GameObject boxStoreUI = CursorController.instance.boxStoreUI;
        return InventoryController.instance.UnEquipmentItem(item, boxStoreUI);
    }
    public List<InventoryItem> GetMultipleBoxItems()
    {
        return multipleBoxItems;
    }
}
