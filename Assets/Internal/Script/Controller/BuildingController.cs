using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public static BuildingController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void StartBuildingItem(BuildingItem item)
    {
        GameObject player = GameObject.FindGameObjectWithTag(TagController.PLAYER_TAG);
        if (player != null && player.TryGetComponent<PlayerBuilding>(out var playerBuilding))
        {
            playerBuilding.ChangeBuildingItem(item);
        }
    }
}
