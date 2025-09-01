using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.LightAnchor;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    playerManager player;
    public CharacterManager characterManager;
    [SerializeField] PlayerUIHUDManager playerUIHUDManager;

    public float verticalMovement;
    public float horizontalMovement;
    public float movementAmount;

    [Header("Movement Settings")]


    [SerializeField] float walkingSpeed = 2;
    [SerializeField] float runningSpeed = 5;
    [SerializeField] float sprintingSpeed = 7;
    [SerializeField] float rotationSpeed = 15;
    [SerializeField] float strafeWalkingSpeed = 1.5f;
    [SerializeField] float strafeRunningspeed = 3.5f;
    [SerializeField] float aimWalkingSpeed = 1.5f;
    [SerializeField] float aimRunningspeed = 3.5f;
    private Vector3 moveDirection;
    private Vector3 targetRotationDirection;
    public bool hasPlayedSprintStart = false;

    [Header("Jump")]
    [SerializeField] float jumpHeight = 3;
    private Vector3 jumpDirection;

    

    [Header("Surface Type")]
    public SurfaceType CurrentsurfaceType;

    [Header("Ladder")]
    private Vector3 ladderBottom;
    private Vector3 ladderTop;
    private Vector3 ladderAxis;
    private float ladderLength;
    [SerializeField] float ladderProgress;
    [SerializeField, Range(0f, 1f)] private float topExitThreshold = 0.9f;
    [SerializeField, Range(0f, 1f)] private float bottomExitThreshold = 0.1f;
    public bool isExitingLadder = false;






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

        if (isClimbingLadder)
        {
            HandleLadderMovement();
            return;

        }

        HandleGroundedMovement();
        HandleRotation();
        HandleJumpingMovement();



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
        {
            player.characterController.Move(Vector3.zero);
            return;
        }

        if (!player.canMove || player.playerInteractionManager.isInspectingObject)
            return;



        // Our MovementDirection is based on Cameras Perspective

        //if (player.isAiming)
        //{
        //    moveDirection = transform.forward* verticalMovement;
        //    moveDirection += moveDirection +  transform.right* horizontalMovement;
        //    moveDirection.Normalize();
        //    moveDirection.y = 0;

        //}


        moveDirection = PlayerCamera.instance.transform.forward* verticalMovement;
        moveDirection += moveDirection +PlayerCamera.instance.transform.right* horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;



        if (characterManager.isSprinting)
        {
            player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
        }
        else if (player.playerCombatManager.isLockedOn)
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                //Move At StrafeRunning Speed
                player.characterController.Move(moveDirection * strafeRunningspeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                //Move At StrafeWalking Speed
                player.characterController.Move(moveDirection * strafeWalkingSpeed * Time.deltaTime);
            }

        }
        else if (player.isAiming)
        {
            if (PlayerInputManager.Instance.moveAmount > 0.5f)
            {
                //Move At StrafeRunning Speed
                player.characterController.Move(moveDirection * aimRunningspeed * Time.deltaTime);
            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
            {
                //Move At StrafeWalking Speed
                player.characterController.Move(moveDirection * aimWalkingSpeed * Time.deltaTime);
            }

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

    private void HandleLadderMovement()
    {
        if (!isClimbingLadder || currentLadderInteractable == null) return;
        if (isExitingLadder)
            return;

        float vertical = PlayerInputManager.Instance.verticalInput;



        // Feed animator
        player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, vertical, false, false);

        // Move using CharacterController
        if (Mathf.Abs(vertical) > 0.05f)
        {
            Vector3 climbDir = Vector3.up * vertical * currentLadderInteractable.climbSpeed * Time.deltaTime;
            transform.position += climbDir;
        }

        Vector3 toPlayer = transform.position - ladderBottom;
        ladderProgress = Vector3.Dot(toPlayer, ladderAxis) / ladderLength;

        if (ladderProgress >= topExitThreshold && vertical > 0.05f) //Close to Top, Stop Climbing Up
        {
            ExitLadder(currentLadderInteractable);
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false, false);
            return;
        }
        if (ladderProgress <= bottomExitThreshold && vertical < -0.05f) //Close to Bottom, Stop Climbing down
        {
            ExitLadder(currentLadderInteractable);
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false, false);
            return;
        }

        // keep facing the ladder

    }



    public void EnterLadder(LadderInteractable ladder)
    {

        currentLadderInteractable = ladder;

        if (currentLadderInteractable == null)
            return;

        if (ladder.isTopPoint && !isClimbingLadder)
        {
            Vector3 targetDirection = ladder.ladderTopStartPoint.forward;
            targetDirection.y = 0;
            if (targetDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(targetDirection);

            transform.position = ladder.ladderTopStartPoint.position;

            player.playerAnimatorManager.PlayTargetActionAnimation("Climb_Down_Start 0", false, false);
            isClimbingLadder = true;


        }
        else if (!ladder.isTopPoint && !isClimbingLadder)
        {
            Vector3 targetDirection = ladder.ladderTopPoint.forward;
            targetDirection.y = 0;
            if (targetDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(targetDirection);

            transform.position = ladder.ladderBottomPoint.position;

            player.playerAnimatorManager.PlayTargetActionAnimation("Climb_Up_Start", false, false);
            isClimbingLadder =true;


        }



        // Enter ladder state
        isClimbingLadder = true;
        player.animator.SetBool("isClimbingLadder", true);

        ladderBottom = ladder.ladderBottomPoint.position;
        ladderTop = ladder.ladderTopPoint.position;
        ladderAxis = (ladderTop - ladderBottom);
        ladderLength = ladderAxis.magnitude;
        ladderAxis.Normalize();
    }

    public void ExitLadder(LadderInteractable ladder)
    {
        if (!isClimbingLadder || currentLadderInteractable == null)
            return;

        isClimbingLadder = false;
        isExitingLadder = true;


        // Decide if it's top or bottom based on ladderProgress
        if (ladderProgress > 0.1)
        {
            // Snap to top
            transform.position = ladderTop;

            // Play climb-up exit animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Climb_Up_To_Idle", true);
        }
        else if (ladderProgress <= 0.01f)
        {
            // Snap to bottom
            transform.position = ladderBottom;

            // Play climb-down exit animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Climb_Down_To_Idle", true);
        }

        currentLadderInteractable = null;

    }

    public void PlayFootstepSfx()
    {
        // Only when grounded
        if (!player.isGrounded) return;

        if (player.isJumping)
            return;
       
        if (!player.isMoving)
            return;

        if (player.playerStatsManager.isDead)
        {
            return;
            
        }

        if (player.isPerformingAction)
            return;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 2f, groundLayer))
        {
            CurrentsurfaceType = SurfaceType.Default;

            if (hit.collider.TryGetComponent<FootstepSurface>(out var surf))
                CurrentsurfaceType = surf.surface;
            player.characterSoundFxManager.PlayFootstepSfx(CurrentsurfaceType);


        }
        else
        {
            // fallback default
            CurrentsurfaceType = SurfaceType.Default;
            player.characterSoundFxManager.PlayFootstepSfx(CurrentsurfaceType);

        }
    }






    //Third Person
    private void HandleRotation()
    {
        if (player.playerStatsManager.isDead)
            return;


        if (!player.canRotate)
            return;


        if (player.isAiming)
        {
            HandleAimedRotation();
        }
        else
        {
            HandleStandardRotation();
        }




    }

    private void HandleStandardRotation()
    {


        if (player.playerCombatManager.isLockedOn||player.playerLocomotionManager.isRolling)
        {
            if (player.isSprinting)
            {
                Vector3 targetDirection = Vector3.zero;
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
                if (player.playerCombatManager.currentTarget == null)
                {
                    return;
                }

                Vector3 targetdirection;
                targetdirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                targetdirection.y = 0;
                targetdirection.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(targetdirection);
                Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed* Time.deltaTime);
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
                targetRotationDirection = player.transform.forward; // Align with character's forward direction when no movement
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

    private void HandleJumpingMovement()
    {
        if (player.isJumping)
        {
            player.characterController.Move(jumpDirection*runningSpeed* Time.deltaTime);
        }
    }

    private void HandleAimedRotation()
    {

        Vector3 targetdirection;
        targetdirection = PlayerCamera.instance.transform.forward;
        targetdirection.y = 0;
        targetdirection.Normalize();


        Quaternion targetRotation = Quaternion.LookRotation(targetdirection);
        Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed* Time.deltaTime);
        transform.rotation = finalRotation;


    }

    //First Person
    private void HandleFirstPersonRotation()
    {
        // Apply camera input for first-person rotation
        float cameraHorizontalInput = PlayerInputManager.Instance.cameraHorizontalInput;
        float cameraVerticalInput = PlayerInputManager.Instance.cameraVerticalInput;

        // Adjust the character's rotation based on the camera input
        Quaternion cameraRotation = Quaternion.Euler(0, cameraHorizontalInput * rotationSpeed * Time.deltaTime, 0);
        transform.rotation *= cameraRotation; // Rotate character based on camera horizontal input

        // You can also handle vertical rotation if needed, but this is usually handled by the camera itself
    }

    public void AttemptToPerformDodge()
    {

        if (!player.isGrounded)
            return;

        if (player.canRoll)
        {


            if (PlayerInputManager.Instance.moveAmount > 0 && !player.playerCombatManager.isLockedOn)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right *  PlayerInputManager.Instance.horizontalInput;

                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                //perform Roll Animation
                player.isMoving = false;
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll", true, true);
                player.playerLocomotionManager.isRolling = true;

            }
            else
            {
                //Backstep ANimation

            }

            //OmniDirectional Dodge when character is Locked ON
            if (PlayerInputManager.Instance.moveAmount > 0 && player.playerCombatManager.isLockedOn)
            {
                float horizontal = PlayerInputManager.Instance.horizontalInput;
                float vertical = PlayerInputManager.Instance.verticalInput;

                if (vertical > 0.5f)
                {
                    player.canMove = false;
                    player.playerAnimatorManager.PlayTargetActionAnimation("Roll Forward", true, true);

                }

                else if (vertical < -0.5f)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("Roll Backward", true, true);

                }

                else if (horizontal > 0.5f)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("Roll Right", true, true);
                }
                else if (horizontal < -0.5f)
                {
                    player.playerAnimatorManager.PlayTargetActionAnimation("Roll Left", true, true);
                }
            }
        }



    }

    public void HandleSprinting()
    {
        if (player.isAiming)
            return;
        float currentStamina = player.playerStatsManager.currentStamina;
        float sprintingStaminaCost = player.sprintingStaminaCost;

        // Calculate new stamina based on whether the character is starting or stopping sprinting

        float deltaTime = Time.deltaTime;

        float oldStamina = currentStamina;
        float newStamina = Mathf.Max(currentStamina - sprintingStaminaCost * deltaTime, -2);
        if (player.isPerformingAction)
        {
            characterManager.isSprinting = false;
        }
        if (movementAmount >= 0.5f)
        {
            player.isSprinting = true;


            player.playerStatsManager.ConsumeStamina(sprintingStaminaCost * deltaTime);
            // Convert oldStamina and newStamina to int before passing them to SetNewStaminaValue
            playerUIHUDManager.SetNewStaminaValue((int)oldStamina, (int)newStamina);
        }
        else
        {
            player.isSprinting = false;
            hasPlayedSprintStart =false;
        }

    }

    public void ApplyJumpVelocity()
    {
        yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
    }

    public void AttemptToPerformJump()
    {
        if (player.isPerformingAction)
            return;

        if (player.playerStatsManager.currentStamina<=0)
            return;

        if (player.playerStatsManager.isDead)
            return;

        if (!player.isGrounded)
            return;

        player.playerAnimatorManager.PlayTargetActionAnimation("Jump_01", true);


        player.playerStatsManager.ConsumeStamina(player.jumpStaminaCost);

        player.isJumping = true;


        //JumpDirection based on Movement Input
        jumpDirection =  PlayerCamera.instance.cameraObject.transform.forward* PlayerInputManager.Instance.verticalInput;
        jumpDirection +=  PlayerCamera.instance.cameraObject.transform.forward* PlayerInputManager.Instance.horizontalInput;

        jumpDirection.y = 0;


        if (jumpDirection!=Vector3.zero)
        {

            if (player.isSprinting)
            {
                jumpDirection *= 1;

            }
            else if (PlayerInputManager.Instance.moveAmount >= 0.5)
            {
                jumpDirection *= 0.5f;



            }
            else if (PlayerInputManager.Instance.moveAmount <= 0.5)
            {

                jumpDirection *= 0.25f;
            }

        }




    }


}
