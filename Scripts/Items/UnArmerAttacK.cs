using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/UnArmed Light Attack ")]
public class UnArmerAttacK : WeaponItemBasedAction
{

    [SerializeField] string light_Attack_01 = "Punch_01";
    [SerializeField] string light_Attack_02 = "Punch_02";
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        //Check for Stops
        if (!playerPerformingAction.isGrounded)
            return;

        playerPerformingAction.isAttacking = true;
        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(playerManager playerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        //If we are Attacking Currently, And We Can Combo,Perform The Combo Attack
        if (playerPerformingAction.playerCombatManager.canComboWithMainHandleWeapon&&playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithMainHandleWeapon = false;

            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed==light_Attack_01)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack02, light_Attack_02, true);

            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);
            }

        }
        //Otherwise if we are not attacking, Just Perform Regular Attack
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack01, light_Attack_01, true);

        }

    }

}
