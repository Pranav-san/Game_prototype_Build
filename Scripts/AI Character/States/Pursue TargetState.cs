using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/ Pursue Target")]
public class PursueTargetState : AIState
{


    public override AIState Tick(AICharacterManager aiCharacter)
    {
        //Debug.Log("Pursue Target State");

        //Check If AI is Performin An Action 
        if (aiCharacter.isPerformingAction)
        {
            return this;
        }

        //Check If AI Target Is null, If we Do not Have a Target Return To Idle State
        if (aiCharacter.aiCharacterCombatManager.currentTarget==null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        //If our Target goes outside of the characters FOV, Pivot to face them
        if(aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumDetectionFOV|| 
            aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumDetectionFOV)
        {
            aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
        }

        aiCharacter.aiCharacterLocomotionManager.RotateTowardsagent(aiCharacter);

        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget<=aiCharacter.combatStance.maximumEngagementDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.combatStance);
        }

        //Pursue Target

        //Option1
        //aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);
        //Causes problems in uneven Terrains

        //Option2
        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);
        //Performant 
        //May Cause Frame Drop

        return this;



    }

}
