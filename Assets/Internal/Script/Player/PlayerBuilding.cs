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
        }
        if (buildingItem != null)
        {
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
            if (currentObj != null)
            {
                currentObj.position = currentPos;
            }
        }
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