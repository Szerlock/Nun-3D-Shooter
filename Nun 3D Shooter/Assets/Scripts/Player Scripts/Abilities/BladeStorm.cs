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

    private float bladeCooldown = 2f;
    private GameObject target;

    private float duration;
    
    private float currentDamage;
    private float speed;

    private int swordAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        duration = weaponData.CoolDownDuration;
        currentDamage = weaponData.Damage;
        speed = weaponData.BulletSpeed;
    }

    private void Update()
    {
        if (bladeCooldown <= 0)
        {
            bladeCooldown = 2f;
            FindClosestEnemy();
        }
        else
        {
            bladeCooldown -= Time.deltaTime;
        }
        if (duration >= 0)
        {
            duration -= Time.deltaTime;
        }
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FindClosestEnemy()
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
        target = closestEnemy;
        ShootSword();
    }

    private void ShootSword()
    {
        for (int i = 0; i < swordAmount; i++)
        {
            GameObject sword = Instantiate(swordProjectile, bladeSpawnPoint.position, target.transform.rotation);
            sword.GetComponent<Blades>().SetDamage(currentDamage, speed);
            sword.GetComponent<Blades>().SetTarget(target.transform);
        }
    }

    public void ChangeCurrentDamage(float value)
    {
        currentDamage *= (1 + value);
    }

    public void IncreaseDuration(int value)
    {
        duration += value;
    }

    public void IncreaseSwordCount(int value)
    { 
        swordAmount += value;
    }
}
