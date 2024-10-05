using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    public GameObject gun;  // Reference to the gun GameObject
    public GameObject meleeWeapon;  // Reference to the melee weapon GameObject

    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private SwordControls meleeWeaponController;
    [HideInInspector]
    public bool isGunActive = false;  // Flag to check if the gun is active

    void Start()
    {

        // Start with the melee weapon enabled and the gun disabled
        meleeWeaponController.enabled = true;
        gunController.enabled = false;
    }

    public void ToggleWeapons()
    {
        if(gunController == null)
        {
            Debug.Log("Gun controller not found.");
        }
        if (gunController.enabled)
        {
            // Switch to melee weapon
            gunController.enabled = false;
            isGunActive = false;
            Debug.Log("Switching to melee weapon");
            meleeWeaponController.enabled = true;
        }
        else
        {
            // Switch to gun
            gunController.enabled = true;
            isGunActive = true;
            Debug.Log("Switching to gun");
            meleeWeaponController.enabled = false;
        }
    }
}
