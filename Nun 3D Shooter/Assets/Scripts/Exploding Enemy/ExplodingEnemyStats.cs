using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
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
        StartCoroutine(Kill(7f));
        }
    }

    private IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
        wave.EnemyDied();
    }

    private void OnCollisionStay(Collision col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage); // use currentDamage
        }
    }
}
