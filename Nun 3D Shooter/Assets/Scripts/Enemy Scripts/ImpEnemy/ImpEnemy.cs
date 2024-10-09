using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpEnemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    [SerializeField]
    public ShowTextDamage showTextDamage;

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
        StartCoroutine(showTextDamage.ShowDamage(dmg, transform));

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        //wave.EnemyDied();
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
