using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentCooldown = weaponData.CoolDownDuration;   // set cooldown to cooldown duration
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;
        if(currentCooldown <= 0f) //Once the cooldown 0; attack
        {
            Attact();
        }
    }

    protected virtual void Attact()
    {
        currentCooldown = weaponData.CoolDownDuration;
    }
}
