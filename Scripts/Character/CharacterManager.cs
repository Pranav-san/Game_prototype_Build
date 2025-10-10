using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterManager : MonoBehaviour
{
    public Animator animator;
    [HideInInspector] public CharacterController characterController;


    [Header("Scripts")]
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector]public CharacterCombatManager characterCombatManager;
    [HideInInspector]public CharacterLocomotionManager characterLocomotionManager;
    public CharacterStatsManager characterStatsManager;
    [HideInInspector] public CharacterSoundFxManager characterSoundFxManager;



    

    [Header("Character Group")]
    public CharacterGroup characterGroup;

    [Header("NPC")]
    public bool isFriendly = false;

    [Header("Current Weapon Type")]
    public WeapomClass currentDamageType = WeapomClass.Unarmed;








    [Header("Flags")]
    public bool applyRootMotion = false;
    public bool isPerformingAction = false;
    public bool isJumping = false;
    public bool isGrounded = true;
    public bool canRotate = true;
    public bool canMove = true;

    public bool isMoving = false;
    public bool canRun = true;
    public bool canRoll = true;
    public bool isSprinting = false;

    public bool isChargingAttack = false;
    public bool isBlocking = false;
    public bool isAttacking = false;
    public bool isInvulnerable = false;
    public bool isPerformingCriticalAttack =  false;


    public bool isGrappled = false;

    [Header("Projectile Flags")]
    public bool hasArrowNotched = false;
    public bool isHoldingArrow = false;
    public bool isAiming = false;
    public bool FireBullet = false;
    public bool isReloadedWeapon = false;
    public bool isExitingToEmptyAfterReload = false;





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

    public void TriggerHitPause(float duration)
    {
        StartCoroutine(HitPauseCoroutine(duration));
    }

    private IEnumerator HitPauseCoroutine(float duration)
    {
        Time.timeScale = 0.01f;
        Time.fixedDeltaTime = 0.01f * 0.02f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

   

}
