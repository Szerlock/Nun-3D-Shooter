using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int CurrentCurrency;
    public int CurrentHealthCurrency { get; private set; }

    public int CurrentHealthPotions { get; private set; }


    public TextMeshProUGUI HealthPotionText;

    private void Awake()
    {
        HealthPotionText.text = string.Empty;

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
        if (CurrentHealthPotions == 15) 
        {
            return;
        }
        HealingIcon.instance.ModifyHealth(amount);
        CurrentHealthCurrency += amount;
        if (CurrentHealthCurrency >= 10)
        { 
            CurrentHealthCurrency -= 10;
            CurrentHealthPotions++;
            HealthPotionText.text = CurrentHealthPotions.ToString();
            HealingIcon.instance.ModifyHealth(-1f);

        }
    }

    public void Update()
    {
        if (HealingIcon.instance.healthSlider.value == 0)
        {
            HealingIcon.instance.ModifyHealth(CurrentHealthCurrency);
        }
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

    public void SpendHealthPotion()
    {
        CurrentHealthPotions--;
        HealthPotionText.text = CurrentHealthPotions.ToString();
    }

    public void ResetCurrency()
    {
        CurrentCurrency = 0;
        CurrentHealthCurrency = 0;
    }
}
