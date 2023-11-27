using System.Collections.Generic;
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


    [SerializeField] private Transform showUI;
    [SerializeField] private GameObject water;
    [SerializeField] private GameObject fertilizer;

    [Space(20)]
    [Header("Water and fertilizer setting time")]
    [SerializeField] private float wateringDelayTime = 1f;
    [SerializeField] private float fertilizerDelayTime = 1f;
    float currentWateringTime = 0f;
    float currentFertilizerTime = 0f;
    [SerializeField] private float maxWater = 100f;
    float currentWater = 0f;

    [Space(10)]
    [Header("Show UI position")]
    [SerializeField] private List<Vector3> showUIPosition = new();


    private void Start()
    {
        defaultPromptMessage = promptMessage;
        water.SetActive(false);
        fertilizer.SetActive(false);
        currentWater = maxWater;
    }

    [SerializeField] private GameObject planParent;
    private void Update()
    {
        if (currentItem != null && !canCollect)
        {
            currentGrowingTime += Time.deltaTime * currentItemRateGrowingTime;
            currentWateringTime += Time.deltaTime;
            currentFertilizerTime += Time.deltaTime;

            if (currentWateringTime >= wateringDelayTime)
            {
                currentWater = Mathf.Max(0f, currentWater - Time.deltaTime);
            }

            water.SetActive(currentWateringTime >= wateringDelayTime);
            fertilizer.SetActive(currentFertilizerTime >= fertilizerDelayTime);
            promptMessage = currentItem.displayName + " \n Level: " + (currentIndex + 1) + "\nwater:" + Mathf.Round(currentWater) + "%";

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
                    showUI.transform.localPosition = showUIPosition.Count <= currentIndex ? showUIPosition[^1] : showUIPosition[currentIndex];
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
            if (currentWateringTime >= wateringDelayTime)
            {
                GameObject handHolding = EquipmentController.instance.GetEquipmentObject(EquipmentType.Hand);
                if (handHolding != null && handHolding.GetComponent<InventoryItem>() != null && handHolding.TryGetComponent<WaterTool>(out var waterTool))
                {
                    float currentWaterInBottle = waterTool.GetCurrentWater();
                    if (Mathf.Round(currentWaterInBottle) == 0)
                    {
                        LogController.instance.Log(MessageController.OUT_OF_WATER);
                        return;
                    }
                    currentWateringTime = 0f;
                    if (currentWaterInBottle >= maxWater - currentWater)
                    {
                        waterTool.UseWater(Mathf.Round(maxWater - currentWater));
                        currentWater = maxWater;
                    }
                    else
                    {
                        currentWater += currentWaterInBottle;
                        waterTool.ClearWater();
                    }
                }
            }

            if (currentFertilizerTime >= fertilizerDelayTime)
            {
                GameObject handHolding = EquipmentController.instance.GetEquipmentObject(EquipmentType.Hand);
                if (handHolding != null && handHolding.TryGetComponent<InventoryItem>(out var inventoryItem) && handHolding.GetComponent<FertilizerTool>() != null)
                {

                    int remain = inventoryItem.GetCurrentQuantity();
                    if (remain == 0)
                    {
                        LogController.instance.Log(MessageController.QUANTITY_ITEM_END);
                        HandIconManager.instance.EmergencyState();
                        return;
                    }
                    inventoryItem.MinusItem(-1);
                    currentFertilizerTime = 0f;
                    if (remain == 1)
                    {
                        Destroy(handHolding);
                        HandIconManager.instance.EmergencyState();
                    }
                }
            }

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
        currentWateringTime = 0f;
        currentFertilizerTime = 0f;
        water.SetActive(false);
        fertilizer.SetActive(false);
        showUI.transform.localPosition = showUIPosition.Count <= currentIndex ? showUIPosition[^1] : showUIPosition[currentIndex];
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
