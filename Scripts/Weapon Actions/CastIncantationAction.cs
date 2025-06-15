using UnityEngine;



[CreateAssetMenu(menuName = "CharacterActions/WeaponActions/Incantation Action")]
public class CastIncantationAction : WeaponItemBasedAction
{

    public override void AttemptToPerformAction(playerManager playerPerformingAction, WeaponItem weaponPerformingAction)
    {
        base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

        //Check for Stops
        if (!playerPerformingAction.isGrounded)
            return;

        playerPerformingAction.isAttacking = true;

        if (playerPerformingAction.playerInventoryManager.currentSpell== null)
            return;

        if (playerPerformingAction.playerInventoryManager.currentSpell.spellClass != SpellClass.Incantation)
            return;


        CastIncantation(playerPerformingAction, weaponPerformingAction);
    }

    private void CastIncantation(playerManager playerPerformingAction, WeaponItem WeaponPerformingAction)
    {

        playerPerformingAction.playerInventoryManager.currentSpell.AttemptToCastSpell(playerPerformingAction);
      

    }

}
