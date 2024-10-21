using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public GameObject shotgunShot;
    public GameObject vfx;
    public Transform shotgunShotSpawnPoint;

    private int gunCapacity = 7;
    [SerializeField]
    private float reloadTime = 15f;
    public int currentGunCapacity { get; set; } = 7;

    [SerializeField]
    public TextMeshProUGUI text;

    [SerializeField]
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

            text.text = currentGunCapacity.ToString();

            if (currentGunCapacity < gunCapacity)
            {
                reloadTime = 15f;
            }
        }
        else if (currentGunCapacity == gunCapacity)
        {
            reloadTime = 15f;
        }
    }

    public void GunFire()
    {
        if (currentGunCapacity > 0)
        {
            currentGunCapacity--;

            text.text = currentGunCapacity.ToString();
            Fire();
        }
    }

    private void Fire()
    {
        GameObject shotgun = Instantiate(shotgunShot, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);
        GameObject vfxOBJ = Instantiate(vfx, shotgunShotSpawnPoint.position, shotgunShotSpawnPoint.rotation);
        Rigidbody rb = shotgun.GetComponent<Rigidbody>();
        rb.velocity = shotgunShotSpawnPoint.forward * weaponData.BulletSpeed;

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
