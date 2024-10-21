using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlade : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    private float frameAttackSpeed = 0.1f;

    [SerializeField]
    private float currentDamage;

    private void Start()
    {
        currentDamage = weaponData.Damage;
    }

    private void Update()
    {
        frameAttackSpeed -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tank_Enemy") || col.CompareTag("Exploding_Enemy") || col.CompareTag("Ghost_Enemy") ||
            col.CompareTag("Imp_Enemy") || col.CompareTag("DoubleFace_Enemy") || col.CompareTag("SecondPhase_Enemy"))
        {
            ApplyDamage(col);
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.CompareTag("Tank_Enemy") || col.CompareTag("Exploding_Enemy") || col.CompareTag("Ghost_Enemy") ||
            col.CompareTag("Imp_Enemy") || col.CompareTag("DoubleFace_Enemy") || col.CompareTag("SecondPhase_Enemy"))
        {
            if (frameAttackSpeed <= 0)
            {
                ApplyDamage(col);
                frameAttackSpeed = 0.2f;
            }
        }
    }
    protected void ApplyDamage(Collider col)
    {
        if (frameAttackSpeed <= 0)
        {
            Debug.Log("Sword hit");
            if (col.CompareTag("Tank_Enemy"))
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

            if (col.CompareTag("Exploding_Enemy"))
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

            if (col.CompareTag("Ghost_Enemy"))
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

            if (col.CompareTag("Imp_Enemy"))
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

            if (col.CompareTag("DoubleFace_Enemy"))
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
            if (col.CompareTag("SecondPhase_Enemy"))
            {
                SecondPhase enemy = col.GetComponent<SecondPhase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
                }
                else
                {
                    Debug.LogError("SecondPhase component not found on enemy object.");
                }
            }
        }
    }

    public void ChangeCurrentDamage(float value)
    {
        currentDamage *= (1 + value);
    }
}
