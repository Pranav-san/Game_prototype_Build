using UnityEngine;

public class ResetIsChugging : StateMachineBehaviour
{
    playerManager player;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player==null)
            player = animator.GetComponent<playerManager>();

        if(player==null)
            return;

        FlaskItem currentFlaskItem = player.playerInventoryManager.currentQuickSlotItem as FlaskItem;

        if (currentFlaskItem.isHealingFlask)
        {
            if (player.playerInventoryManager.remainingHealthFlasks<=0)
            {
                player.playerAnimatorManager.PlayTargetActionAnimation(currentFlaskItem.emptyFlaskAnimation, false, false, true, true, false);
                Destroy(player.playerEffectsManager.activeQuickSlotItemFx);

            }
        }

        //Reset isChugging Bool in Animator
        animator.SetBool("isChuggingFlask", false);






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
