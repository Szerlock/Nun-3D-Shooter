using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordControls : MonoBehaviour
{
    public GameObject text;

    public WeaponScriptableObject weaponData;
    [HideInInspector]
    public Animator animatorSword;
    public BoxCollider swordCollider;
    //public PlayerMovement pm

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
    }

    public void SwordAttack()
    {
        //(pm.charactermovement.ismoving)
        StartCoroutine(DeactivateCollider(0.6f));
        EnableCollider();
    }   

    public void EnableCollider()
    {
        swordCollider.enabled = true;
    }

    public IEnumerator DeactivateCollider(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        swordCollider.enabled = false;
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
                Debug.LogError("ExplodingEnemyStats component not found on enemy object.");
            }
        }

        if(col.CompareTag("Ghost_Enemy"))
        {
            GhostEnemyStats enemy = col.GetComponent<GhostEnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
            }
            else
            {
                Debug.LogError("GhostEnemyStats component not found on enemy object.");
            }
        }

        if(col.CompareTag("Imp_Enemy"))
        {
            ImpEnemy enemy = col.GetComponent<ImpEnemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
            }
            else
            {
                Debug.LogError("ImpEnemy component not found on enemy object.");
            }
        }

        if(col.CompareTag("DoubleFace_Enemy"))
        {
            DoubleFaceStats enemy = col.GetComponent<DoubleFaceStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
            }
            else
            {
                Debug.LogError("DoubleFaceStats component not found on enemy object.");
            }
        }
    }
}
