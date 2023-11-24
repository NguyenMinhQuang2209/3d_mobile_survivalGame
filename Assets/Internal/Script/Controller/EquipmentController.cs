using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public List<EquipmentSlotItem> equipmentSlotItems = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void EquipmentItem(InventoryItem item)
    {
        ItemType itemType = item.GetItemType();
        foreach (EquipmentSlotItem slotItem in equipmentSlotItems)
        {
            if (slotItem.itemTypes.Contains(itemType))
            {
                slotItem.slot.EquipmentItem(item);
                return;
            }
        }
    }

}
[System.Serializable]
public class EquipmentSlotItem
{
    public EquipmentSlot slot;
    public List<ItemType> itemTypes = new();
}
