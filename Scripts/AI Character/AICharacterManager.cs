using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";
    
    
    public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;

    [Header("NavMesh Agent")]
    public NavMeshAgent navMeshAgent;

    

    [Header("Current State")]
    [SerializeField] AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attackState;

    protected override void Awake()
    {
        
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        animator = GetComponent<Animator>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // Use copy of Scriptable object, so the original is not modified
        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);

        currentState = idle;
    }

    protected override void Update()
    {
        base.Update();

        aiCharacterCombatManager.HandleActionRecovery(this);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        ProcessStateMachine();
    }

    private void ProcessStateMachine()
    {
        AIState nextState = currentState?.Tick(this);
        if (nextState != null)
        {
            currentState = nextState;
        }

        // Ensure NavMeshAgent stays at local position and rotation (important if it's a child object)
        navMeshAgent.transform.localPosition = Vector3.zero;
        navMeshAgent.transform.localRotation = Quaternion.identity;

        if(aiCharacterCombatManager.currentTarget != null)
        {
            aiCharacterCombatManager.targetDirection= aiCharacterCombatManager.currentTarget.transform.position - transform.position;
            aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetDirection);
            aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
        }

        // Control the character movement based on NavMeshAgent
        if (navMeshAgent.enabled)
        {
            Vector3 agentDestination = navMeshAgent.destination;
            float remainingDistance = Vector3.Distance(agentDestination, transform.position);

            if (remainingDistance > navMeshAgent.stoppingDistance)
            {
                // Tell the animator that the character is moving
                isMoving = true;
                aiCharacterLocomotionManager.MoveCharacter(this);

            }
            else
            {
                // Tell the animator that the character is not moving
                isMoving = false;
            }
        }
        else
        {
            isMoving = false;
        }

        // Update animator with the movement state
        animator.SetBool("isMoving", isMoving);
    }

    
}
