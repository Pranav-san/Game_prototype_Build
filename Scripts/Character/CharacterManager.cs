using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector]public CharacterCombatManager characterCombatManager;
    [HideInInspector]public CharacterLocomotionManager characterLocomotionManager;
    public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterSoundFxManager characterSoundFxManager;

    //public bool isDead;

    public float maxHealth = 100;
    public float currentHealth = 100;   



    public Animator animator;

    [Header("Character Group")]
    public CharacterGroup characterGroup;

    






    [Header("Flags")]
    public bool applyRootMotion = false;
    public bool isPerformingAction = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool canRotate = true;
    public bool canMove = true;
    public bool isMoving = false;

    public bool isSprinting = false;

    public bool isChargingAttack = false;

    public bool isBlocking = false;
    public bool isAttacking = false;
    public bool isInvulnerable = false;



    

    protected virtual void Awake()
    {
        DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>(); 
        characterLocomotionManager= GetComponent<CharacterLocomotionManager>(); 
        characterSoundFxManager = GetComponent<CharacterSoundFxManager>();
        
        
    }

    public void OnIsChargingAttack( bool Status)
    {
        animator.SetBool("isChargingAttack", isChargingAttack);
    }
    protected virtual void Update()
    {

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("isMoving", isMoving);

        OnIsChargingAttack(isChargingAttack);
        OnIsBlocking(isBlocking);

    }

    public virtual void OnIsBlocking( bool Status)
    {
        animator.SetBool("isBlocking", isBlocking);
    }
    protected virtual void LateUpdate()
    {
        

    }

    public virtual IEnumerator ProcessDeathEvent(bool  manuallySelectDeathAnimation  = false)
    {
        characterStatsManager.currentHealth =0;
        characterStatsManager.isDead = true;
        

        if (!manuallySelectDeathAnimation)
        {
            characterAnimatorManager.PlayTargetActionAnimation("Dead_01",true);
            PlayerCamera.instance.player.playerCombatManager.CheckIfTargetIsDead(PlayerCamera.instance.player.playerCombatManager.currentTarget);

        }

        yield return new WaitForSeconds(5);

   }

    
    public virtual void ReviveCharacter()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

}
