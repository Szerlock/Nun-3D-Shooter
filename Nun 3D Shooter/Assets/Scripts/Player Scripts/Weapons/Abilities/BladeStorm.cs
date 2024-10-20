using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeStorm : MonoBehaviour
{

    public WeaponScriptableObject weaponData;

    [SerializeField]
    private GameObject spinBlade;

    [SerializeField] 
    private GameObject swordProjectile;

    [SerializeField]
    private Transform bladeSpawnPoint;

    [SerializeField]
    private float bladeSpeed = 10f;

    [SerializeField]
    private Blades blades;

    private float bladeCooldown = 0.5f;
    private float nextBladeTime = 0.5f;
    private GameObject target;
    
    private float currentDamage;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        currentDamage = weaponData.Damage;
        speed = weaponData.BulletSpeed;
    }

    private void Update()
    {
        if (bladeCooldown >= 0.5)
        {
            bladeCooldown -= Time.deltaTime;
            target = FindClosestEnemy();
        }
    }

    private GameObject FindClosestEnemy()
    {
        string[] enemyTags = { "Tank_Enemy", "Exploding_Enemy", "Ghost_Enemy", "Imp_Enemy", "DoubleFace_Enemy", "SecondPhase_Enemy" };

        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (string tag in enemyTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestEnemy = enemy;
                    closestDistance = distance;
                }
            }
        }

        return closestEnemy;
    }

    private void ShootSword()
    {
        Instantiate(swordProjectile, bladeSpawnPoint.position, target.transform.rotation);
        blades.SetDamage(currentDamage, speed);
        blades.SetTarget(target.transform);
    }
}
