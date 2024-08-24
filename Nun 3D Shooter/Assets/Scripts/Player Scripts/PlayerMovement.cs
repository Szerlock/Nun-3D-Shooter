using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 5f; // Speed of movement
    private Rigidbody rb;       // Reference to the Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    void Update()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal"); // Left/Right movement (A/D or Left/Right arrows)
        float moveZ = Input.GetAxis("Vertical");   // Forward/Backward movement (W/S or Up/Down arrows)

        // Calculate movement direction
        Vector3 moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        // Apply movement
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }
}
