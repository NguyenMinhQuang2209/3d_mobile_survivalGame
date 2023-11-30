using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;

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

    public void AddCoin(int v)
    {
        totalCoin += v;
    }
    public bool RemoveCoin(int v)
    {
        if (totalCoin < v)
        {
            return false;
        }
        totalCoin -= v;
        return true;
    }
    public int GetTotalCoin()
    {
        return totalCoin;
    }
    public void ClearAllCoin()
    {
        totalCoin = 0;
    }
}
