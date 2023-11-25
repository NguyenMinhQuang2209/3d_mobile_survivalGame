using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public GameObject parentInventoryContainer;
    public GameObject inventoryContainer;
    public GameObject inventorySlot;

    [Space(10)]
    public GameObject bigContainer;

    [Space(10)]
    [Header("Inventory item actions")]
    public InventoryAction itemActionBar;
    public float offsetX = 0f;
    public float offsetY = 5f;

    InventoryItem currentInventoryItem;

    string currentCursorName = string.Empty;
    List<GameObject> currentCursors = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        OnClickInventoryItem(null);
        foreach (Transform item in bigContainer.transform)
        {
            item.gameObject.SetActive(false);
        }
    }
    public void OnClickInventoryItem(InventoryItem item)
    {
        if (item != currentInventoryItem)
        {
            currentInventoryItem = item;
        }
        else
        {
            currentInventoryItem = null;
        }
        if (currentInventoryItem != null)
        {
            itemActionBar.transform.position = currentInventoryItem.transform.position + new Vector3(offsetX, offsetY, 0f);
        }
        itemActionBar.OnClickInventoryItem(currentInventoryItem);
        itemActionBar.gameObject.SetActive(currentInventoryItem != null);
    }

    public void InteractWithInventory()
    {
        OnChangeCursorName("inventory", new() { parentInventoryContainer });
        OnClickInventoryItem(null);
    }
    public void OnChangeCursorName(string currentCursorName, List<GameObject> newCursorList)
    {
        if (currentCursorName == this.currentCursorName)
        {
            this.currentCursorName = string.Empty;
            InteractCurrentCursor(false);
            currentCursors = null;
            return;
        }
        this.currentCursorName = currentCursorName;
        InteractCurrentCursor(false);
        currentCursors = newCursorList;
        InteractCurrentCursor(true);
    }
    private void InteractCurrentCursor(bool v)
    {
        if (currentCursors == null)
        {
            return;
        }
        foreach (GameObject item in currentCursors)
        {
            item.SetActive(v);
        }
    }
}
