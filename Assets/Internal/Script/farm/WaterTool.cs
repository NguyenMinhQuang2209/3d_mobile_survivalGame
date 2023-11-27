using UnityEngine;
using UnityEngine.UI;

public class WaterTool : FarmTool
{
    [SerializeField] private float maxWater = 100f;
    float currentWater = 0f;
    InventoryItem inventoryItem;
    string defaultDescription = string.Empty;

    [SerializeField] private Image waterImage;
    [SerializeField] private Sprite emptyWaterSprite;
    [SerializeField] private Sprite fullWaterSprite;
    private void Start()
    {
        inventoryItem = GetComponent<InventoryItem>();
        defaultDescription = inventoryItem.GetDescription();

        currentWater = maxWater;
    }
    private void Update()
    {
        inventoryItem.SetDescription(defaultDescription + "\n Lượng nước: " + Mathf.Round(currentWater) + "%");
        if (Mathf.Round(currentWater) == 0)
        {
            waterImage.sprite = emptyWaterSprite;
        }
        else
        {
            waterImage.sprite = fullWaterSprite;
        }
    }
    public override void UseFarmTool()
    {
        base.UseFarmTool();
    }
    public void WaterHealth(float v)
    {
        currentWater = Mathf.Min(currentWater + v, maxWater);
    }
    public float UseWater(float v)
    {
        if (v > currentWater)
        {
            float value = v - currentWater;
            currentWater = 0f;
            return value;
        }
        currentWater -= v;
        return 0;
    }
    public float GetCurrentWater()
    {
        return currentWater;
    }
    public void UpdateCurrentWater(float v)
    {
        currentWater = Mathf.Min(maxWater, v);
    }
    public void ClearWater()
    {
        currentWater = 0f;
    }
    public bool IsItem(FarmToolName name)
    {
        return name == GetFarmToolName();
    }
}
