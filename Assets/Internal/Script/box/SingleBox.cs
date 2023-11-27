using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleBox : Interactible
{
    [Header("Get Slot random (can not greater than maxSlot of inventory)")]
    [SerializeField] private Vector2Int slots = Vector2Int.one;
    List<InventoryItem> items = new();
    int currentSlot = 0;
    private void Start()
    {
        currentSlot = Random.Range(Mathf.Min(slots.x, slots.y), Mathf.Max(slots.x, slots.y));
        for (int i = 0; i < currentSlot; i++)
        {
            items.Add(null);
        }
    }


    public override void Interact()
    {
        CursorController.instance.InteractWithBox(transform, currentSlot, items);
    }
}
