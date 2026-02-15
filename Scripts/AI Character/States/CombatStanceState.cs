using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/ Combat Stance")]

public class CombatStanceState : AIState
{
    [Header("Enemy Type")]
    EnemyType enemyType;

    [Header("AI Character Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;// A List of All Possible Attacks This Character can do

    protected List<AICharacterAttackAction> potentialAttacks;// All Attacks Possible in This situation (based on, Angle, Distance, etc)
    private AICharacterAttackAction choosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;

    [Header("Delay Timers")]
    private float attackDecisionTimer = 0f;
    private float attackDecisionInterval = 0.2f;

    private float navMeshUpdateTimer = 0f;
    private float navMeshUpdateInterval = 0.5f;

    [Header("Ranged Attack Setting")]
    [SerializeField] bool hasAmmoLoaded = false;
    [SerializeField] float minimumRangedDistanceengageMentDistance = 4;
    [SerializeField] float MaximumRangedDistanceengageMentDistance = 9;




    [Header("combos")]
    [SerializeField] protected bool canPerformCombo = false;
    [SerializeField] protected int chanceToPerformCombo = 25;
    [SerializeField] protected bool hasRolledForComboChance = false;

    [Header("Enable Pivot")]
    [SerializeField] protected bool enablePivot;

    [Header("Engagement Distance")]
    [SerializeField] public float maximumEngagementDistance = 5f; //The distance we have to be away from the target to enter Pursue State

    [Header("Circling")]
    [SerializeField] bool willCircleTarget = false;
    private bool hasChoosenCirclePath = false;
    [SerializeField] float strafeMoveAmount = 0;

    [Header("Blocking")]
    public bool canBlock = false;
    [SerializeField] int percentageOfTimeWillBlock =  75;
    [SerializeField] bool hasRolledForBlockChance = false;
    [SerializeField] bool willBlockDuringThisCombatRotation = false;



    [Header("Evasion")]
    [SerializeField] bool canEvade = false;
    [SerializeField] int percentageofTimeWillEvade = 75;
    private bool hasEvaded = false;
    private bool hasRolledorEvasionChance = false;
    private bool willEvadeDuringCombatRotation = false;






    public override AIState Tick(AICharacterManager aiCharacter)
    {

        if (aiCharacter.enemyType == EnemyType.Ranged)
        {
            return ProcessArcheryCombatStyle(aiCharacter);
        }

        if(aiCharacter.enemyType == EnemyType.exploder)
        {
            return ProcessExploderCombatStyle(aiCharacter);
        }

        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;


        if (enablePivot)
        {
            if (!aiCharacter.isMoving)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle<-30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

        }




        //If you want your Ai character to turn and Face towards its target when its outside of its FOV


        //Rotate to Face our target
        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        //If Target is No Longer Present, Switch to Idle State
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (willCircleTarget)
            SetCirclePath(aiCharacter);

        if (!willBlockDuringThisCombatRotation)
        {
            aiCharacter.isBlocking = false;
        }

        if (canBlock && !hasRolledForBlockChance)
        {
            hasRolledForBlockChance = true;
            willBlockDuringThisCombatRotation = RollForOutComeChance(percentageOfTimeWillBlock);
        }

        if (willBlockDuringThisCombatRotation)
        {
            aiCharacter.isBlocking = true;
        }
        else
        {
            aiCharacter.isBlocking = false;
            
        }


        if (canEvade && !hasRolledorEvasionChance)
        {
            hasRolledorEvasionChance = true;
            willEvadeDuringCombatRotation = RollForOutComeChance(percentageofTimeWillEvade);

        }

        if (willEvadeDuringCombatRotation && aiCharacter.aiCharacterCombatManager.currentTarget.isAttacking && !hasEvaded)
        {
            hasEvaded = true;
            aiCharacter.aiCharacterCombatManager.PerformEvasion();

        }
        //If we dont have an Attack get one
        //attackDecisionTimer += Time.deltaTime;

        //if (!hasAttack && attackDecisionTimer >= attackDecisionInterval)

        if (!hasAttack)
        {
            GetNewAttack(aiCharacter);
            //If we are outside of its Combat EngagementDistance, switch to Pursue Target State
            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > aiCharacter.combatStance.maximumEngagementDistance)
            {
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            }

            //Performant 
            //May Cause Frame Drop

            if (!IsDestinationReachable(aiCharacter, aiCharacter.characterCombatManager.currentTarget.transform.position))
            {
                if (!aiCharacter.isMoving && (aiCharacter.aiCharacterCombatManager.viewableAngle < -30f || aiCharacter.aiCharacterCombatManager.viewableAngle > 30f))
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
        aiCharacter.navMeshAgent.enabled = false;
        aiCharacter.attackState.currentAttack = choosenAttack;
        if (canPerformCombo)
        {

        }
        return SwitchState(aiCharacter, aiCharacter.attackState);



        


    }


    private AIState ProcessArcheryCombatStyle(AICharacterManager aiCharacter)
    {
        aiCharacter.characterController.Move(Vector3.zero);

        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        if (enablePivot)
        {
            if (!aiCharacter.isMoving)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle<-30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

        }




        //If you want your Ai character to turn and Face towards its target when its outside of its FOV


        //Rotate to Face our target
        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        //If Target is No Longer Present, Switch to Idle State
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (willCircleTarget)
            SetCirclePath(aiCharacter);

        //If we dont have an Attack get one


        if (!hasAmmoLoaded)
        {
            //Draw Arrow Projectile
            DrawArrow(aiCharacter);
            //Aim At Target before We Fire
            AimAtTargetBeforeFiring(aiCharacter);
            hasAmmoLoaded = true;

        }

        if (hasAmmoLoaded)
        {
            if (aiCharacter.aiCharacterCombatManager.currentTarget.isPerformingAction)
                return this;




            aiCharacter.navMeshAgent.enabled = false;

            return SwitchState(aiCharacter, aiCharacter.attackState);


        }



        //If we are outside of its Combat EngagementDistance, switch to Pursue Target State
        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }

        //navMeshUpdateTimer += Time.deltaTime;

        //if (navMeshUpdateTimer >= navMeshUpdateInterval)
        {
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            navMeshUpdateTimer = 0f;
        }
        //Performant 
        //May Cause Frame Drop

        return this;

    }

    private AIState ProcessExploderCombatStyle(AICharacterManager aiCharacter)
    {
        aiCharacter.characterController.Move(Vector3.zero);

        if (aiCharacter.isPerformingAction)
            return this;

        if (!aiCharacter.navMeshAgent.enabled)
            aiCharacter.navMeshAgent.enabled = true;

        if(aiCharacter.aiCharacterCombatManager.currentTarget.characterStatsManager.isDead)
            aiCharacter.aiCharacterCombatManager.SetTarget(null);

        if (enablePivot)
        {
            if (!aiCharacter.isMoving)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle<-30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                {
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
                }
            }

        }




        //If you want your Ai character to turn and Face towards its target when its outside of its FOV


        //Rotate to Face our target
        aiCharacter.aiCharacterCombatManager.RotateTowardsAgent(aiCharacter);

        //If Target is No Longer Present, Switch to Idle State
        if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            return SwitchState(aiCharacter, aiCharacter.idle);
        }

        if (willCircleTarget)
            SetCirclePath(aiCharacter);


        if (canEvade && !hasRolledorEvasionChance)
        {
            hasRolledorEvasionChance = true;
            willEvadeDuringCombatRotation = RollForOutComeChance(percentageofTimeWillEvade);

        }

        if (willEvadeDuringCombatRotation && aiCharacter.aiCharacterCombatManager.currentTarget.isAttacking && !hasEvaded)
        {
            hasEvaded = true;
            aiCharacter.aiCharacterCombatManager.PerformEvasion();

        }
        //If we dont have an Attack get one
        //attackDecisionTimer += Time.deltaTime;

        //if (!hasAttack && attackDecisionTimer >= attackDecisionInterval)

        if (hasAttack)
        {

            aiCharacter.navMeshAgent.enabled = false;
            aiCharacter.attackState.currentAttack = choosenAttack;

            return SwitchState(aiCharacter, aiCharacter.attackState);


        }
        else
        {
            GetNewAttack(aiCharacter);
            attackDecisionTimer = 0f;

        }

        //If we are outside of its Combat EngagementDistance, switch to Pursue Target State
        if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);
        }

        //navMeshUpdateTimer += Time.deltaTime;

        //if (navMeshUpdateTimer >= navMeshUpdateInterval)
        {
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            navMeshUpdateTimer = 0f;
        }
        //Performant 
        //May Cause Frame Drop

        return this;
    }



    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {

        potentialAttacks = new List<AICharacterAttackAction>();

        foreach (var potentialAttack in aiCharacterAttacks)
        {
            //If we are Too close for this Attack, Check the next one
            if (potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            //If we are Too far for this Attack, Check the next one
            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            //If the Target is outside of the MINIMUM FOV for this Attack, Check the Next Attack
            if (potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            //If the Target is outside of the MAXIMUM FOV for this Attack, Check the Next Attack
            if (potentialAttack.maximumAttackAngle < aiCharacter.aiCharacterCombatManager.viewableAngle)
                continue;

            potentialAttacks.Add(potentialAttack);
        }
        if (potentialAttacks.Count<=0)
        {
            return;
        }

        var totalWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            totalWeight += attack.attackWeight;

        }
        var randomWeightValue = Random.Range(1, totalWeight+1);
        var processedWeight = 0;

        foreach (var attack in potentialAttacks)
        {
            processedWeight += attack.attackWeight;

            if (randomWeightValue<=processedWeight)
            {
                //This is our Attack
                choosenAttack = attack;
                previousAttack = choosenAttack;
                hasAttack = true;
                return;
            }
        }



    }

    protected virtual bool RollForOutComeChance(int outComeChance)
    {
        bool outComeWillBePerformed = false;

        int randomPercentage = Random.Range(0, 100);

        if (randomPercentage < outComeChance)
        {
            outComeWillBePerformed = true;
        }
        return outComeWillBePerformed;


    }

    protected override void ResetStateFlags(AICharacterManager aiCharacter)
    {

        hasAttack = false;
        hasRolledForComboChance = false;
        hasRolledorEvasionChance = false;
        hasRolledForBlockChance = false;
        willBlockDuringThisCombatRotation = false;
        hasEvaded= false;
        strafeMoveAmount = 0;
        hasChoosenCirclePath = false;
        hasAmmoLoaded = false;

        attackDecisionTimer = 0f;
        navMeshUpdateTimer = 0f;

    }

    protected virtual void SetCirclePath(AICharacterManager aiCharacter)
    {

        if (Physics.CheckSphere(aiCharacter.aiCharacterCombatManager.LockOnTransform.position, aiCharacter.characterController.radius+ 0.25f, WorldUtilityManager.Instance.GetEnviroLayer()))
        {
            //Stop Strafing/Circling Because We've Hit Something, Instead path Towards Enemy
            //This Will Make Our Character Follow Navmesh Agent And Path Toward Our Target
            Debug.Log("AI Colliding with something, Ended Strafe");
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, Mathf.Abs(strafeMoveAmount));

            return;
        }

        //Strafe
        //Debug.Log("AI STRAFING");
        aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(strafeMoveAmount, 0);

        if (hasChoosenCirclePath)
        {
            return;
        }



        //Strafe Left or Right
        hasChoosenCirclePath = true;

        int leftOrRightIndex = Random.Range(0, 100);

        if (leftOrRightIndex >= 50)
        {
            //Left
            strafeMoveAmount = -0.5f;
            //Debug.Log("Chosen strafe direction: " + strafeMoveAmount);
        }

        else
        {
            //Right
            strafeMoveAmount = 0.5f;
            //Debug.Log("Chosen strafe direction: " + strafeMoveAmount);
        }


    }


    private void DrawArrow(AICharacterManager aiCharacter)
    {
        if (aiCharacter.isPerformingAction)
            return;

        hasAmmoLoaded = true;
        aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Bow_Draw", true, true, true);
        aiCharacter.isPerformingAction = true;
        aiCharacter.animator.SetBool("isHoldingArrow", true);
        aiCharacter.isHoldingArrow = true;

    }

    public void ResetAmmoAfterFiring()
    {
        hasAmmoLoaded = false;
        hasAttack = false;

    }

    private void AimAtTargetBeforeFiring(AICharacterManager aiCharacter)
    {
        float timeUntilAmmoIsShotAtTarget = Random.Range(aiCharacter.minimumTimeToAimAtTarget, aiCharacter.maximunTimeToAimAtTarget);
        aiCharacter.aiCharacterCombatManager.actionRecoveryTimer = timeUntilAmmoIsShotAtTarget;




    }

    public void ResetStatemachine(AICharacterManager aiCharacter)
    {
        ResetStateFlags(aiCharacter);






    }

}
