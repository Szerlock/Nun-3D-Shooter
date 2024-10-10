using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CurrentCurrency { get; private set; }
    public int CurrentHealthCurrency { get; private set; }

    private float multiplier = 1.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddHealthCurrency(int amount)
    {
        CurrentHealthCurrency += amount;
    }

    public int SpendHealthCurrency()
    {
        Debug.Log("Spending Health Currency");
        int increments = CurrentHealthCurrency / 10;
        double healingAmount = 0;
        if(increments < 1)
        {
            return CurrentHealthCurrency;
        }
        for (int i = 0; i < increments; i++)
        {
            healingAmount += 10 * System.Math.Pow(multiplier, i);
        }
        healingAmount = System.Math.Round(healingAmount, 0);
        CurrentHealthCurrency = 0;
        return (int) healingAmount;
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
    }

    public void SpendCurrency(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
        }
    }

    public void ResetCurrency()
    {
        CurrentCurrency = 0;
        CurrentHealthCurrency = 0;
    }
}
