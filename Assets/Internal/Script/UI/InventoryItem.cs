using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantityTxt;
    [Space(10)]
    [SerializeField] private bool useStack = false;
    [SerializeField] private int maxQuantity = 1;
    [SerializeField] int currentQuantity = 1;

    [Space(20)]
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemName itemName;

    [Space(10)]
    [SerializeField] private bool useItemNameToDisplay = false;
    [SerializeField] private string displayName = "";
    [SerializeField] private string description = "";


    private void Update()
    {
        UpdateQuantityText();
        if (currentQuantity <= 0)
        {
            Destroy(gameObject, 0.01f);
        }
    }
    private void Start()
    {
        if (useItemNameToDisplay)
        {
            displayName = itemName.ToString();
        }
    }
    public void UpdateQuantityText()
    {
        if (useStack)
        {
            quantityTxt.text = currentQuantity.ToString();
        }
        else
        {
            quantityTxt.text = "";
        }
    }
    public int PlusItem(int value = 1)
    {
        if (!useStack || currentQuantity == maxQuantity) return value;
        int nextQuantity = currentQuantity + value;
        if (nextQuantity > maxQuantity)
        {
            currentQuantity = maxQuantity;
            return nextQuantity - maxQuantity;
        }
        currentQuantity = nextQuantity;
        return 0;
    }

    public int MinusItem(int value = -1)
    {
        if (!useStack) return value;
        currentQuantity += value;
        if (currentQuantity < 0)
        {
            return currentQuantity;
        }
        return 0;
    }

    public void TouchItem()
    {
        CursorController.instance.OnClickInventoryItem(this);
    }
    public ItemType GetItemType()
    {
        return itemType;
    }
    public ItemName GetItemName()
    {
        return itemName;
    }
    public bool UseStack()
    {
        return useStack;
    }
    public void ChangeQuantity(int newValue)
    {
        currentQuantity = newValue;
    }
    public bool CompareObject(InventoryItem item)
    {
        return itemType == item.GetItemType() && itemName == item.GetItemName();
    }
    public int GetCurrentQuantity()
    {
        return currentQuantity;
    }
    public string GetDisplayName()
    {
        return displayName;
    }
    public string GetDescription()
    {
        return description;
    }
    public void SetDescription(string newDescription)
    {
        description = newDescription;
    }

    public void UseInventoryItem()
    {
        switch (itemType)
        {
            case ItemType.Building:

                break;
        }
    }
}
