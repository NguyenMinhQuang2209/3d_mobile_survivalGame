using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySeedItem : MonoBehaviour
{
    [SerializeField] private bool useSameTime = false;

    [Tooltip("Only using for usesametime = true")]
    [SerializeField] private float sameTime = 0f;
    [SerializeField] private float growingRateTime = 1f;

    [Space(10)]
    [SerializeField] private List<PlanItem> planItems = new();
    public List<PlanItem> GetPlanItems()
    {
        return planItems;
    }
    public float GetGrowingTime(int index)
    {
        return useSameTime ? sameTime : planItems[index].growingTime;
    }
    public float GetGrowingRateTime()
    {
        return growingRateTime;
    }
}
[System.Serializable]
public class PlanItem
{
    public GameObject prefab;
    public float growingTime;
}
