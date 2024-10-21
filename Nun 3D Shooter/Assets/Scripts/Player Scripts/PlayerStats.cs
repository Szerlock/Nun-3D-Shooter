using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    public CharacterScriptableObject characterData;

    [SerializeField]
    private float pushBackForce;
    //Current Stats
    private float currentHealth;
    private float maxHealth;
    [SerializeField]
    Corpse corpse;


    //I-Frames
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invincibilityTimer;
    bool isInvincible;

    void Awake()
    {
        currentHealth = characterData.MaxHealth;
        maxHealth = characterData.MaxHealth;
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

    public void TakeDamage(float dmg, Vector3 damageSourcePosition, float pushBackForce)
    {
        if(!isInvincible)
        {
            //if (pushBackForce < 1)
            //{
            //    return;
            //}
            currentHealth -= dmg;

            Vector3 pushDirection = (transform.position - damageSourcePosition).normalized;
            Rigidbody playerRigidbody = GetComponent<Rigidbody>();
            playerRigidbody.AddForce(pushDirection * pushBackForce, ForceMode.Impulse);

            invincibilityTimer = invincibilityDuration;
            isInvincible = true;
            HealingOrb.instance.ModifyHealth(-dmg);
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
    }

    public void Kill()
    {
        corpse.AddCorpse(transform.position);
        GameManager.instance.OpenRestartMenu();
    }

    public void RestoreHealth(float amount)
    {
        //1 if statement makes it so it only heals if he is below max health
        if(currentHealth < maxHealth)
        {
            currentHealth += amount;

            //to not overheal over max health
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
        HealingIcon.instance.ModifyHealth(-100);
        HealingOrb.instance.ModifyHealth(amount);
    }

    public void IncreaseHealth(int value)
    {
        maxHealth += value;
        if (currentHealth < maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
