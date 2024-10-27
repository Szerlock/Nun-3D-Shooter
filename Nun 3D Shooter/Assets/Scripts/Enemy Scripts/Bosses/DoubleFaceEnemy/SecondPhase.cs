using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPhase : MonoBehaviour
{

    private SecondPhaseMovement enemyMovement;
    public EnemyScriptableObject enemyData;
    private Wave wave;
    [SerializeField]
    public ShowTextDamage showTextDamage;
    private CapsuleCollider enemyCollider;

    private BossHealthBar healthBar;

    private HashSet<Bullet> hitBullets = new HashSet<Bullet>();

    public bool isStaggered = false;

    [HideInInspector]
    public float currentMoveSpeed;
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    private bool nextStage = true;

    private Bullet currentBullet;

    public GameObject StaggerVFX;


    void Awake()
    {
        healthBar = GetComponentInChildren<BossHealthBar>();
        enemyCollider = GetComponent<CapsuleCollider>();
        enemyMovement = GetComponent<SecondPhaseMovement>();
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = 50f;
        currentDamage = enemyData.Damage;
    }

    public void SetWave(Wave wave)
    {
        this.wave = wave;
    }

    public void TakeDamage(float dmg)
    {

        //ShowFloatingText(dmg);

        currentHealth -= dmg;
        //healthBar.ReduceHealth(dmg);

        if (currentHealth <= 0)
        {
            StartCoroutine(Kill());
        }
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
    }

    public IEnumerator Stagger()
    {
        StaggerVFX.SetActive(true);
        isStaggered = true;
        enemyMovement.enabled = false;
        yield return new WaitForSeconds(3f);
        enemyMovement.enabled = true;
        isStaggered = false;
        StaggerVFX.SetActive(false);
    }

    public bool NextStage()
    {
        return nextStage;
    }

    private void ShowFloatingText(float dmg)
    {
        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);

        go.GetComponent<TextMesh>().text = dmg.ToString();
    }

    private IEnumerator Kill()
    {
        enemyCollider.enabled = false;
        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
        wave.EnemyDied(100);
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
