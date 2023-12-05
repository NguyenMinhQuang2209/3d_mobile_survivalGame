
using UnityEngine;

public class ItemEquipmentConfig : MonoBehaviour
{
    public EquipmentFor equipmentFor;

    [Header("For equipment")]
    [SerializeField] private GameObject worldObject;

    [Space(10)]
    [Header("For change color")]
    [SerializeField] private ColorName colorName;

    public GameObject GetWorldObject()
    {
        return worldObject;
    }

    public Material GetMaterial()
    {
        return ColorController.instance.GetMaterial(colorName);
    }

    public EquipmentFor GetEquipmentFor()
    {
        return equipmentFor;
    }
}
