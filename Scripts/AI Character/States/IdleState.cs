using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName ="A.I/States/Idle")]
public class IdleState : AIState
{

    [Header("Idle Options")]
    [SerializeField]IdleStateMode idleStateMode;

    [Header("Patrol Options")]
    public AIPatrolPath aiPatrolPath;
    [SerializeField] bool hasFoundClosestPointNearCharacterSpawn = false;  // IF THE CHARACTER SPAWNS CLOSER TO THE SECOND POINT, START AT THE SECOND POINT
    [SerializeField] bool patrolComplete = false;   // HAVE WE FINISHED THE ENTIRE PATROL YET
    [SerializeField] bool repeatPatrol = false;   // UPON FINISHING, DO WE REPEAT THE PATH AGAIN
    [SerializeField] int patrolDestinationIndex;   // WHICH POINT OF THE PATROL ARE WE CURRENTLY WORKING TOWARDS
    [SerializeField] bool hasPatrolDestination = false;   // DO WE HAVE A POINT WE ARE CURRENTLY WORKING TOWARDS
    [SerializeField] Vector3 currentPatrolDestination; // THE SPECIFIC DESTINATION COORDS WE ARE HEADING TOWARDS
    [SerializeField] float distanceFromCurrentDestination;   // THE DISTANCE FROM THE A.I CHARACTER TO THE DESTINATION
    [SerializeField] float timeBetweenPatrols = 15;  // MINIMUM TIME BEFORE STARTING A NEW PATROL
    [SerializeField] float restTimer = 0;   // ACTIVE TIMER COUNTING THE TIME RESTED

    public override AIState Tick(AICharacterManager aiCharacter)
    {
        
        aiCharacter.aiCharacterCombatManager.FindTargetViaLineOfSight(aiCharacter);

        switch (idleStateMode)
        {
            case IdleStateMode.Idle: return Idle(aiCharacter);
                

            case IdleStateMode.Patrol: return Patrol(aiCharacter);

            default:
                break;
                

        }

        return this;


        
    }

    protected virtual AIState Idle(AICharacterManager aiCharacter)
    {

        if (aiCharacter.characterCombatManager.currentTarget!=null)
        {
            //Return Pursue Target State
            //Debug.Log("AI Has a Target");

            return SwitchState(aiCharacter, aiCharacter.pursueTarget);


        }
        else
        {
            //Return This State, To Continually Search For a Target
            //Debug.Log("Searching for Target");


            return this;

        }

    }

    protected virtual AIState Patrol(AICharacterManager aiCharacter)
    {

        if (!aiCharacter.isGrounded)
            return this;

        if (aiCharacter.isPerformingAction)
        {
            aiCharacter.navMeshAgent.enabled = false;
            aiCharacter.isMoving = false;

            return this;
        }

        if (!aiCharacter.navMeshAgent.enabled)
        aiCharacter.navMeshAgent.enabled = true;

        if(aiCharacter.aiCharacterCombatManager.currentTarget != null)
        return SwitchState(aiCharacter, aiCharacter.pursueTarget);


        //If Our Patrol Is Complete, We Will Repeat it and Check For Rest Time
        if(patrolComplete&& repeatPatrol)
        {

            //If the Time Has not Exceeded its set limit,Stop and wait
            if (timeBetweenPatrols>restTimer)
            {
                aiCharacter.navMeshAgent.enabled=false;
                aiCharacter.isMoving=false;
                restTimer +=Time.deltaTime;
            }

            else
            {
                patrolDestinationIndex =-1;
                hasPatrolDestination = false;
                currentPatrolDestination = aiCharacter.transform.position;
                patrolComplete = false;
                restTimer = 0;

            }
        }
        else if (patrolComplete && !repeatPatrol)
        {
            aiCharacter.navMeshAgent.enabled=false ;
            aiCharacter.isMoving=false;

        }

        //If We have a Destination Move toward it
        if(hasPatrolDestination)
        {
            distanceFromCurrentDestination = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination);

            if(distanceFromCurrentDestination > 2)
            {
                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.aiCharacterLocomotionManager.RotateTowardsagent(aiCharacter);
            }
            else
            {
                currentPatrolDestination = aiCharacter.transform.position;
                hasPatrolDestination=false;
            }
        }

        //Otherwise Get a new Destination
        else
        {
            patrolDestinationIndex += 1;

            if(patrolDestinationIndex > aiPatrolPath.patrolPoints.Count-1)
            {
                patrolComplete=true;
                return this;
            }

            if (!hasFoundClosestPointNearCharacterSpawn)
            {
                hasFoundClosestPointNearCharacterSpawn = true;
                float closestDistance = Mathf.Infinity;

                for (int i = 0; i < aiPatrolPath.patrolPoints.Count; i++)
                {
                    float distanceFromThisPoint = Vector3.Distance(aiCharacter.transform.position, aiPatrolPath.patrolPoints[i]);
                    if (distanceFromThisPoint < closestDistance)
                    {
                        closestDistance = distanceFromThisPoint;
                        patrolDestinationIndex = i;
                        currentPatrolDestination = aiPatrolPath.patrolPoints[i];

                    }
                }
            }
            else
            {
                currentPatrolDestination = aiPatrolPath.patrolPoints[patrolDestinationIndex];
            }
            hasPatrolDestination = true;


        }

        NavMeshPath path = new NavMeshPath();   
        aiCharacter.navMeshAgent.CalculatePath(currentPatrolDestination, path);
        aiCharacter.navMeshAgent.SetPath(path);

        return this;
        

    }


}
