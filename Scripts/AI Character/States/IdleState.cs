using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="A.I/States/Idle")]
public class IdleState : AIState
{

    public override AIState Tick(AICharacterManager aiCharacter)
    {

        if (aiCharacter.characterCombatManager.currentTarget!=null)
        {
            //Return Pursue Target State
            Debug.Log("AI Has a Target");

            return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            
        }
        else
        {
            //Return This State, To Continually Search For a Target
            //Debug.Log("Searching for Target");
            aiCharacter.aiCharacterCombatManager.FindTargetViaLineOfSight(aiCharacter);
            
            return this;

        }

        
    }


}
