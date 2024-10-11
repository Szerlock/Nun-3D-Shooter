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

    [Header("SwordAnim")]
    public float spamWindow = 1.5f;
    public float lastAttackTime = 0;
    private int attackPhase = 0;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>(); // Get the PlayerStats component
        Transform nun = transform.Find("NewIdleNunBaked");
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
        //animatorSword = GetComponentInChildren<Animator>();
        animatorCharacter = nun.GetComponent<Animator>();
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = 5f;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))  // Press E to switch to gun
        {
            weaponToggle.ToggleWeapons();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats not found.");
            }
            playerStats.RestoreHealth(CurrencyManager.Instance.SpendHealthCurrency());
        }

        if (GameManager.instance.IsGameStarted())
        {
            HandleAttack();
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!animatorCharacter.GetBool("IsAttacking") && !animatorCharacter.GetBool("IsSecondAttack") && attackPhase != 1)
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

    private void HandleAttack()
    {
        if (Time.time - lastAttackTime > spamWindow)
        {
            attackPhase = 0;
        }

        if (Input.GetButtonDown("Fire1") && !animatorCharacter.GetBool("IsAttacking"))
        {
            animatorCharacter.SetBool("isMoving", false);

            //Fix Gun animation
            if (weaponToggle.isGunActive)
            {
                animatorCharacter.SetBool("IsShooting", true);
                gunController.GunFire();
                lastAttackTime = Time.time;
                StartCoroutine(FinishShooting());
            }

            else if (attackPhase == 0 && !animatorCharacter.GetBool("IsSecondAttack"))
            {
                animatorCharacter.SetBool("IsAttacking", true);
                swordController.SwordAttack();
                attackPhase = 1;
                lastAttackTime = Time.time;
                StartCoroutine(FinishAttack());
            }

            else if (attackPhase == 1 && !animatorCharacter.GetBool("IsSecondAttack"))
            {
                animatorCharacter.SetBool("IsSecondAttack", true);
                swordController.SwordAttack();
                attackPhase = 0;
                lastAttackTime = Time.time;
                StartCoroutine(FinishSecondAttack());
            }
        }
    }

    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(0.26f);
        animatorCharacter.SetBool("IsAttacking", false);
    }

    private IEnumerator FinishSecondAttack()
    {
        yield return new WaitForSeconds(0.8f);
        animatorCharacter.SetBool("IsSecondAttack", false);
    }

    private IEnumerator FinishShooting()
    {
        yield return new WaitForSeconds(1f);
        animatorCharacter.SetBool("IsShooting", false);
    }
}
