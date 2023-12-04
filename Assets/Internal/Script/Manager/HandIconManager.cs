using UnityEngine;
using UnityEngine.UI;

public class HandIconManager : MonoBehaviour
{
    public static HandIconManager instance;


    [SerializeField] private Image handImage;
    [SerializeField] private Button handlIcon;

    [SerializeField] private Sprite handSprite;
    [SerializeField] private Sprite interactingSprite;
    [SerializeField] private Sprite buildingSprite;
    private string currentState = "";

    public static string INTERACING_STATE = "Interacting";
    public static string BUILDING_STATE = "Building";
    public static string PUNCHING_STATE = "";

    bool handHasItem = false;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        handlIcon.onClick.AddListener(() =>
        {
            HandClick();
        });
    }
    public void ChangeInteractingState(string newState)
    {
        if (currentState != newState)
        {
            if (currentState == BUILDING_STATE && newState == INTERACING_STATE)
            {
                // Can not changing from Building to interacting State
            }
            else
            {
                currentState = newState;
                if (currentState == PUNCHING_STATE)
                {

                    UpdatePunchIcon();
                }
                else if (currentState == INTERACING_STATE)
                {
                    if (!handHasItem)
                    {
                        handImage.sprite = interactingSprite;
                    }
                }
                else if (currentState == BUILDING_STATE)
                {
                    handImage.sprite = buildingSprite;
                }
            }
        }
    }
    public void UpdatePunchIcon()
    {
        if (currentState != PUNCHING_STATE)
        {
            return;
        }
        GameObject handHolding = EquipmentController.instance.GetEquipmentObject(EquipmentType.Hand);
        if (handHolding != null && handHolding.GetComponent<InventoryItem>() != null)
        {
            GameObject child = handHolding.transform.GetChild(0).gameObject;
            if (child != null && child.TryGetComponent<Image>(out var childImage))
            {
                handImage.sprite = childImage.sprite;
                handHasItem = true;
            }
        }
        else
        {
            handImage.sprite = handSprite;
            handHasItem = false;
        }
    }
    public string GetCurrentState()
    {
        return currentState;
    }
    public void EmergencyState()
    {
        handImage.sprite = handSprite;
        handHasItem = false;
    }
    public void HandClick()
    {
        if (currentState == PUNCHING_STATE)
        {
            UIController.instance.Interact();
        }
        else if (currentState == INTERACING_STATE)
        {
            UIController.instance.Interact();
        }
        else if (currentState == BUILDING_STATE)
        {
            BuildingController.instance.SpawnBuildingItem();
        }
    }

}
