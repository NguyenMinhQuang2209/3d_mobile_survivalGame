using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    private BuildingItem buildingItem = null;
    Transform currentObj = null;
    Vector3 currentPos;
    Vector3 currentRot;

    [SerializeField] private float offsetForward = 1f;
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] private LayerMask buildingMask;
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
            GameObject tempObj = Instantiate(buildingItem.preview, currentPos, Quaternion.Euler(currentRot));
            currentObj = tempObj.transform;
        }
    }
    public void Show()
    {
        Vector3 buildingPos = transform.position + transform.forward * offsetForward;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(buildingPos);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, buildingMask))
        {
            currentPos = hit.point;
            currentPos = new Vector3(Mathf.Round(currentPos.x), currentPos.y, Mathf.Round(currentPos.z));
            if (currentObj != null)
            {
                currentObj.position = currentPos;
            }
        }
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
                        Instantiate(buildingItem.building, currentPos, Quaternion.Euler(currentRot));
                        int remain = InventoryController.instance.GetQuantity(buildingItem.inventoryItem);
                        if (remain == 0)
                        {
                            CancelIconManager.instance.ChangeCancelState(CancelIconManager.CANCEL_TAG_NONE);
                        }
                    }
                    else
                    {
                        LogController.instance.Log("Can not remove in inventory!");
                    }
                }
                else
                {
                    LogController.instance.Log("Can not build this object");
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