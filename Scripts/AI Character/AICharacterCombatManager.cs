using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterCombatManager : CharacterCombatManager
{

    protected AICharacterManager aiCharacter;

    [Header("Damage")]
     protected int baseDamage = 25;
     protected int basePoiseDamage = 25;
    

    [Header("Target Onformation")]
    public float distanceFromTarget;
    public float viewableAngle;
    public Vector3 targetDirection;




    [Header("Detection")]
    [SerializeField] float detectionRadius = 15f;
    public float minimumDetectionFOV = -35f;
    public  float maximumDetectionFOV = 35f;

    [Header("Action Recovery")]
    public float actionRecoveryTimer = 0;

    [Header("Attack Rotation Speed")]
    public float attackRotationSpeed = 25;

    protected override void Awake()
    {
        base.Awake();

        aiCharacter = GetComponent<AICharacterManager>();

        
    }

    public void AwardRunesOnDeath(playerManager player)
    {
        if (player.characterGroup == CharacterGroup.Team02)
            return;

        player.playerStatsManager.AddRunes(aiCharacter.characterStatsManager.runesDroppedOnDeath);

    }

    public void FindTargetViaLineOfSight(AICharacterManager aiCharacter)
    {
        if(currentTarget!=null)
            return;

        Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.Instance.GetCharacterLayer());

        for(int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

            if(targetCharacter == null)
                continue;
            if(targetCharacter == aiCharacter) 
                continue;

            if(targetCharacter.characterStatsManager.isDead) 
                continue;

            if(WorldUtilityManager.Instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
            {
                //IF a potential Target is found it has to be infront of us
                Vector3 targetdirection = targetCharacter.transform.position - aiCharacter.transform.position;
                float angleOfPotentialTarget = Vector3.Angle(targetdirection, aiCharacter.transform.forward);

                if(angleOfPotentialTarget > minimumDetectionFOV && angleOfPotentialTarget < maximumDetectionFOV)
                {
                    //Check for Enviro Blocks
                    if(Physics.Linecast(aiCharacter.characterCombatManager.LockOnTransform.position, 
                        targetCharacter.characterCombatManager.LockOnTransform.position, 
                        WorldUtilityManager.Instance.GetEnviroLayer()))
                    {
                        Debug.DrawLine(aiCharacter.characterCombatManager.LockOnTransform.position, targetCharacter.characterCombatManager.LockOnTransform.position, Color.red, 2f );
                        Debug.Log("Blocked");
                    }
                    else
                    {
                        targetDirection=targetCharacter.transform.position - transform.position;
                        viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, targetDirection);
                        aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                        PivotTowardsTarget(aiCharacter);
                    }
                }
            }
            

        }


    }

    //Rotation Animation 
    public void PivotTowardsTarget(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;
        if (viewableAngle>=20 && viewableAngle<=60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 45 ", true);
            Debug.Log("45 degree Right");
        }
        else if (viewableAngle <= -20 && viewableAngle >= -60)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 45 ", true);
            Debug.Log("45 degree left");

        }
        if (viewableAngle >= 61 && viewableAngle <= 110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 90", true);

        }
        else if (viewableAngle <= -61 && viewableAngle >= -110)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 90", true);

        }

        if (viewableAngle >= 146 && viewableAngle <= 180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Right 180", true);

        }
        else if (viewableAngle <= -146 && viewableAngle >= -180)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Turn Left 180", true);

        }


    }

    public void HandleActionRecovery(AICharacterManager aiCharacter)
    {
        if (actionRecoveryTimer>0)
        {
            if(!aiCharacter.isPerformingAction)
            {
                actionRecoveryTimer -= Time.deltaTime;  
            }

        }
    }

    public void RotateTowardsAgent(AICharacterManager aiCharacter)
    {
        //Sebbs Logic
        if(aiCharacter.isMoving)
        {
        aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
        }



       
    }

    public void RotateTowardsTargetWhilestAttacking(AICharacterManager aiCharacter)
    {
        if (currentTarget==null)
            return;

        if(!aiCharacter.canRotate)
            return;

        

        

        Vector3 targetDirection = currentTarget.transform.position - aiCharacter.transform.position;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection==Vector3.zero)
        {
            targetDirection = aiCharacter.transform.forward;  
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        aiCharacter.transform.rotation = Quaternion.Slerp(aiCharacter.transform.rotation, targetRotation, attackRotationSpeed * Time.deltaTime);

        Debug.Log("Rotate While Attacking");


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

    }

}
