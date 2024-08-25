using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed at which the camera rotates
    private Rigidbody rb;       // Reference to the Rigidbody component
    private Camera mainCamera;  // Reference to the main camera
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            animator.SetBool("EndAttack", true);
        }

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
        Vector3 moveDirection = (right * moveX + forward * moveZ);
        moveDirection.Normalize();

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        // Apply movement
        //Vector3 moveVelocity = moveDirection * moveSpeed;
        //rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

    }
}
