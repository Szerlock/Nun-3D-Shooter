using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    private PlayerMovement movementScript;

    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 6f;
    private SphereCollider sphereCollider;

    private static float damageIncrease = 0;

    // Start is called before the first frame update
    void Start()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0)
        {
            StartCoroutine(Dash());
        }
        if(dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    public IEnumerator Dash()
    {
        float startTime = Time.time;
        movementScript.moveSpeed = dashSpeed;
        sphereCollider.enabled = true;
        while (Time.time < startTime + dashTime)
        {
            // Move with dash speed
            Vector3 moveDirection = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            transform.Translate(moveDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        // Reset the moveSpeed to default speed after dashing
        movementScript.ResetMoveSpeed();
        dashCooldown = 3f;
        sphereCollider.enabled = false;
    }

    public float GetDamage()
    {
        return damageIncrease;
    }

    public void IncreaseDamage(float damage)
    {
        damageIncrease *= (1 + damage);
    }

    public void Damage(float dmg)
    { 
        damageIncrease += dmg;
    }

    public void DecreaseCooldown(float value)
    {
        dashCooldown -= value;
    }
}
