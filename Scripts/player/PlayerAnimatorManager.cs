using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : CharacterAnimatorManager
{
    playerManager player;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<playerManager>();
    }
    private void OnAnimatorMove()
    {
        if (player.applyRootMotion)
        {
            Vector3 velocity = player.animator.deltaPosition;
            player.characterController.Move(velocity);
            player.transform.rotation *= player.animator.deltaRotation;
        }
    }

    public override void EnableCanDoCombo()
    {
        if (player.isUsingRightHand)
        {
            player.playerCombatManager.canComboWithMainHandleWeapon = true;
        }
        else
        {
            //Enable Off Hand Weapon
        }
    }

    public override void DisableCanDoCombo()
    {
        player.playerCombatManager.canComboWithMainHandleWeapon = false;
        //player.playerCombatManager.canComboWithOffHandleWeapon = false;


    }
}
