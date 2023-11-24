using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignInteract : Interactible
{
    public override void Interact()
    {
        LogController.instance.Log("Interact item", gameObject);
    }
}
