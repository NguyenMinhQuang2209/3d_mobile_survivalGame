using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayController : MonoBehaviour
{
    public static DayController instance;

    [SerializeField] private TextMeshProUGUI dayTxt;
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
