using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    public GameObject gun;  // Reference to the gun GameObject
    public GameObject meleeWeapon;  // Reference to the melee weapon GameObject

    private GunController gunController;
    private SwordControls meleeWeaponController;

    void Start()
    {
        Transform gun = transform.Find("Gun");
        Transform meleeWeapon = transform.Find("sword");
        gunController = gun.GetComponent<GunController>();
        meleeWeaponController = meleeWeapon.GetComponent<SwordControls>();

        // Start with the melee weapon enabled and the gun disabled
        meleeWeaponController.enabled = true;
        gunController.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))  // Press E to switch to gun
        {
            ToggleWeapons();
        }
    }

    void ToggleWeapons()
    {
        if (gunController.enabled)
        {
            // Switch to melee weapon
            gunController.enabled = false;
            Debug.Log("Switching to melee weapon");
            meleeWeaponController.enabled = true;
        }
        else
        {
            // Switch to gun
            gunController.enabled = true;
            Debug.Log("Switching to gun");
            meleeWeaponController.enabled = false;
        }
    }
}
