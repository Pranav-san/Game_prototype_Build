using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "A.I/States/ Attack State")]
public class AttackState : AIState 
{

    [Header("Current Attack")]    
    
    [HideInInspector] public AICharacterAttackAction currentAttack;
    [HideInInspector] public bool willPerformCombo = false;

    [Header("State Flage")]
    protected bool hasPerformedAttack = false;
    protected bool hasPerformedCombo = false;

    [Header("Pivot After Attack")]
    [SerializeField] protected bool pivotAfterAttack = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if(aiCharacter.aiCharacterCombatManager.currentTarget.characterStatsManager.isDead)
            return SwitchState(aiCharacter, aiCharacter.idle);


        //Rotate Whilest performing a  Attack
        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilestAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0,0, false);



        //Perform a combo
        if(willPerformCombo && !hasPerformedCombo)
        {
            if(currentAttack.comboAction != null)
            {
                hasPerformedCombo = true;
                currentAttack.comboAction.AttemptToPerformAction(aiCharacter);
            }
        }
        if (aiCharacter.isPerformingAction)
            return this;

        if (!hasPerformedAttack)
        {
            //If we are Recovering from an Action, Wait before performing Another
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
            {
                return this;

            }

            

            performAttack(aiCharacter);

            //Return to the Top, So if we have a Combo We process that when are able
            return this;

           

        }

        if(pivotAfterAttack)
        {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        return SwitchState(aiCharacter, aiCharacter.combatStance);




    }

    protected void performAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);

        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;

    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {
        base.ResetStateFlags(aiCharacter);

        hasPerformedAttack = false;
        hasPerformedCombo =false;
    }




}
