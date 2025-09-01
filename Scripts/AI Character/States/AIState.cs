using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState : ScriptableObject
{

    

    public virtual AIState Tick(AICharacterManager aiCharacter)
    {
        return this;

    }

    public virtual AIState SwitchState (AICharacterManager aiCharacter, AIState newState)
    {
        ResetStateFlags(aiCharacter);
        return newState;

    }

    protected virtual void ResetStateFlags(AICharacterManager aiCharacter)
    {
        // Reset Any State Flags Here So When you Return to the State, They Are Blank Once Again
        aiCharacter.applyRootMotion = false;
        aiCharacter.isPerformingAction = false;
        aiCharacter.isMoving = false;
        aiCharacter.characterController.Move(Vector3.zero);

    }
    
      
   
}
