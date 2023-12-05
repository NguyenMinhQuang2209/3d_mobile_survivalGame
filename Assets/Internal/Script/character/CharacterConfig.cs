using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConfig : MonoBehaviour
{
    [SerializeField] private Transform handHolder;

    [SerializeField] private SkinnedMeshRenderer hat;
    [SerializeField] private SkinnedMeshRenderer shirt;
    [SerializeField] private SkinnedMeshRenderer pant;
    [SerializeField] private SkinnedMeshRenderer shoe;

    private Material defaultHatMaterial;
    private Material defaultShirtMaterial;
    private Material defaultShoeMaterial;
    private Material defaultPantMaterial;

    PlayerAttacking playerAttacking;
    private void Start()
    {
        transform.parent.gameObject.TryGetComponent<PlayerAttacking>(out playerAttacking);

        defaultPantMaterial = pant.material;
        defaultHatMaterial = hat.material;
        defaultShirtMaterial = shirt.material;
        defaultShoeMaterial = pant.material;
    }
    public void Attack()
    {
        if (playerAttacking != null)
        {
            playerAttacking.Attack();
        }
    }

    public void ChangeColorEquipment(EquipmentFor equipmentFor, GameObject worldItem, Material color)
    {
        switch (equipmentFor)
        {
            case EquipmentFor.Hand:
                foreach (Transform child in handHolder)
                {
                    Destroy(child.gameObject);
                }
                playerAttacking.SwitchAttackingType(worldItem == null ? WeaponType.Hand : WeaponType.Sword);
                if (worldItem != null)
                {
                    Instantiate(worldItem, handHolder.transform);
                }
                break;
            case EquipmentFor.Shirt:
                shirt.material = color != null ? color : defaultShirtMaterial;
                break;
            case EquipmentFor.Pant:
                pant.material = color != null ? color : defaultPantMaterial;
                break;
            case EquipmentFor.Shoe:
                shoe.material = color != null ? color : defaultShoeMaterial;
                break;
            case EquipmentFor.Hat:
                hat.material = color != null ? color : defaultHatMaterial;
                break;
        }
    }
}
