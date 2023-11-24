using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagController : MonoBehaviour
{
    public static TagController instance;

    public static string PLAYER_TAG = "Player";
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
