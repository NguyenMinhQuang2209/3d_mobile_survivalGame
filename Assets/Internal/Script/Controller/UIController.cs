using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static string PLAYER_TAG = "Player";

    public Button jump;

    private GameObject player;
    public static UIController instance;
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
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG);
        jump.onClick.AddListener(() =>
        {
            Jump();
        });
    }
    private void Jump()
    {
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().Jump();
        }
    }
    public void Interact()
    {
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().OnInteract();
        }
    }

}
