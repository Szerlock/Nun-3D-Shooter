using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blades : MonoBehaviour
{
    private float currentDamage;
    private float speed;
    private Transform target;

    public void SetDamage(float damage, float speed)
    {
        this.currentDamage = damage;
        this.speed = speed;
    }

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Update()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        transform.LookAt(target.position);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Tank_Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            if (enemy != null)
            {
                enemy.TakeDamage(currentDamage);
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
                enemy.TakeDamage(currentDamage);
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
                enemy.TakeDamage(currentDamage); //we use current damage instead of weapon data damage because of damage multipliers that will be added later
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("SecondPhase component not found on enemy object.");
            }
        }
    }
}
