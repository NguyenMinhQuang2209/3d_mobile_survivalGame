using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class CancelIconManager : MonoBehaviour
{
    public static CancelIconManager instance;

    [SerializeField] private Button cancelIcon;
    [SerializeField] private Button rotateIcon;

    private string currentCancelState = "";

    public static string CANCEL_TAG_BUILDING = "Player_Building";
    public static string CANCEL_TAG_NONE = "";
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
        cancelIcon.onClick.AddListener(() =>
        {
            ChangeCancelState("");
        });
        rotateIcon.onClick.AddListener(() =>
        {
            RotateBuildingItem();
        });
        cancelIcon.gameObject.SetActive(false);
        rotateIcon.gameObject.SetActive(false);
    }
    public void ChangeCancelState(string newState)
    {
        if (currentCancelState == newState)
        {
            return;
        }
        switch (currentCancelState)
        {
            case "Player_Building":
                BuildingController.instance.StartBuildingItem(null);
                HandIconManager.instance.ChangeInteractingState(HandIconManager.PUNCHING_STATE);
                break;
        }
        currentCancelState = newState;
        cancelIcon.gameObject.SetActive(currentCancelState != "");
        rotateIcon.gameObject.SetActive(currentCancelState != "");
    }
    public void OnHolding(bool v)
    {
        if (currentCancelState == CANCEL_TAG_BUILDING)
        {
            if (v)
            {
                cancelIcon.gameObject.SetActive(false);
                rotateIcon.gameObject.SetActive(false);
            }
            else
            {
                cancelIcon.gameObject.SetActive(currentCancelState != "");
                rotateIcon.gameObject.SetActive(currentCancelState != "");
            }
        }
    }
    public void RotateBuildingItem()
    {
        BuildingController.instance.RotateItem();
    }
}
