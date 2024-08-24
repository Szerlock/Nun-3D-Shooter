using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 5f; // Speed of movement
    private Rigidbody rb;       // Reference to the Rigidbody component
    private Camera mainCamera;  // Reference to the main camera

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
    }

    void Update()
    {
        // Get input for movement
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical");   

        // Convert input to camera-relative direction
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        // Normalize vectors to ensure consistent movement speed
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction
        Vector3 moveDirection = (right * moveX + forward * moveZ).normalized;

        // Apply movement
        Vector3 moveVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
    }
}
