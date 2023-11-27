using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [HideInInspector]
    public string currentTypeSlot = "";
    public void CloseInventoryActionBar()
    {
        CursorController.instance.OnClickInventoryItem(null);
    }
}
