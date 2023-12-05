using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipmentController : MonoBehaviour
{
    public static CharacterEquipmentController instance;

    CharacterConfig config;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag(TagController.PLAYER_TAG).transform;
        foreach (Transform child in player)
        {
            if (child.TryGetComponent<CharacterConfig>(out config))
            {
                break;
            }
        }
    }
    public void CharacterEquipment(ItemEquipmentConfig configEquipment, EquipmentFor equipmentFor, GameObject worldItem, Material color)
    {
        if (config != null)
        {
            config.ChangeColorEquipment(configEquipment, equipmentFor, worldItem, color);
        }
    }
    public void CharacterEquipment(ItemEquipmentConfig configEquipment, EquipmentFor equipmentFor, GameObject worldItem)
    {
        CharacterEquipment(configEquipment, equipmentFor, worldItem, null);
    }
    public void CharacterEquipment(ItemEquipmentConfig configEquipment, EquipmentFor equipmentFor, Material color)
    {
        CharacterEquipment(configEquipment, equipmentFor, null, color);
    }
}
