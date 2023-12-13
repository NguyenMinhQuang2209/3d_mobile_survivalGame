using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

    [SerializeField] private TextMeshProUGUI coinTxt;

    int totalCoin = 0;
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
        coinTxt.text = totalCoin.ToString();
    }

    public void AddCoin(int v)
    {
        totalCoin += v;
        coinTxt.text = totalCoin.ToString();
    }
    public bool RemoveCoin(int v)
    {
        if (totalCoin < v)
        {
            coinTxt.text = totalCoin.ToString();
            return false;
        }
        totalCoin -= v;
        coinTxt.text = totalCoin.ToString();
        return true;
    }
    public int GetTotalCoin()
    {
        return totalCoin;
    }
    public void ClearAllCoin()
    {
        totalCoin = 0;
        coinTxt.text = totalCoin.ToString();
    }
}
