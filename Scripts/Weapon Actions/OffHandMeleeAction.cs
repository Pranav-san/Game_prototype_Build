using UnityEngine;

[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/off Hand Melee Action ")]
public class OffHandMeleeAction : WeaponItemBasedAction
{
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
            return;

        playerPerformingAction.isBlocking = true;


    }



}
