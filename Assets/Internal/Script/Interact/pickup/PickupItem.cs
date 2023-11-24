using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : Interactible
{
    [SerializeField] private InventoryItem inventoryItem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagController.PLAYER_TAG))
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        bool canAdd = InventoryController.instance.AddItem(inventoryItem);
        if (!canAdd)
        {
            LogController.instance.Log("Can not pick up item!", gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
