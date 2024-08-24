using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public CharacterScriptableObject characterData;

    //Current Stats
    //[HideInInspector]
    public float currentHealth;
   // [HideInInspector]
    public float currentMoveSpeed;
   // [HideInInspector]
    public float currentProjectileSpeed;

    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    void Awake()
    {
        currentHealth = characterData.MaxHealth;
        currentMoveSpeed = characterData.MoveSpeed;
        currentProjectileSpeed = characterData.ProjectileSpeed;
    }

    private void Update()
    {
        if(invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }
        else if(isInvincible)
        {
            isInvincible = false;
        }
    }

    public void TakeDamage(float dmg)
    {
        if(!isInvincible)
        {
            currentHealth -= dmg;

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;

            if(currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        Debug.Log("player DEAD");
    }

    public void RestoreHealth(float amount)
    {
        //1 if statement makes it so it only heals if he is below max health
        if(currentHealth < characterData.MaxHealth)
        {
            currentHealth += amount;

            //to not overheal over max health
            if(currentHealth > characterData.MaxHealth)
            {
                currentHealth = characterData.MaxHealth;
            }
        }
    }
}
