using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "A.I/States/ Combat Stance")]

public class CombatStanceState : AIState
{
    [Header("AI Character Attacks")]
    public List<AICharacterAttackAction> aiCharacterAttacks;// A List of All Possible Attacks This Character can do

    protected List<AICharacterAttackAction> potentialAttacks;// All Attacks Possible in This situation (based on, Angle, Distance, etc)
    private AICharacterAttackAction choosenAttack;
    private AICharacterAttackAction previousAttack;
    protected bool hasAttack = false;


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




    public override AIState Tick(AICharacterManager aiCharacter)
    {
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
        if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
        {
            SwitchState(aiCharacter, aiCharacter.idle);
        }

        if(willCircleTarget)
            SetCirclePath(aiCharacter);

        //If we dont have an Attack get one
        if(!hasAttack)
        {
            GetNewAttack(aiCharacter);

        }
        else
        {

            aiCharacter.attackState.currentAttack = choosenAttack;

            return SwitchState(aiCharacter, aiCharacter.attackState);


        }

        //If we are outside of its Combat EngagementDistance, switch to Pursue Target State
        if(aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
        {
            return SwitchState(aiCharacter, aiCharacter.pursueTarget);  
        }

        NavMeshPath path = new NavMeshPath();
        aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
        aiCharacter.navMeshAgent.SetPath(path);
        //Performant 
        //May Cause Frame Drop

        return this;

    }

    protected virtual void GetNewAttack(AICharacterManager aiCharacter)
    {

        potentialAttacks = new List<AICharacterAttackAction>();

        foreach(var potentialAttack in aiCharacterAttacks)
        {
            //If we are Too close for this Attack, Check the next one
            if(potentialAttack.minimumAttackDistance > aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            //If we are Too far for this Attack, Check the next one
            if (potentialAttack.maximumAttackDistance < aiCharacter.aiCharacterCombatManager.distanceFromTarget)
            {
                continue;
            }
            //If the Target is outside of the MINIMUM FOV for this Attack, Check the Next Attack
            if(potentialAttack.minimumAttackAngle > aiCharacter.aiCharacterCombatManager.viewableAngle)
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

        foreach(var attack in potentialAttacks)
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
        base.ResetStateFlags(aiCharacter);
        
        hasAttack = false;
        hasRolledForComboChance = false;
        strafeMoveAmount = 0;
        hasChoosenCirclePath = false;
   
    }

    protected virtual void SetCirclePath(AICharacterManager aiCharacter)
    {

        if(Physics.CheckSphere(aiCharacter.aiCharacterCombatManager.LockOnTransform.position, aiCharacter.characterController.radius+ 0.25f, WorldUtilityManager.Instance.GetEnviroLayer()))
        {
            //Stop Strafing/Circling Because We've Hit Something, Instead path Towards Enemy
            Debug.Log("AI Colliding with something, Ended Strafe");
            aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(0, Mathf.Abs(strafeMoveAmount));

            return;
        }

        //Strafe
        Debug.Log("AI STRAFING");
        aiCharacter.characterAnimatorManager.SetAnimatorMovementParameters(strafeMoveAmount, 0);

        if (hasChoosenCirclePath)
        {
            return;
        }



        //Strafe Left or Right
        hasChoosenCirclePath = true;

        int leftOrRightIndex = Random.Range(0, 100);

        if(leftOrRightIndex >= 60)
        {
            //Left
            strafeMoveAmount = -0.5f;
            Debug.Log("Chosen strafe direction: " + strafeMoveAmount);
        }

        else
        {
            //Right
            strafeMoveAmount = 0.5f;
            Debug.Log("Chosen strafe direction: " + strafeMoveAmount);
        }


    }

}
