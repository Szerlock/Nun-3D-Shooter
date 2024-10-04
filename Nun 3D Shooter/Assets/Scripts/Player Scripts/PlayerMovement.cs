using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerMovement : MonoBehaviour
{ 
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed at which the camera rotates
    private Rigidbody rb;       // Reference to the Rigidbody component
    private Camera mainCamera;  // Reference to the main camera
    //private Animator animatorSword;
    private Animator animatorCharacter;
    
    //Toggle Weapons
    public WeaponToggle weaponToggle;  // Reference to the WeaponToggle script
    public GunController gunController;  // Reference to the GunController script
    public SwordControls swordController;
    GameManager gameManager;   // Reference to the GameManager script

    // Health Recovery System
    [SerializeField]
    public float maxHealth = 100f;            // Player's maximum health
    public float currentHealth;                // Player's current health
    public float healthRecoveryAmount = 10f;   // Amount of health recovered per enemy killed
    public float comboTimeLimit = 1f;          // Time limit to continue the combo
    public int comboHitLimit = 6;              // Number of hits required to finish a combo
    public float recoveryDuration = 2f;        // Duration of the health recovery state
    public float recoveryCooldown = 5f;        // Cooldown period after health recovery

    private int comboCount = 0;                // Current number of hits in the combo
    private float lastAttackTime;              // Time of the last attack
    private bool isRecovering = false;         // Whether the player is in the recovery state
    private float recoveryCooldownTimer = 0f;  // Cooldown timer

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Transform nun = transform.Find("NewIdleNunBaked");
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
        //animatorSword = GetComponentInChildren<Animator>();
        animatorCharacter = nun.GetComponent<Animator>();
        
    }

    public IEnumerator FinishAttack(float time = 1.1f)
    {
        Debug.Log("Finish Attack");
        yield return new WaitForSeconds(time);
        animatorCharacter.SetBool("EndAttack", true);
    }

    void Update()
    {
        if(recoveryCooldown > 0)
        {
            recoveryCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.E))  // Press E to switch to gun
        {
            weaponToggle.ToggleWeapons();
        }

         if (gameManager != null && gameManager.IsGameStarted())
        {
            if(Input.GetButtonDown("Fire1"))
            {
                if(weaponToggle.isGunActive)
                {
                    gunController.GunFire();
                    animatorCharacter.SetTrigger("Shoot");
                }

                else
                {
                    animatorCharacter.SetBool("EndAttack", false);
                    swordController.SwordAttack();
                    animatorCharacter.SetTrigger("Attack"); // Trigger the attack animation

                    comboCount++; // Increment the combo count
                    lastAttackTime = Time.time; // Update the last attack time

                    if(comboCount >= comboHitLimit && !isRecovering && recoveryCooldownTimer <= 0) // Check if the combo limit has been reached
                    {
                        StartRecovery();
                    }
                    // Start the coroutine to handle attack duration
                    StartCoroutine(FinishAttack());  
                    
                }
            }
        }
        HandleMovement();
    }

    private void HandleMovement()
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
        Vector3 moveDirection = (right * moveX + forward * moveZ);
        moveDirection.Normalize();

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        if(moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            animatorCharacter.SetBool("isMoving", true);
        }

        else
        {
            animatorCharacter.SetBool("isMoving", false);
        }
    }

    private void StartRecovery()
    {
        isRecovering = true; // Set the recovery state to true
        Invoke("StopRecovery", recoveryDuration); // Invoke the RecoverHealth method after the recovery duration
    }

    private void StopRecovery()
    {
        isRecovering = false; // Set the recovery state to false
        recoveryCooldownTimer = recoveryCooldown; // Set the recovery cooldown timer
        comboCount = 0; // Reset the combo count
    }

    public void EnemyKilled()
    {
        if(isRecovering)
        {
            currentHealth += healthRecoveryAmount; // Recover health
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health to the maximum value
        }
    }   
}
