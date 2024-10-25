using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    playerManager player;
    public CharacterManager characterManager;
    [SerializeField]PlayerUIHUDManager playerUIHUDManager;

    public float verticalMovement;
    public float horizontalMovement;
    public float movementAmount;

    [Header("Movement Settings")]
    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 7;
    [SerializeField] float rotationSpeed = 15;
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;

    //[SerializeField] int jumpStaminaCost = 2;

    [Header("Dodge")]
    private Vector3 rollDirection;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<playerManager>();
        characterManager = GetComponent<CharacterManager>();
        
    }
    private void Start()
    {
        playerUIHUDManager = PlayerUIManager.instance.playerUIHUDManager.GetComponent<PlayerUIHUDManager>();
    }
    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();

    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.Instance.verticalInput;
        horizontalMovement = PlayerInputManager.Instance.horizontalInput;
        movementAmount = PlayerInputManager.Instance.moveAmount;
    }
    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();

        if (!player.canMove)
            return;
        // Our MovementDirection is based on Cameras Perspective
        moveDirection = PlayerCamera.instance.transform.forward* verticalMovement;
        moveDirection += moveDirection +PlayerCamera.instance.transform.right* horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (characterManager.isSprinting)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                //Move At Running Speed
                player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                //Move At Walking Speed
                player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
            }

        }


    }

    private void HandleRotation()
    {
        if(player.isDead) 
            return;


        if (!player.canRotate)
            return;

        if (player.playerCombatManager.isLockedOn||player.playerLocomotionManager.isRolling)
        {
            if (player.isSprinting) 
            { 
                Vector3 targetDirection =  Vector3.zero;
                targetDirection = PlayerCamera.instance.cameraObject.transform.forward* verticalMovement;
                targetDirection+= PlayerCamera.instance.cameraObject.transform.right*horizontalMovement;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection==Vector3.zero)
                {
                    targetDirection = transform.forward;
                }
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed* Time.deltaTime);
                transform.rotation = finalRotation;

            }
            else
            {
                if ( player.playerCombatManager.currentTarget == null)
                {
                    return;
                }

                Vector3 targetdirection;
                targetdirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetdirection.y = 0;
                targetdirection.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(targetdirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation,rotationSpeed* Time.deltaTime);
                transform.rotation = finalRotation;

            }

        }
        else
        {
            Vector3 targetRotationDirection = Vector3.zero;
            if (PlayerInputManager.Instance.moveAmount > 0)
            {
                targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                targetRotationDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
            }
            else
            {
                targetRotationDirection = player.transform.forward; // Align with player's forward direction when no movement
            }
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;

        }

        
    }

    public void AttemptToPerformDodge()
    {
        if (player.isPerformingAction)
            return;
        if (!player.isGrounded)
            return;

        if (PlayerInputManager.Instance.moveAmount > 0)
        {
            rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
            rollDirection += PlayerCamera.instance.cameraObject.transform.right *  PlayerInputManager.Instance.horizontalInput;

            rollDirection.y = 0;
            rollDirection.Normalize();

            Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
            player.transform.rotation = playerRotation;

            //perform Roll Animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Roll", true, true);
            player.playerLocomotionManager.isRolling = true;    

        }
        else
        {
            //Backstep ANimation

        }



    }

    public void HandleSprinting()
    {
        float currentStamina = player.characterStatsManager.currentStamina;
        float sprintingStaminaCost = player.sprintingStaminaCost;

        // Calculate new stamina based on whether the player is starting or stopping sprinting

        float deltaTime = Time.deltaTime;

        float oldStamina = currentStamina;
        float newStamina = Mathf.Max(currentStamina - sprintingStaminaCost * deltaTime, -2);
        if (player.isPerformingAction)
        {
            characterManager.isSprinting = false;
        }
        if (movementAmount >= 0.5f)
        {
            characterManager.isSprinting = true;
            player.characterStatsManager.ConsumeStamina(sprintingStaminaCost * deltaTime);
            // Convert oldStamina and newStamina to int before passing them to SetNewStaminaValue
            playerUIHUDManager.SetNewStaminaValue((int)oldStamina, (int)newStamina);
        }
        else
        {
            characterManager.isSprinting = false;
        }

    }
    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction)
            return;

        
        



    }

    
}
