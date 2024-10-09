using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    // Start is called before the first frame update
    DoubleFaceStats enemy;
    Transform player;

    public float chargeSpeedMultiplier = 5f;
    public float chargeDuration = 1f;
    public float chargeCooldown = 3f;
    private float timeBetweenCharges = 0.5f;
    private bool isCharging = false;
    private int chargeCount = 0;


    void Start()
    {
        enemy = GetComponent<DoubleFaceStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
        StartCoroutine(NormalMovement());
    }

    // Update is called once per frame
    void Update()
    {   
        if(!isCharging)
        {
            transform.LookAt(player.transform);

            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
    }

    private IEnumerator NormalMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(chargeCooldown);
            StartCoroutine(ChargeAtPlayer());
        }
    }

    private IEnumerator ChargeAtPlayer()
    {
        Debug.Log("Charging at player");
        isCharging = true;
        chargeCount = 0;

        while(chargeCount < 3)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float chargeEndTime = Time.time + chargeDuration;


            while (Time.time < chargeEndTime)
            {
                // Move towards the player at a higher speed during the charge
                transform.position += directionToPlayer * (enemy.currentMoveSpeed * chargeSpeedMultiplier) * Time.deltaTime;
                yield return null;
            }
            
            chargeCount++;
            yield return new WaitForSeconds(timeBetweenCharges);
        }
        isCharging = false;
    }
}
