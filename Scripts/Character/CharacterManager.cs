using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public CharacterEffectsManager characterEffectsManager;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector]public CharacterCombatManager characterCombatManager;
    [HideInInspector]public CharacterLocomotionManager characterLocomotionManager;
    public CharacterStatsManager characterStatsManager;

    public bool isDead;

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
        
        
    }
    protected virtual void Update()
    {

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("isMoving", isMoving);

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
