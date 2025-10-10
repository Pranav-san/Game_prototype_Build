using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    [SerializeField]CharacterManager character;
    playerManager player;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //This is called when an Action Ends, And the state returns to "Empty"

        if (character == null)
        {
            character= animator.GetComponent<CharacterManager>();
            
        }
        if (player == null)
        {
            player = animator.GetComponent<playerManager>();

        }
        character.isPerformingAction = false;
        character.canRotate = true;
        character.canMove = true;
        character.applyRootMotion = false;
        character.characterAnimatorManager.DisableCanDoCombo();
        character.characterLocomotionManager.isRolling = false;
        character.isAttacking = false;
        character.isInvulnerable = false;
        character.isJumping = false;
        character.canRoll = true;
        character.characterCombatManager.isRipostable = false;
        character.isPerformingCriticalAttack = false;
        character.characterLocomotionManager.isExitingLadder = false;

       


        character.canRun =true;

        if(character.characterEffectsManager.activeQuickSlotItemFx != null)
            Destroy(character.characterEffectsManager.activeQuickSlotItemFx);


        if (player != null)
        {
            player.isUsingRightHand = false;
            player.isUsingLeftHand = false;
            player.isAiming = false;
            player.playerCombatManager.isAimLockedOn = false;
            player.isReloadedWeapon = false;
            player.animator.SetBool("isAiming",player.isAiming);
            if (player.playerInventoryManager.currentTwoHandWeapon!=null && player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun || player.playerInventoryManager.currentRightHandWeapon.weaponClass == WeapomClass.Gun)
            {
                PlayerCamera.instance.OnIsAimingChanged(false);
            }
            
            player.isGrappled = false;
            player.playerLocomotionManager.hasPlayedSprintStart = false;
            player.playerLocomotionManager.isExitingLadder = false;
            player.playerLocomotionManager.isClimbingLadder = false;
            player.animator.SetBool("isClimbingLadder", false);



        }
           

        
        

        



    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
