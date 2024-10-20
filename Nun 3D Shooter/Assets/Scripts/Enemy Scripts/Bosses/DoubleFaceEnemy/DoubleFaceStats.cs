using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceStats : MonoBehaviour
{
    [SerializeField]
    public GameObject secondPhase;

    private Animator anim;

    private BossMovement enemyMovement;
    public EnemyScriptableObject enemyData;
    private Wave wave;
    [SerializeField]
    public ShowTextDamage showTextDamage;
    private CapsuleCollider enemyCollider;

    private HashSet<Bullet> hitBullets = new HashSet<Bullet>();

    public bool isStaggered = false;

    [HideInInspector]
    public float currentMoveSpeed;
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;

    private float maxHealth;
    private bool nextStage = false;

    void Awake()
    {
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

        ShowFloatingText(dmg);

        currentHealth -= dmg;
        if(currentHealth <= maxHealth/2)
        {
            //StartCoroutine(Kill());
            StartCoroutine(ActivateSecondPhase());
        }

        if (currentHealth <= maxHealth/2)
        {
            nextStage = true;
            NextStage();
        }

        StartCoroutine(Stagger());
    }

    private IEnumerator ActivateSecondPhase()
    {
        anim.SetBool("Change", true);
        yield return new WaitForSeconds(0.45f);
        Instantiate(secondPhase, transform.position, transform.rotation);
        this.enabled = false;
    }

    public bool NextStage()
    {
        return nextStage;
    }

    public IEnumerator Stagger()
    {
        isStaggered = true;
        enemyMovement.enabled = false;
        yield return new WaitForSeconds(10f);
        enemyMovement.enabled = true;
        isStaggered = false;
    }

    private void ShowFloatingText(float dmg)
    {
        Transform cameraTransform = Camera.main.transform;

        var go = Instantiate(showTextDamage, transform.position, Quaternion.LookRotation(transform.position - cameraTransform.position), transform);

        go.GetComponent<TextMesh>().text = dmg.ToString();
    }

    //private IEnumerator Kill()
    //{
    //    enemyCollider.enabled = false;
    //    yield return new WaitForSeconds(0.3f);

    //    Destroy(gameObject);
    //    wave.EnemyDied(100);
    //}

    protected void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Collision");
            PlayerStats player = col.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage, transform.position, 5);
        }
    }

    public bool IsHitBy(Bullet bullet)
    {
        if (hitBullets.Contains(bullet))
        {
            return true;
        }

        hitBullets.Add(bullet);
        return false;
    }
}
