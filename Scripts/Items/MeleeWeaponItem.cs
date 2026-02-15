using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/WeaponItems/MeleeWeapon")]
public class MeleeWeaponItem : WeaponItem
{

    [Header("Attack Modifiers")]
    public float riposte_Attack_01_Modifier = 3.5f;
    public float backstab_Attack_01_Modifier = 4.5f;

    [Header("Wall Recoil")]
    public float wallCheckDistance = 0.6f;
    public float wallCheckRadius = 0.15f;

}
