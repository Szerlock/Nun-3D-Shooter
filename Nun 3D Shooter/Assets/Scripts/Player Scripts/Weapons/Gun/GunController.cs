using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public GameObject shotgunShot;
    public GameObject vfx;
    public Transform shotgunShotSpawnPoint;

    private int gunCapacity = 4;
    private float reloadTime = 7f;
    private int currentGunCapacity = 4;
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

    private void Update()
    {
        if (reloadTime >= 0)
        {
            reloadTime -= Time.deltaTime;
        }
        if (reloadTime <= 0 && currentGunCapacity < gunCapacity)
        {
            currentGunCapacity++;

            // Implement showing icons for gun capacity
            Debug.Log(currentGunCapacity);

            if (currentGunCapacity < gunCapacity)
            {
                reloadTime = 7f;
            }
        }
        else if (currentGunCapacity == gunCapacity)
        {
            reloadTime = 7f;
        }
    }

    public void GunFire()
    {
        if (currentGunCapacity > 0)
        {
            currentGunCapacity--;

            //Update icons for 1 less bullet capacity
            Debug.Log(currentGunCapacity);
            Fire();
        }
        else
        {
            Debug.Log("Out of ammo");
        }
    }

    private void Fire()
    {
        GameObject shotgun = Instantiate(shotgunShot, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);
        GameObject vfxOBJ = Instantiate(vfx, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);
        // Set bullet velocity in the direction the player is looking
        Rigidbody rb = shotgun.GetComponent<Rigidbody>();
        rb.velocity = shotgunShotSpawnPoint.forward * weaponData.BulletSpeed;

        // Set bullet damage
        Bullet[] bullets = shotgun.GetComponentsInChildren<Bullet>();
        foreach (Bullet bullet in bullets)
        {
            if (bullet != null)
            {
                bullet.SetDamage(currentDamage);
            }
            else
            {
                Debug.LogError("Bullet script not found on bullet prefab.");
            }
        }
        Destroy(shotgun, 2f);
        Destroy(vfxOBJ, 0.25f);
    }

    public void ChangeCurrentReloadSpeed(float value)
    {
        reloadTime -= value;
    }

    public void ChangeCurrentDamage(float value)
    {
        currentDamage *= (1 + value);
    }
}
