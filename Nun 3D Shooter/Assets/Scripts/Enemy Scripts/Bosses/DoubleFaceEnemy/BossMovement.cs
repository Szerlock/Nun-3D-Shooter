using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    // Start is called before the first frame update
    DoubleFaceStats enemy;
    Transform player;
    Rigidbody rb;


    public Transform changeSpot;

    public float chargeSpeedMultiplier = 4f;
    public float chargeDuration = 1f;
    public float chargeCooldown = 5f;
    private float timeBetweenCharges = 0.5f;
    private bool isCharging = false;
    private int chargeCount = 0;

    private Animator anim;

    private float restingTime = 3f;
    private bool IsResting = false;

    private bool isStaggered = false;

    [SerializeField]
    public Vector3 spawnNewBoss;

    [SerializeField]
    public GameObject secondPhase;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        enemy = GetComponent<DoubleFaceStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (chargeCooldown > 0)
        {
            chargeCooldown -= Time.deltaTime;
        }
        if (!enemy.NextStage())
        {
            if (chargeCooldown <= 0 && !isCharging && !IsResting)
            {
                StartCoroutine(ChargeAtPlayer());
            }
        }
        if (enemy.NextStage())
        {
            StartCoroutine(MoveTowardsChangeSpot());
        }
        else if (!isCharging && !IsResting)
        {
            MoveTowardsPlayer();
        }
        else if (IsResting)
        {
            StartCoroutine(Resting());
        }

        if (enemy.isStaggered)
        {
            anim.SetBool("IsMoving", false);
        }
        else if (!enemy.isStaggered)
        {
            anim.SetBool("IsMoving", true);
        }
    }

    private IEnumerator MoveTowardsChangeSpot()
    {
        transform.LookAt(changeSpot.transform);
        Vector3 newPosition = Vector3.MoveTowards(rb.position, changeSpot.position, enemy.currentMoveSpeed * 7 * Time.deltaTime);

        rb.MovePosition(newPosition);
        yield return new WaitForSeconds(5f);
        GameObject temp = Instantiate(secondPhase, spawnNewBoss, transform.rotation);
        SecondPhase secondTemp = temp.GetComponent<SecondPhase>();
        secondTemp.SetWave(enemy.wave);
        Destroy(gameObject);
    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(player.transform);
        Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);

        rb.MovePosition(newPosition);
    }


    private IEnumerator Resting()
    {
        anim.SetBool("IsMoving", false);
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        yield return new WaitForSeconds(random.NextFloat(3f, 6f));
        IsResting = false;
        anim.SetBool("IsMoving", true);
    }

    private IEnumerator ChargeAtPlayer()
    {
        isCharging = true;
        chargeCount = 0;

        while (chargeCount < 3)
        {
            if (enemy.isStaggered)
            {
                isCharging = false;
                yield break;
            }

            transform.LookAt(player.transform);
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float chargeEndTime = Time.time + chargeDuration;


            while (Time.time < chargeEndTime)
            {
                transform.position += directionToPlayer * (enemy.currentMoveSpeed * chargeSpeedMultiplier) * Time.deltaTime;
                yield return null;
            }

            chargeCount++;
            yield return new WaitForSeconds(timeBetweenCharges);
        }

        chargeCooldown = 5f;
        isCharging = false;
        IsResting = true;
        yield return new WaitForSeconds(restingTime);
    }
}
