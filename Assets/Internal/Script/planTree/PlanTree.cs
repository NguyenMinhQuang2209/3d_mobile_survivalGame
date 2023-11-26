using Unity.VisualScripting;
using UnityEngine;

public class PlanTree : Interactible
{
    private InventorySeedItem currentItem = null;
    private int currentIndex = 0;
    private bool canCollect = false;

    float currentGrowingTime = 0f;
    float targetGrowingTime = 0f;
    float currentItemRateGrowingTime = 1f;
    GameObject currentStateObject = null;
    string defaultPromptMessage = string.Empty;
    private void Start()
    {
        defaultPromptMessage = promptMessage;
    }

    [SerializeField] private GameObject planParent;
    private void Update()
    {
        if (currentItem != null && !canCollect)
        {
            currentGrowingTime += Time.deltaTime * currentItemRateGrowingTime;

            if (currentGrowingTime >= targetGrowingTime)
            {
                if (currentIndex >= currentItem.GetPlanItems().Count - 1)
                {
                    canCollect = true;
                    promptMessage = MessageController.COLLECTING_MESSAGE;
                }
                else
                {
                    currentGrowingTime = 0f;
                    currentIndex += 1;
                    promptMessage = currentItem.displayName + " \n Level: " + (currentIndex + 1);
                    targetGrowingTime = currentItem.GetGrowingTime(currentIndex);
                    if (currentStateObject != null)
                    {
                        Destroy(currentStateObject);
                    }
                    currentStateObject = Instantiate(currentItem.GetPlanItems()[currentIndex].prefab, planParent.transform);
                }
            }
        }
    }
    public override void Interact()
    {
        if (currentItem == null)
        {
            GameObject handHolding = EquipmentController.instance.GetEquipmentObject(EquipmentType.Hand);
            if (handHolding != null && handHolding.TryGetComponent<InventoryItem>(out var inventoryItem) && handHolding.TryGetComponent<InventorySeedItem>(out var seedItem))
            {
                GameObject pre = PrefabController.instance.GetGameObject(seedItem.preItemType, seedItem.preItemName);
                if (pre != null && pre.TryGetComponent<InventorySeedItem>(out var seedItemPre))
                {
                    int remain = inventoryItem.GetCurrentQuantity();
                    if (remain == 0)
                    {
                        LogController.instance.Log(MessageController.QUANTITY_ITEM_END);
                        HandIconManager.instance.EmergencyState();
                        return;
                    }
                    PlanSeedItem(seedItemPre);
                    inventoryItem.MinusItem(-1);
                    if (remain == 1)
                    {
                        Destroy(handHolding);
                        HandIconManager.instance.EmergencyState();
                    }
                }
            }
        }
        else
        {
            if (canCollect)
            {
                Collecting();
            }
        }
    }
    public void PlanSeedItem(InventorySeedItem newItem)
    {
        currentItem = newItem;
        currentGrowingTime = 0f;
        currentIndex = 0;
        canCollect = false;
        if (currentStateObject != null)
        {
            Destroy(currentStateObject);
        }
        if (currentItem != null)
        {
            promptMessage = currentItem.displayName + " \n Level: " + (currentIndex + 1);
            currentItemRateGrowingTime = currentItem.GetGrowingRateTime();
            targetGrowingTime = newItem.GetGrowingTime(currentIndex);
            currentStateObject = Instantiate(currentItem.GetPlanItems()[currentIndex].prefab, planParent.transform);
        }
    }
    public void Collecting()
    {
        if (canCollect)
        {
            PlanSeedItem(null);
            promptMessage = defaultPromptMessage;
        }
    }
}
