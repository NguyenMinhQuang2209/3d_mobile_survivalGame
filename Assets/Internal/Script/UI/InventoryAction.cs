﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAction : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI descriptionTxt;
    public Button useBtn;
    public TextMeshProUGUI useBtnTxt;
    public Button removeBtn;
    public Button closeBtn;

    public List<ItemType> equipmentTypes = new();
    public List<ItemType> foodTypes = new();

    InventoryItem item;
    private void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            CloseActionBar();
        });
        useBtn.onClick.AddListener(() =>
        {
            if (item != null)
            {
                GameObject itemParent = item.transform.parent.gameObject;
                bool isEquipmentItem = true;
                if (itemParent.GetComponent<InventorySlot>() != null)
                {
                    isEquipmentItem = false;
                }
                if (equipmentTypes.Contains(item.GetItemType()))
                {
                    if (!isEquipmentItem)
                    {
                        EquipmentController.instance.EquipmentItem(item);
                    }
                    else
                    {
                        bool canAddInventory = InventoryController.instance.UnEquipmentItem(item);
                        if (!canAddInventory)
                        {
                            LogController.instance.Log("Inventory is full!", gameObject);
                        }
                    }
                    CloseActionBar();
                }
                else if (foodTypes.Contains(item.GetItemType()))
                {

                }
                else
                {

                }
            }
        });
        removeBtn.onClick.AddListener(() =>
        {

        });
    }
    private void Update()
    {
        if (item != null)
        {
            ItemType type = item.GetItemType();
            GameObject itemParent = item.transform.parent.gameObject;
            bool isEquipmentItem = true;
            if (itemParent.GetComponent<InventorySlot>() != null)
            {
                isEquipmentItem = false;
            }
            bool show = false;
            if (equipmentTypes.Contains(type))
            {
                useBtnTxt.text = isEquipmentItem ? "Tháo ra" : "Trang bị";
                show = true;
            }
            else if (foodTypes.Contains(type))
            {
                useBtnTxt.text = "Sử dụng";
                show = true;
            }
            useBtn.gameObject.SetActive(show);
        }
    }
    public void OnClickInventoryItem(InventoryItem newItem)
    {
        item = newItem;
        if (item != null)
        {
            nameTxt.text = item.GetDisplayName();
            descriptionTxt.text = item.GetDescription();
        }
    }
    private void CloseActionBar()
    {
        item = null;
        CursorController.instance.OnClickInventoryItem(null);
    }
}
