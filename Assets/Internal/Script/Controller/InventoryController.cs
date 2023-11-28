using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController instance;

    public int maxInventorySlot = 24;

    public int currentSlot = 1;

    private Dictionary<string, int> stock = new();



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
        UpdateStock();
    }
    private void OnDrawInventorySlot()
    {
        if (CursorController.instance != null)
        {
            GameObject inventory = CursorController.instance.inventoryContainer;
            InventorySlot inventorySlot = CursorController.instance.inventorySlot;
            for (int i = 0; i < currentSlot; i++)
            {
                Instantiate(inventorySlot, inventory.transform);
            }
        }
    }
    public bool UnEquipmentItem(InventoryItem item)
    {
        if (IsFull()) return false;
        GameObject inventory = CursorController.instance.inventoryContainer;
        return UnEquipmentItem(item, inventory);
    }
    public bool UnEquipmentItem(InventoryItem item, GameObject inventory)
    {
        if (IsFull()) return false;
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                item.transform.SetParent(slot.transform, false);
                return true;
            }
        }
        return false;
    }
    public int AddItem(InventoryItem item, int quantity = 1)
    {
        if (!item.UseStack() && IsFull()) return quantity;
        GameObject inventory = CursorController.instance.inventoryContainer;
        foreach (Transform slot in inventory.transform)
        {
            if (slot.childCount == 0)
            {
                InventoryItem newItem = Instantiate(item, slot.transform);
                newItem.ChangeQuantity(quantity);
                UpdateStock();
                return 0;
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
                            UpdateStock();
                            return 0;
                        }
                        quantity = remain;
                    }
                }
            }
        }
        return quantity;
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
    public void UpdateStock()
    {
        stock?.Clear();
        GameObject inventory = CursorController.instance.inventoryContainer;
        for (int i = 0; i < currentSlot; i++)
        {
            Transform slot = inventory.transform.GetChild(i);
            if (slot.childCount > 0)
            {
                GameObject child = slot.GetChild(0).gameObject;
                if (child.TryGetComponent<InventoryItem>(out var childItemInventory))
                {
                    UpdateStock(childItemInventory.GetItemName(), childItemInventory.GetCurrentQuantity());
                    continue;
                }
            }
        }
    }
    private void UpdateStock(ItemName itemName, int quantity)
    {
        stock[itemName.ToString()] = stock.ContainsKey(itemName.ToString()) ? stock[itemName.ToString()] + quantity : quantity;
    }
    public bool RemoveItem(ItemName itemName, int removeQuantity = 1)
    {
        if (stock.ContainsKey(itemName.ToString()) && stock[itemName.ToString()] >= removeQuantity)
        {
            GameObject inventory = CursorController.instance.inventoryContainer;
            foreach (Transform slot in inventory.transform)
            {
                if (slot.childCount > 0)
                {
                    GameObject child = slot.GetChild(0).gameObject;
                    if (child.TryGetComponent<InventoryItem>(out var childItemInventory))
                    {
                        if (childItemInventory.GetItemName() == itemName)
                        {
                            int remain = childItemInventory.MinusItem(removeQuantity * -1);
                            UpdateStock();
                            if (remain == 0)
                            {
                                return true;
                            }
                            removeQuantity = remain * -1;
                        }
                    }
                }
            }
        }
        return false;
    }
    public bool RemoveItem(InventoryItem item, int removeQuantity = 1)
    {
        return RemoveItem(item.GetItemName(), removeQuantity);
    }
    public int GetQuantity(ItemName itemName)
    {
        return stock.ContainsKey(itemName.ToString()) ? stock[itemName.ToString()] : 0;
    }
    public int GetQuantity(InventoryItem item)
    {
        return GetQuantity(item.GetItemName());
    }

    public int GetCurrentSlot()
    {
        return currentSlot;
    }

    public int GetMaxSlot()
    {
        return maxInventorySlot;
    }


}
