using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordControls : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    private Animator animatorSword;
    private BoxCollider swordCollider;

    //current Stats
    protected float currentDamage;


    void Awake()
    {
        swordCollider = GetComponent<BoxCollider>();
        swordCollider.enabled = false;
        animatorSword = GetComponent<Animator>();
        if (weaponData != null)
        {
        currentDamage = weaponData.Damage;
        }

        else
        {
            Debug.LogError("Weapon data not assigned.");
        }
    }

    public void SwordAttack()
    {
        //play attack animation
        animatorSword.SetTrigger("Attack");
        EnableCollider();
        Debug.Log("Sword Attack");
        animatorSword.SetBool("EndAttack", true);
        EndAttack();
    }

    public void EnableCollider()
    {
        swordCollider.enabled = true;
    }

    private void EndAttack()
    {
        swordCollider.enabled = false;
        Debug.Log("End Attack");
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
