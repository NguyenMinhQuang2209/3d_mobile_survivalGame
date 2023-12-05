using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplerBox : Interactible
{
    [Header("Get Slot random (can not greater than maxSlot of inventory)")]
    [SerializeField] private Vector2Int slots = Vector2Int.one;
    int currentSlot = 0;
    private void Start()
    {
        currentSlot = Random.Range(Mathf.Min(slots.x, slots.y), Mathf.Max(slots.x, slots.y));
    }
    public override void Interact()
    {
        List<InventoryItem> items = BoxController.instance.GetMultipleBoxItems();
        CursorController.instance.InteractWithBox(transform, currentSlot, items);
    }
}
