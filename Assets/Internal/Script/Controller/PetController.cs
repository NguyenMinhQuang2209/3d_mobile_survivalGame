using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetController : MonoBehaviour
{
    public static PetController instance;

    private List<FollowingAnimals> pets = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void AddPet(FollowingAnimals newPet)
    {
        if (!pets.Contains(newPet))
        {
            pets.Add(newPet);
        }
    }
    public void RemovePet(FollowingAnimals pet)
    {
        pets.Remove(pet);
    }
    private void ClearPetItem()
    {
        GameObject petContainer = CursorController.instance.petContainer;
        if (petContainer != null)
        {
            foreach (Transform child in petContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public void SpawnPetItem()
    {
        ClearPetItem();
        GameObject petContainer = CursorController.instance.petContainer;
        PetItem petItem = CursorController.instance.petItem;
        for (int i = 0; i < pets.Count; i++)
        {
            if (petContainer != null)
            {
                PetItem temp = Instantiate(petItem, petContainer.transform, false);
                temp.SetFollowingAnimal(pets[i]);
            }
        }
    }
}
