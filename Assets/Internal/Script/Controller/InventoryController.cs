using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    public int maxInventorySlot = 24;
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
        OnDrawInventorySlot();
    }
    private void OnDrawInventorySlot()
    {
        if (CursorController.instance != null)
        {
            GameObject inventory = CursorController.instance.inventoryContainer;
            GameObject inventorySlot = CursorController.instance.inventorySlot;
            for (int i = 0; i < maxInventorySlot; i++)
            {
                Instantiate(inventorySlot, inventory.transform);
            }
        }
    }
    public bool UnEquipmentItem(InventoryItem item)
    {
        if (IsFull()) return false;
        GameObject inventory = CursorController.instance.inventoryContainer;
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                item.transform.parent = slot.transform;
                return true;
            }
        }
        return false;
    }
    public bool AddItem(InventoryItem item, int quantity = 1)
    {
        if (!item.UseStack() && IsFull()) return false;
        GameObject inventory = CursorController.instance.inventoryContainer;
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                InventoryItem newItem = Instantiate(item, slot.transform);
                newItem.ChangeQuantity(quantity);
                return true;
            }
            else
            {
                if (!item.UseStack())
                {
                    continue;
                }
                GameObject childItem = slot.transform.GetChild(0).gameObject;
                if (childItem.TryGetComponent<InventoryItem>(out var childItemInventory))
                {
                    if (childItemInventory.CompareObject(item))
                    {
                        int remain = childItemInventory.PlusItem(quantity);
                        if (remain == 0)
                        {
                            return true;
                        }
                        quantity = remain;
                    }
                }
            }
        }
        return false;
    }
    public bool IsFull()
    {
        GameObject inventory = CursorController.instance.inventoryContainer;
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                return false;
            }
        }
        LogController.instance.Log("Inventory is full");
        return true;
    }
}
