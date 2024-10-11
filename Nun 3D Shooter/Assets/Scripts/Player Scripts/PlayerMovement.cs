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
    private Animator animatorCharacter;
    PlayerStats playerStats;  // Reference to the PlayerStats script

    //Toggle Weapons
    public WeaponToggle weaponToggle;  // Reference to the WeaponToggle script
    public GunController gunController;  // Reference to the GunController script
    public SwordControls swordController;
    GameManager gameManager;   // Reference to the GameManager script

    void Start()
    {
        playerStats = GetComponent<PlayerStats>(); // Get the PlayerStats component
        gameManager = FindObjectOfType<GameManager>();
        Transform nun = transform.Find("NewIdleNunBaked");
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
        //animatorSword = GetComponentInChildren<Animator>();
        animatorCharacter = nun.GetComponent<Animator>();
        
    }

    public IEnumerator FinishAttack(float time = 1.09f)
    {
        yield return new WaitForSeconds(time);
        animatorCharacter.SetBool("IsAttacking", false);

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))  // Press E to switch to gun
        {
            weaponToggle.ToggleWeapons();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(playerStats == null)
            {
                Debug.LogError("PlayerStats not found.");
            }
            playerStats.RestoreHealth(CurrencyManager.Instance.SpendHealthCurrency());
        }

         if (gameManager != null && gameManager.IsGameStarted())
        {
            if(Input.GetButtonDown("Fire1"))
            {
                animatorCharacter.SetBool("isMoving", false);
                animatorCharacter.SetBool("IsAttacking", true);

                //Fix Gun animation
                if (weaponToggle.isGunActive)
                {
                    gunController.GunFire();
                    animatorCharacter.SetTrigger("Shoot");
                }

                else
                {
                    swordController.SwordAttack();
                    StartCoroutine(FinishAttack());
                }
            }
        }
           HandleMovement();
    }

    private void HandleMovement()
    {
        if (!animatorCharacter.GetBool("IsAttacking"))
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

            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            if (moveDirection != Vector3.zero)
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
    } 
}
