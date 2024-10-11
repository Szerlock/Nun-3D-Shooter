using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
    private PlayerMovement movementScript;

    public float dashSpeed;
    public float dashTime;
    public float dashCooldown = 3f;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldown <= 0)
        {
            Dash();
        }
        if(dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
    }

    public void  Dash()
    {
        float startTime = Time.time;
        float originalMoveSpeed = movementScript.moveSpeed;
        movementScript.moveSpeed = dashSpeed; // Set the speed for the dash

        while (Time.time < startTime + dashTime)
        {
            // Move with dash speed
            Vector3 moveDirection = (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")).normalized;
            transform.Translate(moveDirection * dashSpeed * Time.deltaTime, Space.World);
        }

        // Reset the moveSpeed to default speed after dashing
        movementScript.moveSpeed = originalMoveSpeed;
        dashCooldown = 3f;
        Debug.Log("Dash ended. Speed reset to: " + originalMoveSpeed);
    }
}
