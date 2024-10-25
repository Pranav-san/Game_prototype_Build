using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{

    //Animator Controller Override (Change Attack Animation Based on Weapon)


    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexhREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int lightiningDamage = 0;

    [Header("Attack Modifiers")]
    public float light_attack_01_Modifier = 1.1f;


    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;

    [Header("Actions")]
    public WeaponItemBasedAction OH_RB_Action;


    [Header("whoosh")]
    public AudioClip[] whooshes;


}
