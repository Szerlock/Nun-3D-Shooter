using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DoubleFaceStats : MonoBehaviour
{

    private Animator anim;

    private BossMovement enemyMovement;
    public EnemyScriptableObject enemyData;
    public Wave wave;
    //[SerializeField]
    //public ShowTextDamage showTextDamage;
    private CapsuleCollider enemyCollider;

    //private BossHealthBar healthBar;


    private HashSet<Bullet> hitBullets = new HashSet<Bullet>();

    public bool isStaggered = false;

    [HideInInspector]
    public float currentMoveSpeed;
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    private float maxHealth;
    private bool nextStage = false;

    public GameObject StaggerVFX;


    void Awake()
    {
        //healthBar = GetComponentInChildren<BossHealthBar>();
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();
        enemyMovement = GetComponent<BossMovement>();
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        maxHealth = currentHealth;
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {

        //ShowFloatingText(dmg);
        if (currentHealth <= 50)
        {
            currentHealth = 50;
            return;
        }
        //healthBar.ReduceHealth(dmg);
        currentHealth -= dmg;

        if (currentHealth <= maxHealth/2)
        {
            nextStage = true;
        }
    }

    public bool NextStage()
    {
        return nextStage;
    }

    public void TakeGunDamage(float dmg)
    {

        //ShowFloatingText(dmg);
        if (currentHealth <= 50)
        {
            currentHealth = 50;
            return;
        }
        //healthBar.ReduceHealth(dmg);
        currentHealth -= dmg;
        StartCoroutine(Stagger());
        if (currentHealth <= maxHealth / 2)
        {
            nextStage = true;
        }
    }

    public IEnumerator Stagger()
    {
        StaggerVFX.SetActive(true);
        if (currentHealth <= maxHealth / 2)
        {
            StaggerVFX.SetActive(false);
            yield break;
        }
        if (isStaggered)
        {
            yield break;
        }
        isStaggered = true;
        enemyMovement.enabled = false;
        yield return new WaitForSeconds(3f);
        enemyMovement.enabled = true;
        isStaggered = false;
        StaggerVFX.SetActive(false);
    }

    


    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision");
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 5);
        }
    }
}
