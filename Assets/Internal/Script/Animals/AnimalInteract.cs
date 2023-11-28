using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInteract : Interactible
{
    CollectingObject collecting = null;
    bool canInteracting = false;
    private void Start()
    {
        collecting = GetComponent<CollectingObject>();
    }
    public override void Interact()
    {
        if (collecting != null && canInteracting)
        {
            bool stillRemain = collecting.AddItem();
            if (stillRemain)
            {
                LogController.instance.Log(MessageController.INVENTORY_FULL);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
