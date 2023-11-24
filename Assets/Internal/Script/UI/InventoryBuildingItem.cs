using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBuildingItem : MonoBehaviour
{
    [SerializeField] private GameObject buildingPreviewObject;
    [SerializeField] private GameObject buildingObject;
    public void UseBuildingItem()
    {
        BuildingItem item = new(buildingPreviewObject, buildingObject, GetComponent<InventoryItem>());
        BuildingController.instance.StartBuildingItem(item);
    }

}
