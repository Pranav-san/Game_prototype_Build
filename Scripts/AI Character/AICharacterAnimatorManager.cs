using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterAnimatorManager : CharacterAnimatorManager
{
    public  AICharacterManager aiCharacter;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }

    private void OnAnimatorMove()
    {
        if(!aiCharacter.isGrounded)
            return;

        Vector3 velocity = aiCharacter.animator.deltaPosition;
        aiCharacter.characterController.Move(velocity);
        //aiCharacter.transform.rotation = aiCharacter.animator.deltaRotation;
    }

}
