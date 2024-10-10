using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //base stats for enemies
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed {get => moveSpeed; private set => moveSpeed = value;}

    [SerializeField]
    float maxHealth;
    public float MaxHealth {get => maxHealth; private set => maxHealth = value;}

    [SerializeField]
    float damage;
    public float Damage {get => damage; private set => damage = value;}

    [SerializeField]
    float projectileSpeed;
    public float ProjectileSpeed {get => projectileSpeed; private set => projectileSpeed = value;}

    [SerializeField]
    int currencyAmount;
    public int CurrencyAmount {get => currencyAmount; private set => currencyAmount = value;}

    [SerializeField]
    int healthCurrencyAmount;
    public int HealthCurrencyAmount { get => healthCurrencyAmount; private set => healthCurrencyAmount = value; }
}
