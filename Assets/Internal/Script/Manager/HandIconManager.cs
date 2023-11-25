using UnityEngine;
using UnityEngine.UI;

public class HandIconManager : MonoBehaviour
{
    public static HandIconManager instance;
    [SerializeField] private GameObject interactIcon;
    [SerializeField] private GameObject punchIcon;
    [SerializeField] private GameObject buildingIcon;

    [SerializeField] private Button handlIcon;
    private string currentState = "";

    public static string INTERACING_STATE = "Interacting";
    public static string BUILDING_STATE = "Building";
    public static string PUNCHING_STATE = "";
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
        punchIcon.SetActive(currentState == PUNCHING_STATE);
        interactIcon.SetActive(currentState == INTERACING_STATE);
        buildingIcon.SetActive(currentState == BUILDING_STATE);
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
                punchIcon.SetActive(currentState == PUNCHING_STATE);
                interactIcon.SetActive(currentState == INTERACING_STATE);
                buildingIcon.SetActive(currentState == BUILDING_STATE);
            }
        }
    }
    public string GetCurrentState()
    {
        return currentState;
    }
    public void HandClick()
    {
        if (currentState == PUNCHING_STATE)
        {

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
