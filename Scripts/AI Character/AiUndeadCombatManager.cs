using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiUndeadCombatManager : AICharacterCombatManager
{
    [Header("Damage Colliders")]
    [SerializeField]ManualDamageCollider rightDamageCollider;
    //[SerializeField]ManualDamageCollider leftDamageCollider;

    [Header("Damage Modifiers")]
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;

    public void SetAttack01Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
        //leftDamageCollider.physicalDamage = baseDamage*attack01DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        //leftDamageCollider.physicalDamage = baseDamage*attack02DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack02DamageModifier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightDamageCollider.DisableDamageCollider();
    }




}
