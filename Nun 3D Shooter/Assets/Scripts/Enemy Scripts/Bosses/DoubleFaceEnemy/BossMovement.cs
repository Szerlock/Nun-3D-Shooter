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

    public float chargeSpeedMultiplier = 4f;
    public float chargeDuration = 1f;
    public float chargeCooldown = 5f;
    private float timeBetweenCharges = 0.5f;
    private bool isCharging = false;
    private int chargeCount = 0;

    private float restingTime = 3f;
    private bool IsResting = false;


    void Start()
    {
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
        if(chargeCooldown <= 0 && !isCharging && !IsResting)
        {
            StartCoroutine(ChargeAtPlayer());
        }
        if (!isCharging && !IsResting)
        {
            //transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
            //transform.LookAt(player.transform);
            //Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
            MoveTowardsPlayer();
            //rb.MovePosition(newPosition);
        }
        else if (IsResting)
        {
            StartCoroutine(Resting());
        }

    }

    private void MoveTowardsPlayer()
    {
        transform.LookAt(player.transform);
        Vector3 newPosition = Vector3.MoveTowards(rb.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);

        rb.MovePosition(newPosition);
    }


    private IEnumerator Resting()
    {
        Unity.Mathematics.Random random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
        yield return new WaitForSeconds(random.NextFloat(3f, 6f));
        IsResting = false;
    }

    private IEnumerator ChargeAtPlayer()
    {
        isCharging = true;
        chargeCount = 0;

        while(chargeCount < 3)
        {
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
