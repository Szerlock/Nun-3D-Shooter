using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField]
    SphereCollider explosionCollider;
    private Wave wave;

    //[HideInInspector]
    public float currentMoveSpeed;
    //[HideInInspector]
    public float currentHealth;
    //[HideInInspector]
    public float currentDamage;

    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    public void SetEnemyData(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
        StartCoroutine(Kill(3f));
        }
    }

    private IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);
        explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        explosionCollider.enabled = false;
        Destroy(gameObject);
        //wave.EnemyDied();
    }

    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision");
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // use currentDamage
        }
        else if(col.gameObject.CompareTag("Tank_Enemy"))
        {
            Debug.Log("Tank Enemy Collision");
            EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
        else if(col.gameObject.CompareTag("Exploding_Enemy"))
        {
            Debug.Log("exploding Enemy Collision");
            ExplodingEnemyStats enemy = col.gameObject.GetComponent<ExplodingEnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
    }
}
