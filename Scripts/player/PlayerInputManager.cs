using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    [SerializeField] bool useFloatingJoyStick = false;
    public playerManager player;
    PlayerControls2 playerControls;
    PlayerUIHUDManager playerUIHUDManager;

    [Header("Camera Movement Input")]
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    [SerializeField] Vector2 cameraInput;

    [Header("Mobile Inputs")]
    [SerializeField] FloatingJoystick floatingJoystick;




    [Header("Lock On")]
    [SerializeField] public bool LockOn_Input;
    [SerializeField] bool LockOn_Left_Input;
    [SerializeField] bool LockOn_Right_Input;
    private Coroutine lockOnCoroutine;


    [Header("Player Movement Input")]
    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;
    private Vector2 lastMovementInput;

    [Header("Player Actions")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;

    [SerializeField] bool use_Item_Input = false;

    [SerializeField] bool Switch_Right_Weapon_Input = false;
    [SerializeField] bool Switch_Left_Weapon_Input = false;
    [SerializeField] bool Switch_Quick_Slot_Item_Input = false;

    [Header("Bumper Input")]
    public bool RB_Input = false;
    public bool Hold_RB_Input = false;
    [SerializeField] bool LB_Input = false;
    [SerializeField] bool Hold_LB_Input = false;

    [Header("Two Hand Input")]
    [SerializeField] public bool Two_Hand_Input = false;


    [Header("Trigger Input")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool Hold_RT_Input = false;


    [SerializeField] bool interaction_Input = false;

    [Header("UI Inputs")]
    [SerializeField] bool openCharacterMenuInput = false;
    [SerializeField] bool closeMenuInput = false;
    


    [Header("Qued Inputs")]
    private bool is_Input_Que_Active = false;
    [SerializeField] float default_Que_Input_Time = 0.35f;
    [SerializeField] float qued_Input_Timer = 0;
    [SerializeField] bool que_RB_Input = false;
    [SerializeField] bool que_LB_Input = false;


    [Header("Aim Setting")]
    [SerializeField] float rbHoldThreshold = 0.25f;
    float rbHoldTimer;
    bool rbAimTriggered;

    [SerializeField] bool oneHandRBDecisionPending;











    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }



    }

    private void Start()
    {

        DontDestroyOnLoad(gameObject);




        SceneManager.activeSceneChanged += OnSceneChange;
        Instance.enabled = false;





    }

    private void OnSceneChange(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
        {
            Instance.enabled = true;
        }
        else
        {
            cameraVerticalInput = 0;
            cameraHorizontalInput = 0;

            Instance.enabled = false;
        }



    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls2();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
            playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
            playerControls.PlayerActions.SwitchRightWeapon.performed += i => Switch_Right_Weapon_Input = true;
            playerControls.PlayerActions.SwitchLeftWeapon.performed += i => Switch_Left_Weapon_Input = true;
            playerControls.PlayerActions.SwitchQuickSlotItem.performed += i => Switch_Quick_Slot_Item_Input= true;

            playerControls.PlayerActions.X.performed += i => use_Item_Input = true;

            playerControls.PlayerActions.InteractionKey.performed += i => interaction_Input = true;

            playerControls.PlayerActions.TwoHandWeapon.performed += i => Two_Hand_Input = true;

            //Bumpers
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;
            playerControls.PlayerActions.LB.performed += i => LB_Input = true;
            playerControls.PlayerActions.LB.canceled += i => player.isBlocking = false;
            //playerControls.PlayerActions.LB.canceled += i => player.isAiming = false;
            playerControls.PlayerActions.LB.canceled += i =>
            {
                if (!player.playerCombatManager.isAimLockedOn)
                {
                    player.isAiming = false;
                    PlayerCamera.instance.OnIsAimingChanged(false);
                }

            };


            playerControls.PlayerActions.RT.performed += i => RT_Input = true;

            playerControls.PlayerActions.RB.performed += i => Hold_RB_Input = true;
            playerControls.PlayerActions.RB.canceled += i => Hold_RB_Input = false;

            playerControls.PlayerActions.LB.performed += i => Hold_LB_Input = true;
            playerControls.PlayerActions.LB.canceled += i => Hold_LB_Input = false;



            //Triggers
            playerControls.PlayerActions.HoldRT.performed += i => Hold_RT_Input = true;
            playerControls.PlayerActions.HoldRT.canceled += i => Hold_RT_Input = false;

            //Lock On
            playerControls.PlayerActions.LockOn.performed += i => LockOn_Input = true;
            playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => LockOn_Left_Input = true;
            playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => LockOn_Right_Input = true;

            playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            //UI Inputs
            playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;
            //playerControls.PlayerActions.OpenSurvivalWheel.performed += i => openSurvivalWheelInput = true;
            
            //Qued Inputs
            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);
            playerControls.PlayerActions.QueLB.performed += i => QueInput(ref que_LB_Input);



        }
        playerControls.Enable();


    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChange;
    }

    private void Update()
    {


        HandleAllInputs();
        ForceStateUpdate();





    }

    private void HandleAllInputs()
    {
        if (player.playerStatsManager.isDead)
            return;

        HandleAllQuedInput();

        HandlePlayerMovementInput();


        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleSprinting();

        HandleJumpInput();


        HandleRBInput();
        HandleLBInput();

        HandleTwoHandInput();

        HandleHoldRBInput();

        HandleSwitchRightWeaponInput();
        HandleSwitchLefttWeaponInput();
        HandleQuickSlotItemInput();

        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandleInteractionInput();
        HandleOpenCharacterMenuInput();

        HandleRTInput();
        HandleChargeRTInput();

        HandleUseItemInput();

    }

    private void ProcessQuedInput()
    {

        if (player.playerStatsManager.isDead)
            return;


        if (que_RB_Input)
        {
            RB_Input = true;

        }
        if (que_LB_Input)
        {
            LB_Input = true;
        }
    }

    private void HandleLockOnInput()
    {
        //AIM Lock

        if(LockOn_Input && player.playerInventoryManager.currentTwoHandWeapon !=null && player.playerEquipmentManager.isTwoHandingWeapon)
        {
            if (player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow || player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun)
            {
                if (LockOn_Input && !player.playerCombatManager.isAimLockedOn)
                {

                    LockOn_Input = false;
                    player.playerCombatManager.isAimLockedOn= true;
                    PlayerCamera.instance.OnIsAimingChanged(player.playerCombatManager.isAimLockedOn);
                   

                    if(player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
                    {
                        TriggerBowAim();
                        
                    }
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_LB_Action, player.playerInventoryManager.currentRightHandWeapon);

                    PlayerUIManager.instance.mobileControls.EnableAimLockOn();
                    PlayerUIManager.instance.mobileControls.ToggleLeftFireButton(player.playerCombatManager.isAimLockedOn);

                }

                if (LockOn_Input && player.playerCombatManager.isAimLockedOn && player.isAiming)
                {
                    LockOn_Input = false;
                    player.playerCombatManager.isAimLockedOn = false;
                    player.isAiming = false;

                    if(player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
                    {
                        ExitBowAim();
                    }


                    PlayerCamera.instance.OnIsAimingChanged(player.isAiming);
                    PlayerUIManager.instance.mobileControls.EnableAimLockOn();
                    PlayerUIManager.instance.mobileControls.ToggleLeftFireButton(player.playerCombatManager.isAimLockedOn);

                }

            }
            else
            {
                //Two Hand Melee Werapon
            }

        }

        
        

        //Disable Lock On

        if (LockOn_Input && player.playerCombatManager.isLockedOn &&  !player.playerEquipmentManager.isTwoHandingWeapon)
        {

            LockOn_Input = false;
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerCombatManager.isLockedOn = false;
            PlayerUIManager.instance.SetLockOnTarget(null);
            player.playerCombatManager.currentTarget = null; // Clear current target
            PlayerUIManager.instance.mobileControls.EnablelockOn();
            return;
        }

        //Enable Lock On

        if (LockOn_Input && !player.playerCombatManager.isLockedOn && !player.playerEquipmentManager.isTwoHandingWeapon)
        {
            LockOn_Input = false;

            PlayerCamera.instance.HandleLocatingLockOnTarget();

            if (PlayerCamera.instance.nearestLockOnTarget!=null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerCombatManager.isLockedOn = true;
                PlayerUIManager.instance.SetLockOnTarget(player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform);
                PlayerUIManager.instance.mobileControls.EnablelockOn();


            }
            return;

        }

        if (player.playerCombatManager.isLockedOn && !player.playerEquipmentManager.isTwoHandingWeapon)
        {
            if (player.playerCombatManager.currentTarget==null)
                return;

            if (!PlayerCamera.instance.autoSwitchToNearestTarget)
                return;


            //Attempt to Find new Target 

            //This Ensures that Couroutine never runs multiple times overlapping itself
            if (lockOnCoroutine !=null)
                StopCoroutine(lockOnCoroutine);

            lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenfindNewTarget());



        }



    }


    private void HandleUseItemInput()
    {
        if (use_Item_Input)
        {
            use_Item_Input = false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

            if (player.playerInventoryManager.currentQuickSlotItem != null)
            {
                player.playerInventoryManager.currentQuickSlotItem.AttemptToUseItem(player);
            }
        }
    }

    private void HandleTwoHandInput()
    {
        if (player.isAttacking)
            return;
        if (player.isPerformingAction)
            return;

        if (Two_Hand_Input)
        {
            Two_Hand_Input = false;

            if (player.playerInventoryManager.weaponsInTwoHandSlot[0] == null)
            {
                player.playerEquipmentManager.isTwoHandingWeapon = false;
                return;

            }

            if (player.playerInventoryManager.weaponsInTwoHandSlot != null && player.playerEquipmentManager.isTwoHandingWeapon)
            {

                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.animator.SetBool("isAiming", player.isAiming);
                    PlayerCamera.instance.OnIsAimingChanged(player.isAiming);
                }
                // Unload two-hand weapon, restore previous one-handed weapons
                player.playerEquipmentManager.UnloadTwoHandWeaponAndRestore();
                player.playerEquipmentManager.isTwoHandingWeapon = false;
                PlayerUIManager.instance.mobileControls.ToggleTwoHandButtonIcon(false);

            }
            else
            {
                // Save current one-handed weapons before switching to 2H
                player.playerEquipmentManager.SaveCurrentOneHandedWeapons();
                player.playerEquipmentManager.isTwoHandingWeapon = true;
                PlayerUIManager.instance.mobileControls.ToggleTwoHandButtonIcon(true);
                player.playerEquipmentManager.LoadTwoHandWeapon();

            }

        }
    }

    private void HandleLockOnSwitchTargetInput()
    {
        if (LockOn_Left_Input)
        {
            LockOn_Left_Input = false;

            if (player.playerCombatManager.isLockedOn)
            {
                PlayerCamera.instance.HandleLocatingLockOnTarget();

                if (PlayerCamera.instance.leftLockOnTarget!=null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                }
            }
        }
        if (LockOn_Right_Input)
        {
            LockOn_Right_Input = false;

            if (player.playerCombatManager.isLockedOn)
            {
                PlayerCamera.instance.HandleLocatingLockOnTarget();

                if (PlayerCamera.instance.RightLockOnTarget!=null)
                {
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.RightLockOnTarget);
                }
            }
        }

    }

    private void HandlePlayerMovementInput()
    {



        if (useFloatingJoyStick)
        {
            if (floatingJoystick != null && floatingJoystick.gameObject.activeInHierarchy)
            {
                movementInput = floatingJoystick.GetInput();
            }

        }




        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput));

        if (!player.canMove || player.isPerformingAction)
        {
            moveAmount = 0;
            return;

        }



        if (moveAmount<=0.5 && moveAmount > 0&& !player.isAiming)
        {
            moveAmount = 0.5f;
        }
        else if (moveAmount>0.5&&  moveAmount <= 1)
        {
            moveAmount = 1;
        }
        if (player == null)
            return;

        if (moveAmount!=0)
        {
            player.isMoving = true;
        }
        else
        {
            player.isMoving=false;
        }

        if (!player.canRun)
        {
            if (moveAmount>0.5)
            {
                moveAmount = 0.5f;
            }
            if (verticalInput>0.5)
            {
                verticalInput = 0.5f;
            }
            if (horizontalInput>0.5)
            {
                horizontalInput = 0.5f;
            }
        }

        //if (player.isSprinting && movementInput.sqrMagnitude > 0.01f && lastMovementInput.sqrMagnitude > 0.01f)
        //{
        //    float dot = Vector2.Dot(lastMovementInput.normalized, movementInput.normalized);

        //    if (dot < -0.9f) // ~180° opposite direction
        //    {
        //        player.playerAnimatorManager.PlayTargetActionAnimation("Turn180", true, true, false, false, false);
        //        player.isPerformingAction = true;

        //        // update last input & STOP processing locomotion this frame
        //        lastMovementInput = movementInput;
        //        return;
        //    }
        //}

        //lastMovementInput = movementInput;



        if (player.isAiming)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting, player.isAiming);


        }

        if (!player.playerCombatManager.isLockedOn && !player.isAiming || player.isSprinting)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting, player.isAiming);
        }
        else
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting, player.isAiming);
        }






    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;


    }
    private void HandleDodgeInput()
    {
        if (dodgeInput == true)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }

    }
    private void HandleSprinting()
    {

        // Immediately stop sprinting if stamina is 0
        if (player.characterStatsManager.currentStamina <= 0 || player.isAiming)
        {
            sprintInput = false; // Prevent further sprinting input
            player.isSprinting = false; // Stop sprinting immediately
            player.playerLocomotionManager.characterManager.isSprinting = false; // Ensure character manager knows
            return; // Exit the method early
        }

        // Only proceed if there's valid sprint input and stamina is sufficient
        if (sprintInput && player.characterStatsManager.currentStamina > 0)
        {
            player.playerLocomotionManager.HandleSprinting(); // Proceed with sprinting
        }
        else
        {
            player.isSprinting = false; // Ensure sprinting is stopped
        }
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput= false;


            player.playerLocomotionManager.AttemptToPerformJump();

        }

    }
    public void ForceStateUpdate()
    {
        // This method forces a reevaluation of the character's sprinting state
        // Call this method at appropriate times, e.g., after stamina changes or at the end of each frame/update cycle
        HandleSprinting();


    }

    private void HandleRBInput()
    {
        if (PlayerUIManager.instance.menuWindowOpen)
            return;
        if (PlayerUIManager.instance.isLoadingScreenActive)
            return;

        //We Are Using An Item Return
        if (player.playerCombatManager.isUsingItem)
            return;

       



        //if (player.playerInventoryManager.currentTwoHandWeapon != null && player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow && player.playerCombatManager.isAimLockedOn)
        //{
        //    if (!player.isAiming)
        //        return;

        //    RB_Input = false;

        //    player.isUsingRightHand = true;
        //    player.currentDamageType = WeapomClass.Bow;

        //    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_RB_Action,player.playerInventoryManager.currentRightHandWeapon);

        //    return; 
        //}



        if (player.playerInventoryManager.currentTwoHandWeapon != null)
        {
            if (RB_Input == true && player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun &&
            !player.playerCombatManager.isAimLockedOn && !player.isAiming)
            {
                RB_Input = false;
                LockOn_Input = true;

            }

        }

        




        if (RB_Input == true)
        {
            RB_Input = false;


            if (player.playerStatsManager.currentStamina < player.playerInventoryManager.currentRightHandWeapon.baseStaminaCost)
                return;
            player.isUsingRightHand = true;
            player.currentDamageType = player.playerInventoryManager.currentRightHandWeapon.weaponClass;
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_RB_Action, player.playerInventoryManager.currentRightHandWeapon);

        }

    }

    private void QueInput(ref bool quedInput)
    {
        que_RB_Input = false;
        que_LB_Input = false;

        if (player.isAiming)
            return;




        if (player.isPerformingAction || player.isAttacking)
        {
            quedInput = true;
            qued_Input_Timer = default_Que_Input_Time;
            is_Input_Que_Active = true;


        }

    }

    private void HandleAllQuedInput()
    {
        if (is_Input_Que_Active)
        {
            if (qued_Input_Timer>0)
            {
                qued_Input_Timer -= Time.deltaTime;
                ProcessQuedInput();
            }
            else
            {
                //Reset All Input
                que_RB_Input =false;
                que_LB_Input =false;


                qued_Input_Timer = 0;
            }
        }

    }

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;
            player.isUsingRightHand = true;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_RT_Action, player.playerInventoryManager.currentRightHandWeapon);





        }

    }

    private void HandleLBInput()
    {
        if (PlayerUIManager.instance.menuWindowOpen)
            return;

        if (player.isAiming)
        {
            LB_Input = false;
            return;
        }
            

        //We Are Using An Item Return
        if (player.playerCombatManager.isUsingItem)
            return;

        if (!player.isGrounded)
        {
            player.isAiming = false;
            player.playerCombatManager.isAimLockedOn = false;
            PlayerCamera.instance.OnIsAimingChanged(false);
            return;
        }

        if (LB_Input == true)
        {
            LB_Input = false;
            player.isUsingLeftHand = true;

            player.currentDamageType = player.playerInventoryManager.currentLeftHandWeapon.weaponClass;
            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.OH_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);

        }

    }

    public void HandleCancleLBInPut()
    {

    }

    private void HandleChargeRTInput()
    {
        if (Hold_RT_Input)
        {


            player.isChargingAttack = Hold_RT_Input;





        }
        else
        {
            player.isChargingAttack = Hold_RT_Input;
        }

    }
    private void HandleSwitchRightWeaponInput()
    {
        if (player.isAttacking)
            return;
        if (player.isPerformingAction)
            return;

        if (Switch_Right_Weapon_Input)
        {
            Switch_Right_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

            player.playerEquipmentManager.SwitchRightWeapon();


        }

    }
    private void HandleSwitchLefttWeaponInput()
    {
        if (player.isAttacking)
            return;
        if (player.isPerformingAction)
            return;

        if (Switch_Left_Weapon_Input)
        {
            Switch_Left_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

            player.playerEquipmentManager.SwitchLeftWeapon();

        }


    }

    private void HandleQuickSlotItemInput()
    {
        if (Switch_Quick_Slot_Item_Input)
        {
            Switch_Quick_Slot_Item_Input= false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

            if (player.isPerformingAction)
                return;
            player.playerEquipmentManager.SwitchQuickSlotItem();






        }


    }

    private void HandleInteractionInput()
    {
        if (interaction_Input && !player.playerInteractionManager.isInspectingObject)
        {
            interaction_Input = false;

            player.playerInteractionManager.Interact();
        }
    }

    private void HandleOpenCharacterMenuInput()
    {
        if (openCharacterMenuInput)
        {
            openCharacterMenuInput = false;

            // Check if the menu is already open
            if (PlayerUIManager.instance.menuWindowOpen)
            {
                // Close the menu if it's open
                PlayerUIManager.instance.CloseAllMenu();
                PlayerUIManager.instance.playerUICharacterMenuManager.ToggleSettingsButton(false);





            }
            else
            {
                // Open the menu if it's not open
                PlayerUIManager.instance.playerUICharacterMenuManager.OpenMenu();
                WorldSoundFXManager.instance.PlayMenuOpenSFX();
                PlayerUIManager.instance.playerUICharacterMenuManager.ToggleSettingsButton(true);
            }
        }
    }


    private void HandleHoldRBInput()
    {


      








        if (player.playerInventoryManager.currentTwoHandWeapon == null)
            return;
        
        // Only care about bow & gun
        if (player.playerInventoryManager.currentTwoHandWeapon.weaponClass != WeapomClass.Bow && player.playerInventoryManager.currentTwoHandWeapon.weaponClass != WeapomClass.Gun)
            return;


        //Bow
        if(player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
        {

            if (Hold_RB_Input)
            {
                rbHoldTimer += Time.deltaTime;

                // ENTER AIM MODE
                if (!rbAimTriggered && rbHoldTimer >= rbHoldThreshold)
                {
                    rbAimTriggered = true;

                    player.isAiming = true;
                    player.animator.SetBool("isAiming", true);
                    PlayerCamera.instance.OnIsAimingChanged(true);

                    // Aim action
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_LB_Action, player.playerInventoryManager.currentRightHandWeapon);
                }

                player.isHoldingArrow = true;
                player.animator.SetBool("isHoldingArrow", true);
            }
            else
            {
                // RELEASE FRAME
                if (rbHoldTimer > 0f )
                {
                    if (rbAimTriggered)
                    {
                        // Stop holding arrow
                        player.isHoldingArrow = false;
                        player.animator.SetBool("isHoldingArrow", false);


                    }
                    else
                    {
                        // Tap shot
                        RB_Input = true;

                        player.isHoldingArrow = false;
                        player.animator.SetBool("isHoldingArrow", false);
                    }
                }

                rbHoldTimer = 0f;
                rbAimTriggered = false;
            }


        }


        //Gun
        if (player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun)
        {

            if (Hold_RB_Input && !player.playerCombatManager.isAimLockedOn && !player.isAiming)
            {
                rbHoldTimer += Time.deltaTime;

                // ENTER AIM MODE
                if (!rbAimTriggered && rbHoldTimer >= rbHoldThreshold)
                {
                    rbAimTriggered = true;

                    player.isAiming = true;
                    player.animator.SetBool("isAiming", true);
                    LockOn_Input = true;
                    PlayerCamera.instance.OnIsAimingChanged(true);

                    // Aim action
                    player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_LB_Action, player.playerInventoryManager.currentRightHandWeapon);
                }

            }
            else
            {
                // RELEASE FRAME
                if (rbHoldTimer > 0f)
                {
                    if (rbAimTriggered)
                    {
                        


                    }
                    else
                    {
                        // Tap shot
                        
                    }
                }

                rbHoldTimer = 0f;
                rbAimTriggered = false;
            }

        }




    }

    public void TriggerBowAim()
    {
        if (player.isHoldingArrow)
            return;

        if (!player.playerCombatManager.isAimLockedOn)
            return;


        player.isAiming = true;
        player.animator.SetBool("isAiming", true);
        player.isHoldingArrow = true;
        player.animator.SetBool("isHoldingArrow", true);
        player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_LB_Action, player.playerInventoryManager.currentRightHandWeapon);
    }

    public void ExitBowAim()
    {
        // 1. Exit aim state
        player.isAiming = false;
        player.animator.SetBool("isAiming", false);
        PlayerCamera.instance.OnIsAimingChanged(false);

        // 3. Cancel arrow if it exists
        if (player.hasArrowNotched)
        {
            player.hasArrowNotched = false;

            if (player.playerEquipmentManager.notchedArrow != null)
            {
                Destroy(player.playerEquipmentManager.notchedArrow);
                player.playerEquipmentManager.notchedArrow = null;
            }
        }

        // 4. Safety: stop any bow firing state
        player.playerCombatManager.isFiringBow = false;
    }



}