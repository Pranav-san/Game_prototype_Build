using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/off Hand Melee Action ")]
public class OffHandMeleeAction : WeaponItemBasedAction
{

    [SerializeField] string offHand_Attack_01 = "Light_Attack_01";
    [SerializeField] string offHand_Attack_02 = "Light_Attack_02";
    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        if(!playerPerformingAction.playerCombatManager.canBlock)
            return;

        //Check for Attack Status
        if (playerPerformingAction.isAttacking)
        {
            //Disable blocking when using certain weapon (like spear light attack)
            playerPerformingAction.isBlocking = false;
            return;
        }
        if(playerPerformingAction.isBlocking)
            return;
        
        if(playerPerformingAction.playerInventoryManager.currentLeftHandWeapon.weaponClass != WeapomClass.Shield)
        {
            PerformOffHandAttack(playerPerformingAction, weaponPerformingAction);
        }

        if (playerPerformingAction.playerInventoryManager.currentLeftHandWeapon.weaponClass == WeapomClass.Shield)
        {
            playerPerformingAction.isBlocking = true;
            playerPerformingAction.isUsingLeftHand = true;
        }

        else
        {

            playerPerformingAction.isUsingLeftHand = true;

        }




    }

    private void PerformOffHandAttack(playerManager playerPerformingAction, WeaponItem WeaponPerformingAction)
    {
        //If we are Attacking Currently, And We Can Combo,Perform The Combo Attack
        if (playerPerformingAction.playerCombatManager.canComboWithOffHandleWeapon&&playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerCombatManager.canComboWithOffHandleWeapon = false;



            if (playerPerformingAction.characterCombatManager.lastAttackAnimationPerformed==offHand_Attack_01)
            {

                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack02, offHand_Attack_02, true);


            }
            else
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack01, offHand_Attack_01, true);
            }

        }
        //Otherwise if we are not attacking, Just Perform Regular Attack
        else if (!playerPerformingAction.isPerformingAction)
        {
            playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(WeaponPerformingAction, AttackType.LightAttack01, offHand_Attack_01, true);

        }

    }



}
