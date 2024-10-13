using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class ImpEnemy : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    private Wave wave;
    [SerializeField]
    public GameObject showTextDamage;
    private ImpMovement enemyMovement;

    public Animator animatorController { get; set; }

    public bool IsAttacking { get; set; }

    public float currentMoveSpeed;
    private float currentHealth;
    private float currentDamage;
    private float healthCurrency;
    private int currencyAmount; 

    void Awake()
    {
        animatorController = GetComponent<Animator>();
        currentMoveSpeed = enemyData.MoveSpeed;
        enemyMovement = GetComponentInParent<ImpMovement>();
        healthCurrency = enemyData.HealthCurrencyAmount;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        currencyAmount = enemyData.CurrencyAmount;
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {
        //StartCoroutine(showTextDamage.ShowDamage(dmg, transform)); 
        Instantiate(showTextDamage, transform.position, Quaternion.identity, transform);


        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }

        enemyMovement.enabled = false;
        StartCoroutine(Stagger());
    }

    private IEnumerator Stagger()
    {
        yield return new WaitForSeconds(1f);
        enemyMovement.enabled = true;
    }

    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);

        
        wave.EnemyDied(currencyAmount);
        CurrencyManager.Instance.AddHealthCurrency((int)System.Math.Round(healthCurrency));
        Destroy(transform.parent.gameObject);
        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            IsAttacking = true;
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 0); // use currentDamage
        }
    }
}
