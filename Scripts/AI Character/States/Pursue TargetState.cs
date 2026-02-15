using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/ Pursue Target")]
public class PursueTargetState : AIState
{
    [SerializeField] protected bool enablePivot = false;
    [SerializeField] protected bool willBlockWhenPursuingTarget = false;
    [SerializeField] float endPursueTargetDistance = 2f;


    public override AIState Tick(AICharacterManager aiCharacter)
    {
        //Debug.Log("Pursue Target State");

        if (willBlockWhenPursuingTarget)
        {
            aiCharacter.isBlocking = true;
        }
        //Check If AI is Performing An Action 
        if (aiCharacter.isPerformingAction || !aiCharacter.isGrounded)
        {
            //aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 0);
            return this;

        }


        //Make Sure The Navmesh Agent Is Enabled, If Its Not Enable It
        if (!aiCharacter.navMeshAgent.enabled)
        {
            aiCharacter.navMeshAgent.enabled = true;

        }
            



        float distanceFromTarget = aiCharacter.aiCharacterCombatManager.distanceFromTarget;

        if (distanceFromTarget <= 2)
        {
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 0.5f);

        }

        else if(distanceFromTarget >2 &&  distanceFromTarget<=5)
        {
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 1f);

        }
        else if(distanceFromTarget > 5 && distanceFromTarget <=100f)
        {
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 2f);
        }

        else
        {
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, 1);
        }

        //If our Target goes outside of the characters FOV, Pivot to face them
        aiCharacter.aiCharacterLocomotionManager.RotateTowardsagent(aiCharacter);

        //Check If AI Target Is null, If we Do Not Have A Target Return To Idle State
        if (aiCharacter.aiCharacterCombatManager.currentTarget==null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);

        }
            

        //Option_1
        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget < endPursueTargetDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.combatStance);

        }
            

        //Option_2
        //if (aiCharacter.aiCharacterCombatManager.distanceFromTarget <= aiCharacter.navMeshAgent.stoppingDistance)
        //{
        //    return SwitchState(aiCharacter, aiCharacter.combatStance);
        //}

        //Pursue Target

        //Option1
        //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);
        //Causes problems in uneven Terrains

        //Option2
        //Performant 
        //May Cause Frame Drop

        if (!IsDestinationReachable(aiCharacter, aiCharacter.characterCombatManager.currentTarget.transform.position))
        {
            if (!aiCharacter.isMoving && (aiCharacter. aiCharacterCombatManager.viewableAngle < -30f || aiCharacter.aiCharacterCombatManager.viewableAngle > 30f))
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
            if (NavMesh.SamplePosition(aiCharacter.characterCombatManager.currentTarget.transform.position, out var hit, 2f, -1))
            {
                NavMeshPath path = new NavMeshPath();
                aiCharacter.navMeshAgent.CalculatePath(hit.position, path);
                aiCharacter.navMeshAgent.SetPath(path);
            }
            return this;
        }


        NavMeshPath path2 = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path2);
        aiCharacter.navMeshAgent.SetPath(path2);

        return this;



    }


}
