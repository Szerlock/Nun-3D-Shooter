using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// base script for all weapon controllers
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName ="ScriptableObject/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    //Base Stats for weapons
    [SerializeField] 
    float damage;
    public float Damage {get => damage; private set => damage = value;}

    [SerializeField]
    float coolDownDuration;
    public float CoolDownDuration {get => coolDownDuration; private set => coolDownDuration = value;}
    
}
