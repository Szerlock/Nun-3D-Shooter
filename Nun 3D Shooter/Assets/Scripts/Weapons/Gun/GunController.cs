using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public GameObject bulletPrefab;  // The bullet prefab to instantiate
    public Transform bulletSpawnPoint;  // The point from where the bullets will be spawned

    //current Stats
    protected float currentDamage;


    void Awake()
    {
        if (weaponData != null)
        {
        currentDamage = weaponData.Damage;
        }

        else
        {
            Debug.LogError("Weapon data not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))  // Fire1 is usually the left mouse button
        {
            Fire();
        }
    }
    
    void Fire()
    {
    // Instantiate the bullet at the bullet spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Set bullet velocity in the direction the player is looking
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = bulletSpawnPoint.forward * weaponData.BulletSpeed;

        // Set bullet damage
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(currentDamage);
        }
        else
        {
            Debug.LogError("Bullet script not found on bullet prefab.");
        }
    }
}
