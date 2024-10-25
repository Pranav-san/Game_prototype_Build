using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public MeleeWeaponDamageCollider meleeDamageCollider;

    private void Awake()
    {
        meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
    }

    public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
    {
        meleeDamageCollider.characterCausingDamage = characterWieldingWeapon;
        meleeDamageCollider.physicalDamage = weapon.physicalDamage;
        meleeDamageCollider.magicDamage = weapon.magicDamage;
        meleeDamageCollider.firelDamage = weapon.fireDamage;
        meleeDamageCollider.lightininglDamage =weapon.lightiningDamage;

        meleeDamageCollider.light_Attack_01_Modifier = weapon.light_attack_01_Modifier;
    }
}