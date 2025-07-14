using UnityEngine;


[CreateAssetMenu(menuName ="Items/Spells/Test Spell")]
public class TestSpell : SpellItem
{

    public override void AttemptToCastSpell(playerManager player)
    {

        if (CanICastThisSpell(player))
            return;

        base.AttemptToCastSpell(player);

        if(player.isUsingRightHand)
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(mainHandSpellAnimation, true);

        }
        else
        {
            player.playerAnimatorManager.PlayTargetActionAnimation(offHandSpellAnimation, true);

        }
    }

    public override void SuccessfullyCastSpell(playerManager player)
    {
        base.SuccessfullyCastSpell(player);

        Debug.Log("Casted Spell");
    }

    public override void InstantiateWarmUpSpellFX(playerManager player)
    {
        base.InstantiateWarmUpSpellFX(player);

        Debug.Log("Instantiated FX");
    }

    public override void InstantiateReleaseFX(playerManager player)
    {
        base.InstantiateReleaseFX(player);

        Debug.Log("Instantiated Projectile");
    }


    public override bool CanICastThisSpell(playerManager player)
    {
        if (player.isPerformingAction)
            return false;
        if(player.isJumping)
            return false;
        if(player.playerStatsManager.isDead)
            return false;
        if(player.isAttacking)
            return false;



        return true;
    }


}
