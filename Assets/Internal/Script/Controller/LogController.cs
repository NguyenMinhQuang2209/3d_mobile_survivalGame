using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogController : MonoBehaviour
{
    public static LogController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void Log(string prompt, GameObject target)
    {
        Debug.Log(prompt, target);
    }
    public void Log(string prompt)
    {
        Debug.Log(prompt);
    }
}
