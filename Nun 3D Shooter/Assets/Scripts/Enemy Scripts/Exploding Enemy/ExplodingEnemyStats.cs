using System;
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
    private CapsuleCollider enemyCollider;
    private HealthBar healthBar;

    [SerializeField]
    public GameObject showTextDamage;

    public float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;
    private float healthCurrency;
    private int currencyAmount;

    public GameObject Explosion;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        enemyCollider = GetComponent<CapsuleCollider>();
        currentMoveSpeed = enemyData.MoveSpeed;
        healthCurrency = enemyData.HealthCurrencyAmount;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currencyAmount = enemyData.CurrencyAmount;
    }

    public void TakeDamage(float dmg)
    {        
        ShowFloatingText(dmg);
        healthBar.ReduceHealth(dmg);

        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill(3f));
        }
    }

    private void ShowFloatingText(float dmg)
    {

        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);
        go.GetComponent<TextMesh>().text = dmg.ToString();
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    private IEnumerator Kill(float delay)
    {
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        enemyCollider.enabled = true;
        isExploding = true;
        gameObject.GetComponent<Animator>().SetBool("Explode", true);
        yield return new WaitForSeconds(delay);
        explosionCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        explosionCollider.enabled = false;

        wave.EnemyDied(currencyAmount);
        CurrencyManager.Instance.AddHealthCurrency((int)Math.Round(healthCurrency));
        Instantiate(Explosion, transform.position + new Vector3(0, 1.79f, 0), Quaternion.identity);
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
