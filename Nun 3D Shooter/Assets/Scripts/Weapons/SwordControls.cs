using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControls : MonoBehaviour
{
    public WeaponScriptableObject weaponData;

    //current Stats
    protected float currentDamage;


    void Awake()
    {
        if (weaponData != null)
        {
        currentDamage = weaponData.Damage;
        }

        else
        {
            Debug.LogError("Weapon data not assigned.");
        }
    }

    protected void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
            }
            else
            {
                Debug.LogError("EnemyStats component not found on enemy object.");
            }
        }
    }
}
