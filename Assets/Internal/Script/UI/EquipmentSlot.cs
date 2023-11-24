using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour
{
    [SerializeField] private GameObject defaultIcons;
    [SerializeField] private Transform container;

    InventoryItem inventoryItem;
    private void Update()
    {
        defaultIcons.SetActive(container.childCount == 0);
    }
    public void EquipmentItem(InventoryItem item)
    {

        if (container.childCount == 0)
        {
            inventoryItem = item;
            inventoryItem.transform.parent = container;
        }
        else
        {
            Transform previousParent = item.transform.parent;
            inventoryItem.transform.parent = previousParent;
            item.transform.parent = container;
            inventoryItem = item;
        }
    }
}
