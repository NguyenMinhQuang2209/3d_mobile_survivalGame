using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySeedItem : MonoBehaviour
{

    [SerializeField] private bool useSameTime = false;
    [Tooltip("Only using for usesametime = true")]
    [SerializeField] private float sameTime = 0f;
    [SerializeField] private float growingRateTime = 1f;
    public string displayName = "";

    [Space(10)]
    [SerializeField] private List<PlanItem> planItems = new();

    [SerializeField] private InventoryItem collectingItem;
    [SerializeField] private Vector2Int quantity = new();
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
    public InventoryItem GetInventoryItem()
    {
        return collectingItem;
    }
    public int GetQuantity()
    {
        return Random.Range(Mathf.Min(quantity.x, quantity.y), Mathf.Max(quantity.x, quantity.y) + 1);
    }

}
[System.Serializable]
public class PlanItem
{
    public GameObject prefab;
    public float growingTime;
}
