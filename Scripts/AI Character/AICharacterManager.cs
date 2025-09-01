using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICharacterManager : CharacterManager
{
    [Header("Character Name")]
    public string characterName = "";

    [Header("Enemy Type")]
    public EnemyType enemyType;

    [Header("Ranged Enemy Setting")]
    public float minimumTimeToAimAtTarget = 3f;
    public float maximunTimeToAimAtTarget = 6f;

    [Header("Exploder")]
    public bool hasExploded = false;
    


    public AICharacterCombatManager aiCharacterCombatManager;
    [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
    [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager;

    [Header("NavMesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Chase Settings")]
    public float maxChaseDistance = 15f;
    [HideInInspector] public Vector3 spawnPosition;

    [Header("HP bar")]
    public UI_Character_HP_Bar HP_Bar;



    [Header("Current State")]
    [SerializeField] protected AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attackState;


    [Header("NPC Quest and  Dialogue")]
    public Quest questToGive;
    [TextArea] public string[] dialogueLines;
    [SerializeField] private Sprite npcPortrait;



    protected override void Awake()
    {
        
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();

        animator = GetComponent<Animator>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        HP_Bar = GetComponentInChildren<UI_Character_HP_Bar>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // Use copy of Scriptable object, so the original is not modified
        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);
        combatStance = Instantiate(combatStance);
        attackState = Instantiate(attackState);

        currentState = idle;
    }

    protected override void Update()
    {
        base.Update();

        if (characterStatsManager.isDead)
            return;

        if (navMeshAgent==null)
            return;
        

       

        

        aiCharacterCombatManager.HandleActionRecovery(this);
        ProcessStateMachine();

        if (!navMeshAgent.enabled)
            return;

        Vector3 positiondifference = navMeshAgent.transform.position-transform.position;

        if (positiondifference.magnitude>0.2)
            navMeshAgent.transform.localPosition=Vector3.zero;
        

    }



    private void ProcessStateMachine()
    {
        if (characterStatsManager.isDead)
            return;


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

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        characterStatsManager.currentHealth =0;
        characterStatsManager.isDead = true;

        aiCharacterCombatManager.currentTarget= null;
        characterController.enabled = false;
        isMoving = false;
        canMove = false;



        if (!manuallySelectDeathAnimation)
        {
            if(enemyType != EnemyType.exploder)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);

            }
           

            playerManager killer = characterStatsManager.lastAttacker as playerManager;

            
            PlayerInputManager.Instance.player.playerCombatManager.isLockedOn = false;
            PlayerInputManager.Instance.player.playerCombatManager.currentTarget = null;
            PlayerUIManager.instance.SetLockOnTarget(null);
            PlayerCamera.instance.SetLockCameraHeight();

            if (killer != null)
            {
                aiCharacterCombatManager.AwardRunesOnDeath(killer);
            }



           PlayerUIManager.instance.SetLockOnTarget(null);
          

        }

        yield return new WaitForSeconds(5);

    }



    public void TryTalkToNPC(playerManager player)
    {
        if (!isFriendly || dialogueLines.Length == 0)
        {
            Debug.LogWarning("NPC has no dialogue lines.");
            return;
        }
          

        PlayerUIManager.instance.playerUIPopUPManager.StartDialogue(characterName,dialogueLines);
    }


    public void ResetStateMachine()
    {
        // Reset state to Idle
        currentState = idle;


        combatStance.ResetStatemachine(this);
        attackState.ResetStateMachine(this);

        // Clear combat values
        aiCharacterCombatManager.currentTarget = null;
        aiCharacterCombatManager.targetDirection = Vector3.zero;
        aiCharacterCombatManager.distanceFromTarget = 0f;
        aiCharacterCombatManager.viewableAngle = 0f;
        aiCharacterCombatManager.actionRecoveryTimer = 0f;

        // Reset locomotion
        isMoving = false;
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
        }

        // Reset animator
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.CrossFade("Empty", 0.1f); // "Empty" = safe idle animation
        }

        // Reset stance & health related values
        aiCharacterCombatManager.stanceRegenerationTimer = 0f;
        aiCharacterCombatManager.currentStance = aiCharacterCombatManager.maxStance;
    }




}
