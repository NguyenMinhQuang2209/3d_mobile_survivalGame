using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleBoxController : MonoBehaviour
{
    private List<InventoryItem> inventoryItems = new();
    public List<InventoryItem> GetMultipleInventoryItems()
    {
        return inventoryItems;
    }
    public void SetMultipleInventoryItem(List<InventoryItem> newInventoryItems)
    {
        inventoryItems = newInventoryItems;
    }
}
