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

        // Debug.Log("Trigger detected with: " + col.gameObject.name); // Check if trigger is detected

        // if (col.CompareTag("Enemy"))
        // {
        //     Debug.Log("Hit an enemy!"); // Confirm that the correct tag is detected

        //     EnemyStats enemy = col.GetComponent<EnemyStats>();
        //     if (enemy != null)
        //     {
        //         Debug.Log("Applying damage: " + currentDamage); // Verify damage application
        //         enemy.TakeDamage(currentDamage);
        //     }
        //     else
        //     {
        //         Debug.LogError("EnemyStats component not found on enemy object.");
        //     }
        // }
    }
}
