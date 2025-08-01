using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetActionFlag : StateMachineBehaviour
{
    CharacterManager character;
    playerManager player;
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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


        if (player != null)
        {
            player.isUsingRightHand = false;
            player.isUsingLeftHand = false;

        }
           

        
        

        //This is called when an Action Ends, And the state returns to "Empty"



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
