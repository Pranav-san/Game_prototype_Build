using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;


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
    [SerializeField] protected bool PivotAfterAttackFourAngles = false;

    public override AIState Tick(AICharacterManager aiCharacter)
    {

        aiCharacter.isBlocking = false;

        if(aiCharacter.enemyType == EnemyType.Ranged)
        {
            return ProcessArcheryCombatStyle(aiCharacter);
        }




        aiCharacter.characterController.Move(Vector3.zero);

        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if(aiCharacter.aiCharacterCombatManager.currentTarget.characterStatsManager.isDead)
            return SwitchState(aiCharacter, aiCharacter.idle);


        //Rotate Whilest performing a  Attack
        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilestAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0,0, false,false);



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
                aiCharacter.isBlocking = true;
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
        else if (PivotAfterAttackFourAngles)
        {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTargetFourRotations(aiCharacter);
        }

        aiCharacter.navMeshAgent.enabled = true;
        return SwitchState(aiCharacter, aiCharacter.combatStance);




    }

    private AIState ProcessArcheryCombatStyle(AICharacterManager aiCharacter)
    {
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (aiCharacter.aiCharacterCombatManager.currentTarget.characterStatsManager.isDead)
            return SwitchState(aiCharacter, aiCharacter.idle);

        if (aiCharacter.enemyType == EnemyType.exploder && aiCharacter.hasExploded)
        {
            return this; //stay in Attack Sate After Explosion
        }


        if (aiCharacter.enemyType==EnemyType.Ranged&&!aiCharacter.isHoldingArrow)
        {
            ResetStateFlags(aiCharacter);
            return SwitchState(aiCharacter, aiCharacter.combatStance);
           

        }

        aiCharacter.aiCharacterCombatManager.RotateTowardsTargetWhilestAttacking(aiCharacter);

        aiCharacter.characterAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false, false);


        if (!hasPerformedAttack)
        {
            //If we are Recovering from an Action, Wait before performing Another
            if (aiCharacter.aiCharacterCombatManager.actionRecoveryTimer > 0)
                return this;

            



            //Fire Ammo
            FireAmmo(aiCharacter);
            hasPerformedAttack = true;
            return this;



        }
        if (aiCharacter.isPerformingAction)
            return this;
        if(aiCharacter.enemyType == EnemyType.Ranged)
        {
            aiCharacter.combatStance.ResetAmmoAfterFiring();

        }
        

        if (pivotAfterAttack)
        {
            aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);
        }

        if(aiCharacter.enemyType != EnemyType.exploder)
        {
            aiCharacter.navMeshAgent.enabled = true;
            return SwitchState(aiCharacter, aiCharacter.combatStance);

        }

        return this;
        

    }

    protected void performAttack(AICharacterManager aiCharacter)
    {
        hasPerformedAttack = true;
        currentAttack.AttemptToPerformAction(aiCharacter);

        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = currentAttack.actionRecoveryTime;

    }
    private void FireAmmo(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isHoldingArrow)
        {
            aiCharacter.isHoldingArrow = false;
            aiCharacter.isPerformingAction = true;
            aiCharacter.animator.SetBool("isHoldingArrow", false);
            ResetStateFlags(aiCharacter);



        }
    }

    public void ResetStateMachine(AICharacterManager aiCharacter)
    {
        ResetStateFlags(aiCharacter);

    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {

        hasPerformedAttack = false;
        hasPerformedCombo =false;
        aiCharacter.isPerformingAction=false;
        aiCharacter.combatStance.ResetStatemachine(aiCharacter);

    }


  




}
