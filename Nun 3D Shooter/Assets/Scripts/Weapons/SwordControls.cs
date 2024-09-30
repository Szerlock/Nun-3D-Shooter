using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordControls : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    [HideInInspector]
    public Animator animatorSword;
    public BoxCollider swordCollider;

    //current Stats
    protected float currentDamage;


    void Start()
    {
        swordCollider.enabled = false;
        if(swordCollider != null)
        {
            Debug.Log("Sword collider found.");
        }
        animatorSword = GetComponent<Animator>();
        if (weaponData != null)
        {
        currentDamage = weaponData.Damage;
        }

        else
        {
            Debug.Log("Weapon data not assigned.");
        }
    }

    public void SwordAttack()
    {
        StartCoroutine(DeactivateCollider(0.6f));
        EnableCollider();
        Debug.Log("Sword Attack");
    }   

    public void EnableCollider()
    {
        swordCollider.enabled = true;
        Debug.Log("Collider enabled");
    }

    public IEnumerator DeactivateCollider(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        swordCollider.enabled = false;
        Debug.Log("End Attack");
    }

    protected void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Tank_Enemy"))
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

        if(col.CompareTag("Exploding_Enemy"))
        {
            ExplodingEnemyStats enemy = col.GetComponent<ExplodingEnemyStats>();
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
