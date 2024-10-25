using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/UnArmed Light Attack ")]
public class UnArmerAttacK : WeaponItemBasedAction
{

    [SerializeField] string light_Attack_01 = "Punch_01";
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        //Check for Stops
        if (!playerPerformingAction.isGrounded)
            return;


        PerformLightAttack(playerPerformingAction, weaponPerformingAction);
    }

    private void PerformLightAttack(playerManager playerPerformingAction, WeaponItem WeaponPerformingAction)
    {

        if (playerPerformingAction.isUsingRightHand)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_Attack_01, true);
        }

    }

}
