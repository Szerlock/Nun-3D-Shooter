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
    
    public GameObject spinBlade;
    public GameObject colliderSpin;

    public GameObject bladeStorm;

    public Transform MarthyrSpawn;

    [SerializeField]
    private int bladeSpinDuration = 2;


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

        if (Input.GetKeyDown(KeyCode.Q))
        { 
            spinBlade.SetActive(true);
            colliderSpin.SetActive(true);

            StartCoroutine(DeactivateSpin());
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(bladeStorm, MarthyrSpawn.position, MarthyrSpawn.rotation);
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

    private IEnumerator DeactivateSpin()
    {
        yield return new WaitForSeconds(bladeSpinDuration);
        spinBlade.SetActive(false);
        colliderSpin.SetActive(false);
    }

    private void HandleMovement()
    {
        if (!animatorCharacter.GetBool("IsAttacking") && !animatorCharacter.GetBool("IsShooting"))
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
        if (Input.GetButtonDown("Fire1") && !animatorCharacter.GetBool("IsAttacking") && !animatorCharacter.GetBool("IsShooting"))
        {
            animatorCharacter.SetBool("isMoving", false);

            //Fix Gun animation
            if (weaponToggle.isGunActive)
            {
                animatorCharacter.SetBool("IsShooting", true);
                gunController.GunFire();
                StartCoroutine(FinishShooting());
            }

            else
            {
                animatorCharacter.SetBool("IsAttacking", true);
                swordController.SwordAttack();
                StartCoroutine(FinishAttack());
            }
        }
    }
    private IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(0.7f);
        animatorCharacter.SetBool("IsAttacking", false);
    }

    private IEnumerator FinishShooting()
    {
        yield return new WaitForSeconds(0.7f);
        animatorCharacter.SetBool("IsShooting", false);
    }

    public void IncreaseDuration(int value)
    {
        bladeSpinDuration += value;
    }
}