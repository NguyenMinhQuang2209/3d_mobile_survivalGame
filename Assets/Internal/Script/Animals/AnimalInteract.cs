using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInteract : Interactible
{
    CollectingObject collecting = null;
    bool canInteracting = false;

    bool wasInteracting = false;

    private Animator animator;
    private void Start()
    {
        collecting = GetComponent<CollectingObject>();
        animator = GetComponent<Animator>();
    }
    public override void Interact()
    {
        if (collecting != null && canInteracting && !wasInteracting)
        {
            bool stillRemain = collecting.AddItem();
            if (stillRemain)
            {
                LogController.instance.Log(MessageController.INVENTORY_FULL);
            }
            else
            {
                wasInteracting = true;
                if (animator != null)
                {
                    animator.SetTrigger("Die");
                    Destroy(gameObject, 2f);
                }
            }
        }
    }
    public void InteractingObject()
    {
        canInteracting = true;
    }
    public bool WasInteracting()
    {
        return wasInteracting;
    }
}
