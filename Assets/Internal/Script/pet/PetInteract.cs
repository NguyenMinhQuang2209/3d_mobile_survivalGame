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
        followingAnimals.WasPet(wasPet);
    }
    public override void Interact()
    {
        wasPet = true;
        canInteract = false;
        followingAnimals.WasPet(wasPet);
        PetController.instance.AddPet(followingAnimals);
    }
    public void BuyPet()
    {
        wasPet = true;
        canInteract = false;
        followingAnimals.WasPet(wasPet);
        PetController.instance.AddPet(followingAnimals);
    }
}
