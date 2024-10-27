using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [Header("CooldownsForAbilities")]
    private float bladeStormCooldown = 0f;
    private float bladeSpinCooldown = 0f;
    private float healthPotionDuration = 0f;

    //public TextMeshProUGUI bladeStormCooldownText;
    //public TextMeshProUGUI bladeSpinCooldownText;
    //public TextMeshProUGUI healthPotionText;

    public bool Unlocked = false;
    private bool activateStormBlade = false;
    public Transform SpawnUnlocked;

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
        //if (bladeStormCooldown > 0 && !Unlocked)
        //{
        //    bladeStormCooldown -= Time.deltaTime;
        //    // Can change to 2 decimal places
        //    bladeStormCooldownText.text = bladeStormCooldown.ToString("F0");
        //}
        //else if (bladeStormCooldown < 0 && !Unlocked)
        //{
        //    bladeStormCooldownText.text = string.Empty;
        //}

        //if (bladeSpinCooldown > 0)
        //{
        //    bladeSpinCooldown -= Time.deltaTime;
        //    bladeSpinCooldownText.text = bladeSpinCooldown.ToString("F0");
        //}
        //else if (bladeSpinCooldown < 0)
        //{
        //    bladeSpinCooldownText.text = string.Empty;
        //}

        //if (healthPotionDuration > 0)
        //{
        //    healthPotionDuration -= Time.deltaTime;
        //    healthPotionText.text = healthPotionDuration.ToString("F0");
        //}
        //else if (healthPotionDuration < 0)
        //{
        //    healthPotionText.text = string.Empty;
        //}

        if (Input.GetKeyDown(KeyCode.E))  // Press E to switch to gun
        {
            weaponToggle.ToggleWeapons();
        }

        if (Input.GetKeyDown(KeyCode.Q) && bladeSpinCooldown <= 0 && !Unlocked)
        {
            bladeSpinCooldown = 10f;
            spinBlade.SetActive(true);
            colliderSpin.SetActive(true);

            StartCoroutine(DeactivateSpin());
        }
        else if (Unlocked)
        { 
            spinBlade.SetActive(true);
            colliderSpin.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.T) && bladeStormCooldown <= 0 && !Unlocked)
        {
            bladeStormCooldown = 20f;
            Instantiate(bladeStorm, MarthyrSpawn.position, MarthyrSpawn.rotation);
        }
        else if (Unlocked && !activateStormBlade)
        {
            activateStormBlade = true;
            bladeStormCooldown = - 1;
            Instantiate(bladeStorm, SpawnUnlocked.position, SpawnUnlocked.rotation);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (CurrencyManager.Instance.CurrentHealthPotions > 0)
            {
                healthPotionDuration = 4f;
                CurrencyManager.Instance.SpendHealthPotion();
                playerStats.RestoreHealth();
            }
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
                if (gunController.currentGunCapacity >= 1)
                {
                    animatorCharacter.SetBool("IsShooting", true);
                    gunController.GunFire();
                    StartCoroutine(FinishShooting());
                }
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