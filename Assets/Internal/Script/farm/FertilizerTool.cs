using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FertilizerTool : FarmTool
{
    InventoryItem inventoryItem;
    private void Start()
    {
        inventoryItem = GetComponent<InventoryItem>();
    }
    public void UseItem()
    {
        inventoryItem.MinusItem();
    }
}
