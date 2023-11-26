using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentController : MonoBehaviour
{
    public static EquipmentController instance;

    public List<EquipmentSlotItem> equipmentSlotItems = new();

    [Space(20)]
    [SerializeField] private EquipmentSlot hand;
    [SerializeField] private EquipmentSlot hat;
    [SerializeField] private EquipmentSlot shoe;
    [SerializeField] private EquipmentSlot shirt;
    [SerializeField] private EquipmentSlot pant;
    [SerializeField] private EquipmentSlot bag;
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
    public GameObject GetEquipmentObject(EquipmentType type)
    {
        switch (type)
        {
            case EquipmentType.Hand:
                return hand.GetEquipmentObject();
            case EquipmentType.Hat:
                return hat.GetEquipmentObject();
            case EquipmentType.Shoe:
                return shoe.GetEquipmentObject();
            case EquipmentType.Pant:
                return pant.GetEquipmentObject();
            case EquipmentType.Shirt:
                return shirt.GetEquipmentObject();
            case EquipmentType.Bag:
                return bag.GetEquipmentObject();
            default:
                break;
        }
        return null;
    }

}
[System.Serializable]
public class EquipmentSlotItem
{
    public EquipmentSlot slot;
    public List<ItemType> itemTypes = new();
}
public enum EquipmentType
{
    Hand,
    Shirt,
    Pant,
    Shoe,
    Hat,
    Bag
}