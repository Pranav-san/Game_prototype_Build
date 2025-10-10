using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : EquipmentItem
{

    //Animator Controller Override (Change Attack Animation Based on Weapon)
    [Header("Animator")]
    public AnimatorOverrideController weaponAnimator;


    [Header("Weapon Model")]
    public GameObject weaponModel;

    [Header("Model Instantiation")]
    public WeaponModelType weaponModelType;
    public WeaponWieldingType weaponWieldingType;

    [Header("Weapon Class")]
    public WeapomClass weaponClass;

    [Header("Weapon Requirements")]
    public int strengthREQ = 0;
    public int dexhREQ = 0;
    public int faithREQ = 0;

    [Header("Weapon Base Damage")]
    public int physicalDamage = 0;
    public int magicDamage = 0;
    public int fireDamage = 0;
    public int lightiningDamage = 0;

    [Header("Poise Damage")]
    public float PoiseDamage  = 0;

    [Header("Attack Modifiers")]
    public float light_attack_01_Modifier = 1.1f;
    public float heavy_attack_01_Modifier = 1.4f;
    public float charged_attack_01_Modifier = 2.0f;




    [Header("Stamina Costs")]
    public int baseStaminaCost = 20;
    public float lightAttackStaminaCostMultiplier = 0.9f;
    public float heavyAttackStaminaCostMultiplier = 0.9f;

    [Header("weapon Blocking Absorption")]
    public float physicalBaseDamageAbsorption = 75;
    public float fireBaseDamageAbsorption = 40;
    public float magicBaseDamageAbsorption = 40;
    public float lightningBaseDamageAbsorption = 20;

    public float stabilty = 20;//Reduces stamina lost from Blocking
    


    [Header("Actions")]
    public WeaponItemBasedAction OH_RB_Action; // Light Attack
    public WeaponItemBasedAction OH_RT_Action; // Heavy Attack
    public WeaponItemBasedAction OH_LB_Action; // One Handed Left Bumper Action (Blocking)



    [Header("whoosh")]
    public AudioClip[] whooshes;
    public AudioClip[] blocking;


    [Header("VFX")]
    public GameObject vfx;


}
