using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public GameObject shotgunShot;  // The bullet prefab to instantiate
    public GameObject vfx;
    public Transform shotgunShotSpawnPoint;  // The point from where the shotgunShot will be spawned

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
    
    public void GunFire()
    {
    // Instantiate the bullet at the bullet spawn point
        GameObject shotgun = Instantiate(shotgunShot, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);
        GameObject vfxOBJ = Instantiate(vfx, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);   
        // Set bullet velocity in the direction the player is looking
        Rigidbody rb = shotgun.GetComponent<Rigidbody>();
        rb.velocity = shotgunShotSpawnPoint.forward * weaponData.BulletSpeed;

        // Set bullet damage
        Bullet[] bullets = shotgun.GetComponentsInChildren<Bullet>();
        foreach(Bullet bullet in bullets)
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
}
