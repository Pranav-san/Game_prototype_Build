using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIDurkCombatManager : AICharacterCombatManager
{

    [Header("Damage Colliders")]
    [SerializeField] ManualDamageCollider rightDamageCollider;
    [SerializeField]ManualDamageCollider leftDamageCollider;

    [Header("Damage")]
    [SerializeField] float attack01DamageModifier = 1.0f;
    [SerializeField] float attack02DamageModifier = 1.4f;
    [SerializeField] float attack03DamageModifier = 1.6f;

    public void SetAttack01Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack01DamageModifier;
        leftDamageCollider.physicalDamage = baseDamage*attack01DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack01DamageModifier;
    }

    public void SetAttack02Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        leftDamageCollider.physicalDamage = baseDamage*attack02DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack02DamageModifier;
    }

    public void SetAttack03Damage()
    {
        rightDamageCollider.physicalDamage = baseDamage*attack02DamageModifier;
        leftDamageCollider.physicalDamage = baseDamage*attack02DamageModifier*attack02DamageModifier;

        rightDamageCollider.poiseDamage = basePoiseDamage*attack02DamageModifier;
    }

    public void OpenRightHandDamageCollider()
    {
        rightDamageCollider.EnableDamageCollider();
        leftDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightDamageCollider.DisableDamageCollider();
        leftDamageCollider.DisableDamageCollider();
    }

   
    public override void PivotTowardsTarget(AICharacterManager aiCharacter)
    {


        if (aiCharacter.isPerformingAction)
            return;

        if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 90", true);

        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 90", true);

        }

        if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 180", true);

        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 180", true);

        }

    }


}
