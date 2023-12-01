using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class PetInteract : Interactible
{
    [SerializeField] private bool wasPet = false;

    FollowingAnimals followingAnimals;
    private void Start()
    {
        followingAnimals = GetComponent<FollowingAnimals>();
        if (!wasPet)
        {
            followingAnimals.enabled = false;
            canInteract = true;
        }
        else
        {
            followingAnimals.enabled = true;
            canInteract = false;
        }
    }
    public override void Interact()
    {
        wasPet = true;
        canInteract = false;
        followingAnimals.enabled = true;
        PetController.instance.AddPet(followingAnimals);
    }
}
