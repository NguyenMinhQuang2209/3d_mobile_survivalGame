using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
                }
                else
                {
                    currentIndex += 1;
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
            if (handHolding != null && handHolding.TryGetComponent<InventorySeedItem>(out var seedItem))
            {
                PlanSeedItem(seedItem);
                Debug.Log("HEre");
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
        }
    }
}
