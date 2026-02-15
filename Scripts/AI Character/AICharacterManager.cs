using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

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
    [HideInInspector] public AICharacterSoundFXManager aiCharacterSoundFXManager;

    [Header("NavMesh Agent")]
    public NavMeshAgent navMeshAgent;

    [Header("Chase Settings")]
    public float maxChaseDistance = 15f;
    [HideInInspector] public Vector3 spawnPosition;

    [Header("HP bar")]
    public UI_Character_HP_Bar HP_Bar;



    [Header("Current State")]
    public  AIState currentState;

    [Header("States")]
    public IdleState idle;
    public PursueTargetState pursueTarget;
    public CombatStanceState combatStance;
    public AttackState attackState;
    public InvestigateSoundState investigateSound;

    [Header("Beacon Activation Range")]
    public List<playerManager>playersWithinActivationRange = new List<playerManager>();
    [SerializeField]protected AIActivationBeacon beacon;


    private void Start()
    {
        if(aiCharacterSoundFXManager.characterDialogueID != characterDialogueID.NoDialogueID)
        {
            aiCharacterSoundFXManager.dialogueInteractableGameObject = Instantiate(WorldAIManager.instance.dialogueInteractable, transform);
        }
    }

    protected override void Awake()
    {
        
        base.Awake();
        aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
        aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
        aiCharacterSoundFXManager = GetComponent<AICharacterSoundFXManager>();

        animator = GetComponent<Animator>();
        aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
        HP_Bar = GetComponentInChildren<UI_Character_HP_Bar>();

        navMeshAgent = GetComponentInChildren<NavMeshAgent>();

        // Use copy of Scriptable object, so the original is not modified
        idle = Instantiate(idle);
        pursueTarget = Instantiate(pursueTarget);
        combatStance = Instantiate(combatStance);
        attackState = Instantiate(attackState);
        investigateSound = Instantiate(investigateSound);

        currentState = idle;

        
    }

    protected override void Update()
    {

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("isMoving", isMoving);

        OnIsChargingAttack(isChargingAttack);

        if (combatStance.canBlock)
        {
            OnIsBlocking(isBlocking);
        }

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

    public override void OnIsBlocking(bool Status)
    {
        animator.SetBool("isBlocking", isBlocking);
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
        isBlocking = false;
        isMoving = false;
        canMove = false;



        if (!manuallySelectDeathAnimation)
        {
            if(enemyType != EnemyType.exploder)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);

            }
           

            playerManager killer = PlayerInputManager.Instance.player;

            
            PlayerInputManager.Instance.player.playerCombatManager.isLockedOn = false;
            PlayerInputManager.Instance.player.playerCombatManager.currentTarget = null;
            PlayerUIManager.instance.SetLockOnTarget(null);
            //PlayerCamera.instance.SetLockCameraHeight();
            if (!killer.playerCombatManager.isAimLockedOn)
            {
                PlayerUIManager.instance.mobileControls.EnablelockOn();
            }
           

            if (killer != null && !killer.playerStatsManager.isDead)
            {
                aiCharacterCombatManager.AwardRunesOnDeath(killer);
            }
          

        }

        yield return new WaitForSeconds(3);

        aiCharacterInventoryManager.DropItem();
    }


    public void AddPlayersToPlayersWithinRange(playerManager player)
    {
        if(playersWithinActivationRange.Contains(player))
            return;

        playersWithinActivationRange.Add(player);

        for(int i = 0; i < playersWithinActivationRange.Count; i++)
        {
            if(playersWithinActivationRange[i] == null)
                playersWithinActivationRange.RemoveAt(i);
        }

    }

    public void RemovePlayerFromPlayersWithinRage(playerManager player)
    {
        if (!playersWithinActivationRange.Contains(player))
            return;

        playersWithinActivationRange.Remove(player);

        for (int i = 0; i < playersWithinActivationRange.Count; i++)
        {
            if (playersWithinActivationRange[i] == null)
                playersWithinActivationRange.RemoveAt(i);
        }

    }
    public void ActivateCharacter(playerManager player)
    {
        AddPlayersToPlayersWithinRange(player);


        if(playersWithinActivationRange.Count > 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }


    }

    public void DeactivateCharacter(playerManager player)
    {
        RemovePlayerFromPlayersWithinRage(player);


        if (beacon!=null)
        {
            beacon.gameObject.transform.position = transform.position;
            beacon.gameObject.SetActive(true);

        }


        if(playersWithinActivationRange.Count>0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            aiCharacterCombatManager.SetTarget(null);
            gameObject.SetActive(false);
        }

    }



    public void CreateActivationBeacon()
    {
        if(beacon==null)
        {
            GameObject beconGameObject = Instantiate(WorldAIManager.instance.beaconGameObject);
            beconGameObject.transform.position = transform.position; 
            
            beacon = beconGameObject.GetComponent<AIActivationBeacon>(); 
            beacon.SetOwnerOfBeacon(this);
        }
        else
        {
            beacon.transform.position = transform.position;
            beacon.gameObject.SetActive(true);
        }
    }


    public void ResetStateMachine()
    {
        
        
        isMoving = false;
        isHoldingArrow = false;
        isBlocking = false;
        hasExploded = false;
        aiCharacterCombatManager.currentTarget = null;
        aiCharacterCombatManager.targetDirection = Vector3.zero;
        aiCharacterCombatManager.distanceFromTarget = 0f;
        aiCharacterCombatManager.viewableAngle = 0f;
        aiCharacterCombatManager.actionRecoveryTimer = 0f;

        if (navMeshAgent != null)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.ResetPath();
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;
            navMeshAgent.enabled = false;
        }

        // Reset state to Idle
        currentState.SwitchState(this, idle);

        combatStance.ResetStatemachine(this);
        attackState.ResetStateMachine(this);
       
        
        

        // Reset animator
        if (animator != null)
        {
            animator.SetBool("isMoving", false);
            animator.CrossFade("Empty", 0.1f); // "Empty" = safe idle animation
        }

        // Reset stance & health related values
        aiCharacterCombatManager.stanceRegenerationTimer = 0f;
        aiCharacterCombatManager.currentStance = aiCharacterCombatManager.maxStance;
        characterStatsManager.currentHealth = characterStatsManager.maxHealth;
        characterStatsManager.isDead = false;
    }




}
