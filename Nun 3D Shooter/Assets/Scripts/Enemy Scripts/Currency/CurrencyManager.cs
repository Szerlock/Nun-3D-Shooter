using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CurrentCurrency;
    public int CurrentHealthCurrency { get; private set; }

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
        HealingIcon.instance.ModifyHealth(amount);
        CurrentHealthCurrency += amount;
    }

    public int SpendHealthCurrency()
    {
        int temp = CurrentHealthCurrency;
        CurrentHealthCurrency = 0;
        return temp;
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (CurrentCurrency >= amount)
        {
            CurrentCurrency -= amount;
            return true;
        }
        return false;
    }

    public void ResetCurrency()
    {
        CurrentCurrency = 0;
        CurrentHealthCurrency = 0;
    }
}
