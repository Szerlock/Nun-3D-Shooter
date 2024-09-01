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
    //private Animator animatorSword;
    private Animator animatorCharacter;
    
    //Toggle Weapons
    public WeaponToggle weaponToggle;  // Reference to the WeaponToggle script
    public GunController gunController;  // Reference to the GunController script
    public SwordControls swordController;
    GameManager gameManager;   // Reference to the GameManager script

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Transform nun = transform.Find("NunRun");
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        mainCamera = Camera.main;       // Get the main camera
        //animatorSword = GetComponentInChildren<Animator>();
        animatorCharacter = nun.GetComponent<Animator>();
        
    }

    void Update()
    {

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
                }

                else
                {
                    swordController.SwordAttack();
                }
            }
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
            animatorCharacter.SetBool("isMoving", true);
        }

        else
        {
            animatorCharacter.SetBool("isMoving", false);
        }
    }
}
