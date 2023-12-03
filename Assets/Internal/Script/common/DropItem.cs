using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : Interactible
{
    private List<DropItemInfor> items = new();
    public override void Interact()
    {
        AddItem();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagController.PLAYER_TAG))
        {
            AddItem();
        }
    }
    private void AddItem()
    {
        if (items == null)
        {
            Destroy(gameObject);
            return;
        }
        bool itemRemain = false;
        List<DropItemInfor> remainItems = new();
        for (int i = 0; i < items.Count; i++)
        {
            DropItemInfor collectingItem = items[i];
            int quantity = items[i].quantity;
            int remain = InventoryController.instance.AddItem(collectingItem.item, quantity);
            if (remain > 0)
            {
                remainItems.Add(new(collectingItem.item, remain));
                itemRemain = true;
            }
        }
        if (!itemRemain)
        {
            Destroy(gameObject);
        }
        else
        {
            items = remainItems;
        }
    }
    public void MyInitialized(List<DropItemInfor> newItems)
    {
        items = newItems;
    }
}
[System.Serializable]
public class DropItemInfor
{
    public InventoryItem item;
    public int quantity;
    public DropItemInfor(InventoryItem item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}