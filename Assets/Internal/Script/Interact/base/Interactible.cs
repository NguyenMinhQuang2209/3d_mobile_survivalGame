using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour
{
    public string promptMessage;
    public bool useEvent;
    public bool canInteract = true;
    public void BaseInteract()
    {
        Interact();
        if (useEvent)
            GetComponent<InteractibleEvent>().onInteract.Invoke();
    }
    public virtual void Interact()
    {

    }
}
