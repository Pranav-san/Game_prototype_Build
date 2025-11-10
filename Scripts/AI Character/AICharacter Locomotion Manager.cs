using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class AICharacterLocomotionManager : CharacterLocomotionManager
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    AICharacterManager aiCharacter;
    public void RotateTowardsagent(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isMoving)
        {
            // Calculate the direction to the target
            Vector3 direction = aiCharacter.navMeshAgent.desiredVelocity.normalized;

            // If the direction is valid, rotate smoothly
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void MoveCharacter(AICharacterManager aiCharacter)
    {
        if (aiCharacter.navMeshAgent.isOnNavMesh)
        {
            // Move the character towards the desired velocity
            //aiCharacter.transform.position += aiCharacter.navMeshAgent.desiredVelocity * Time.deltaTime;

            Vector3 desiredVelocity = aiCharacter.navMeshAgent.desiredVelocity;


            // Move via CharacterController to keep collision active
            aiCharacter.characterController.Move(desiredVelocity * Time.deltaTime);

            // Rotate the character based on desired velocity
            if (desiredVelocity.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(desiredVelocity.normalized);
                aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }


    }

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();
    }


    protected override void Update()
    {
        base.Update();
        
    }

}
