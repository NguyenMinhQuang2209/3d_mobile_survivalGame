using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;

    public GameObject parentInventoryContainer;
    public GameObject inventoryContainer;
    public InventorySlot inventorySlot;

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

    [Space(10)]
    [Header("Box store UI")]
    public GameObject boxStoreUI;
    public GameObject boxContainer;

    List<InventoryItem> boxStoringItems = null;
    Transform currentBoxItem = null;

    [Header("Trash")]
    public Transform trashUI;

    [Header("Pet UI")]
    public GameObject parentPetContainer;
    public GameObject petContainer;
    public PetItem petItem;
    public PetAction petAction;

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
        OnChangeCursorName(MessageController.OPEN_INVENTORY, new() { parentInventoryContainer });
        OnClickInventoryItem(null);
    }

    public void InteractWithPet()
    {
        OnChangeCursorName(MessageController.OPEN_PET_UI, new() { parentPetContainer });
        PetController.instance.SpawnPetItem();
        PetAction.instance.ChangePetItem(null);
    }


    public void InteractWithBox(Transform newObject, int slot, List<InventoryItem> inventoryItems = null)
    {
        OnChangeCursorName(MessageController.OPEN_BOX, new() { boxContainer, parentInventoryContainer });
        if (currentBoxItem == newObject)
        {
            return;
        }
        if (boxStoringItems != null)
        {
            for (int i = 0; i < boxStoreUI.transform.childCount; i++)
            {
                Transform child = boxStoreUI.transform.GetChild(i);
                if (child != null)
                {
                    if (child.childCount > 0)
                    {
                        Transform inventoryChild = child.GetChild(0);
                        if (inventoryChild.gameObject.TryGetComponent<InventoryItem>(out var inventoryItem))
                        {
                            inventoryItem.transform.SetParent(trashUI, false);
                            boxStoringItems[i] = inventoryItem;
                        }
                        else
                        {
                            boxStoringItems[i] = null;
                        }
                    }
                }
            }
        }
        currentBoxItem = newObject;
        boxStoringItems = inventoryItems;
        foreach (Transform item in boxStoreUI.transform)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < slot; i++)
        {
            InventorySlot currentSlot = Instantiate(inventorySlot, boxStoreUI.transform);
            currentSlot.currentTypeSlot = MessageController.OPEN_BOX;
            if (inventoryItems[i] != null)
            {
                inventoryItems[i].transform.SetParent(currentSlot.transform, false);
            }
        }
        OnClickInventoryItem(null);
    }

    public string GetCurrentCursorName()
    {
        return currentCursorName;
    }
    public void OnChangeCursorName(string newCursorName, List<GameObject> newCursorList)
    {
        if (currentCursorName == newCursorName)
        {
            currentCursorName = string.Empty;
            InteractCurrentCursor(false);
            currentCursors = null;
            return;
        }
        currentCursorName = newCursorName;
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
    public void CloseCursor()
    {
        OnChangeCursorName("", null);
        OnClickInventoryItem(null);
        PetAction.instance.ChangePetItem(null);
    }
}
