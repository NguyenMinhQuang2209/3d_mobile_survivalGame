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
            inventoryItem.transform.SetParent(container, false);
        }
        else
        {
            Transform previousParent = item.transform.parent;
            inventoryItem.transform.SetParent(previousParent, false);
            item.transform.SetParent(container, false);
            inventoryItem = item;
        }
    }
    public GameObject GetEquipmentObject()
    {
        return container.childCount > 0 ? container.GetChild(0).gameObject : null;
    }
}
