using System.Collections;
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
    public List<ItemType> buildingTypes = new();

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
                string currentSlotType = "";
                if (itemParent.TryGetComponent<InventorySlot>(out var inventorySlot))
                {
                    isEquipmentItem = false;
                    currentSlotType = inventorySlot.currentTypeSlot;
                }
                if (CursorController.instance.GetCurrentCursorName() == MessageController.OPEN_BOX)
                {
                    if (currentSlotType == MessageController.OPEN_BOX)
                    {
                        bool canAddInventory = InventoryController.instance.UnEquipmentItem(item);
                        if (!canAddInventory)
                        {
                            LogController.instance.Log(MessageController.INVENTORY_FULL, gameObject);
                            return;
                        }
                    }
                    else
                    {
                        bool canAdd = BoxController.instance.AddItemToBoxStore(item);
                        if (!canAdd)
                        {
                            LogController.instance.Log(MessageController.INVENTORY_FULL, gameObject);
                            return;
                        }
                    }
                    CloseActionBar();
                    return;
                }

                if (equipmentTypes.Contains(item.GetItemType()))
                {
                    if (!isEquipmentItem)
                    {
                        EquipmentController.instance.EquipmentItem(item);
                        HandIconManager.instance.UpdatePunchIcon();

                        if (item.TryGetComponent<ItemEquipmentConfig>(out var itemConfig))
                        {
                            CharacterEquipmentController.instance.CharacterEquipment(itemConfig, itemConfig.equipmentFor, itemConfig.GetWorldObject(), itemConfig.GetMaterial());
                        }
                    }
                    else
                    {
                        bool canAddInventory = InventoryController.instance.UnEquipmentItem(item);
                        HandIconManager.instance.UpdatePunchIcon();
                        if (!canAddInventory)
                        {
                            LogController.instance.Log(MessageController.INVENTORY_FULL, gameObject);
                        }

                        if (item.TryGetComponent<ItemEquipmentConfig>(out var itemConfig))
                        {
                            CharacterEquipmentController.instance.CharacterEquipment(itemConfig, itemConfig.equipmentFor, null, null);
                        }
                    }
                    CloseActionBar();
                }
                else if (foodTypes.Contains(item.GetItemType()))
                {

                }
                else if (buildingTypes.Contains(item.GetItemType()))
                {
                    if (item.TryGetComponent<InventoryBuildingItem>(out var buildingItem))
                    {
                        buildingItem.UseBuildingItem();
                        CloseActionBar();
                    }
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
            GameObject itemParent = item.transform.parent.gameObject;
            ItemType type = item.GetItemType();
            bool isEquipmentItem = true;
            string currentSlotType = "";
            if (itemParent.TryGetComponent<InventorySlot>(out var inventorySlot))
            {
                isEquipmentItem = false;
                currentSlotType = inventorySlot.currentTypeSlot;
            }
            if (CursorController.instance.GetCurrentCursorName() == MessageController.OPEN_BOX)
            {
                useBtnTxt.text = currentSlotType == MessageController.OPEN_BOX ? "Lấy ra" : "Cất vào";
                removeBtn.gameObject.SetActive(false);
                return;
            }
            bool show = false;
            if (equipmentTypes.Contains(type))
            {
                useBtnTxt.text = isEquipmentItem ? "Tháo ra" : "Trang bị";
                show = true;
            }
            else if (foodTypes.Contains(type) || buildingTypes.Contains(type))
            {
                useBtnTxt.text = "Sử dụng";
                show = true;
            }
            useBtn.gameObject.SetActive(show);
            removeBtn.gameObject.SetActive(true);
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
