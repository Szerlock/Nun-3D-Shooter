using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float currentDamage;

    public void SetDamage(float damage)
    {
        this.currentDamage = damage;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tank_Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage);
                StartCoroutine(enemy.Stagger());
                Destroy(gameObject);
            }

        }

        if (col.CompareTag("Exploding_Enemy"))
        {
            ExplodingEnemyStats enemy = col.GetComponent<ExplodingEnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage);
                Destroy(gameObject);
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
                enemy.TakeDamage(currentDamage);
                Destroy(gameObject);
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
                enemy.TakeDamage(currentDamage);
                Destroy(gameObject);
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
                enemy.TakeGunDamage(currentDamage);
                Destroy(gameObject);
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
                enemy.TakeGunDamage(currentDamage);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("SecondPhase component not found on enemy object.");
            }
        }
    }
}
