using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewUIController : MonoBehaviour
{
    public static ViewUIController instance;

    [SerializeField] private ShowTxtConfig showDamageTxt;

    public float offsetY = 0.1f;
    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void ShowTxt(string newV, Vector3 pos, float delayTime)
    {
        ShowTxtConfig tempDamageTxt = Instantiate(showDamageTxt, pos, showDamageTxt.transform.rotation);
        tempDamageTxt.Config(newV, offsetY, delayTime);
    }
}
