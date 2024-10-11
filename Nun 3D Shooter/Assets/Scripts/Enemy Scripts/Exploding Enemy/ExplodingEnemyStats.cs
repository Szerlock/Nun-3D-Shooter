using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [SerializeField]
    SphereCollider explosionCollider;
    private Wave wave;
    private bool isExploding = false;

    [SerializeField]
    public ShowTextDamage showTextDamage;

    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    private int healthCurrency;
    [HideInInspector]
    public int currencyAmount;

    void Awake()
    {
        healthCurrency = enemyData.HealthCurrencyAmount;
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currencyAmount = enemyData.CurrencyAmount;
    }

    public void TakeDamage(float dmg)
    {
        StartCoroutine(showTextDamage.ShowDamage(dmg, transform));

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill(3f));
        }
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    private IEnumerator Kill(float delay)
    {
        isExploding = true;
        gameObject.GetComponent<Animator>().SetBool("Explode", true);
        yield return new WaitForSeconds(delay);
        explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        explosionCollider.enabled = false;

        wave.EnemyDied(currencyAmount);
        CurrencyManager.Instance.AddHealthCurrency(healthCurrency);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 8);    
        }
        else if(col.gameObject.CompareTag("Tank_Enemy"))
        {
            EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
        else if(col.gameObject.CompareTag("Exploding_Enemy"))
        {
            ExplodingEnemyStats enemy = col.gameObject.GetComponent<ExplodingEnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
        else if(col.gameObject.CompareTag("DoubleFace_Enemy"))
        {
            DoubleFaceStats enemy = col.gameObject.GetComponent<DoubleFaceStats>();
            enemy.TakeDamage(currentDamage/2); // use currentDamage
        }
        else if(col.gameObject.CompareTag("Imp_Enemy"))
        {
            ImpEnemy enemy = col.gameObject.GetComponent<ImpEnemy>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
        else if(col.gameObject.CompareTag("Ghost_Enemy"))
        {
            GhostEnemyStats enemy = col.gameObject.GetComponent<GhostEnemyStats>();
            enemy.TakeDamage(currentDamage); // use currentDamage
        }
    }

    public bool IsExploding()
    {
        return isExploding;
    }
}
