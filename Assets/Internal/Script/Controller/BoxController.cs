using UnityEngine;

public class BoxController : MonoBehaviour
{
    public static BoxController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public bool AddItemToBoxStore(InventoryItem item)
    {
        GameObject boxStoreUI = CursorController.instance.boxStoreUI;
        return InventoryController.instance.UnEquipmentItem(item, boxStoreUI);
    }
}
