using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerBuilding : MonoBehaviour
{
    private BuildingItem buildingItem = null;
    Transform currentObj = null;
    Vector3 currentPos;
    Vector3 currentRot;

    [SerializeField] private LayerMask buildingMask;
    [SerializeField] private string iconTag = "Icons";
    [SerializeField] private GraphicRaycaster uiRaycaster;

    private void Update()
    {
        Show();
    }
    public void ChangeBuildingItem(BuildingItem newBuildingItem)
    {
        buildingItem = newBuildingItem;
        if (currentObj != null)
        {
            Destroy(currentObj.gameObject);
            currentObj = null;
        }

        if (buildingItem != null)
        {
            HandIconManager.instance.ChangeInteractingState(HandIconManager.BUILDING_STATE);
            CancelIconManager.instance.ChangeCancelState(CancelIconManager.CANCEL_TAG_BUILDING);
            GameObject tempObj = Instantiate(buildingItem.preview, transform.forward, Quaternion.Euler(currentRot));
            currentObj = tempObj.transform;
        }
    }
    public void Show()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (!CheckTouchIcon(touch.position))
            {
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buildingMask))
                {
                    currentPos = hit.point;
                    currentPos = new Vector3(Mathf.Round(currentPos.x), currentPos.y, Mathf.Round(currentPos.z));
                    if (currentObj != null)
                    {
                        currentObj.position = currentPos;
                    }
                }
            }
        }
    }
    public bool CheckTouchIcon(Vector2 touchPosition)
    {
        PointerEventData eventData = new(EventSystem.current)
        {
            position = touchPosition
        };

        List<RaycastResult> results = new();
        uiRaycaster.Raycast(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(iconTag))
            {
                return true;
            }
        }
        return false;
    }
    public void SpawnObject()
    {
        if (currentObj != null && currentObj.gameObject.TryGetComponent<ObjectConfig>(out var objectConfig))
        {
            if (objectConfig.mainObject.TryGetComponent<BuildingPreview>(out var buildingPreviewItem))
            {
                if (buildingPreviewItem.CanBuild())
                {
                    bool canRemove = InventoryController.instance.RemoveItem(buildingItem.inventoryItem, 1);
                    if (canRemove)
                    {
                        Instantiate(buildingItem.building, currentPos, buildingPreviewItem.UseRotate() ? Quaternion.Euler(currentRot) : Quaternion.identity);
                        currentPos += Vector3.forward;
                        int remain = InventoryController.instance.GetQuantity(buildingItem.inventoryItem);
                        if (remain == 0)
                        {
                            CancelIconManager.instance.ChangeCancelState(CancelIconManager.CANCEL_TAG_NONE);
                        }
                        else
                        {
                            if (currentObj != null)
                            {
                                currentObj.position = currentPos;
                            }
                        }
                    }
                    else
                    {
                        LogController.instance.Log(MessageController.INVENTORY_REMOVE_ERROR);
                    }
                }
                else
                {
                    LogController.instance.Log(MessageController.BUILDING_ERROR);
                }
            }
        }
    }
    public void RotateObject()
    {
        currentRot.y = currentRot.y == 360f ? 90f : currentRot.y + 90f;
        currentObj.rotation = Quaternion.Euler(currentRot);
    }
}
[System.Serializable]
public class BuildingItem
{
    public GameObject preview;
    public GameObject building;
    public InventoryItem inventoryItem;
    public BuildingItem(GameObject preview, GameObject building, InventoryItem inventoryItem)
    {
        this.preview = preview;
        this.inventoryItem = inventoryItem;
        this.building = building;
    }
}