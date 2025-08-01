using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    public playerManager player;
    PlayerControls2 playerControls;
    PlayerUIHUDManager playerUIHUDManager;

    [Header("Camera Movement Input")]
    public float cameraVerticalInput;
    public float cameraHorizontalInput;
    [SerializeField] Vector2 cameraInput;

    

    [Header("Lock On")]
    [SerializeField] bool LockOn_Input;
    [SerializeField] bool LockOn_Left_Input;
    [SerializeField] bool LockOn_Right_Input;
    private Coroutine lockOnCoroutine;


    [Header("Player Movement Input")]
    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Player Actions")]
    [SerializeField] bool dodgeInput = false;
    [SerializeField] bool sprintInput = false;
    [SerializeField] bool jumpInput = false;
   
    [SerializeField] bool Switch_Right_Weapon_Input = false;
    [SerializeField] bool Switch_Left_Weapon_Input = false;

    [Header("Bumper Input")]
    [SerializeField] bool RB_Input = false;
    [SerializeField] bool Hold_RB_Input = false;
    [SerializeField] bool LB_Input = false;
    [SerializeField] bool Hold_LB_Input = false;

    [Header("Two Hand Input")]
    [SerializeField] public bool Two_Hand_Input = false;
   

    [Header("Trigger Input")]
    [SerializeField] bool RT_Input = false;
    [SerializeField] bool Hold_RT_Input = false;


    [SerializeField] bool interaction_Input = false;
    [SerializeField] bool cameraSwitch_Input = false;

    [Header("UI Inputs")]
    [SerializeField] bool openCharacterMenuInput = false;
    [SerializeField] bool closeMenuInput = false;
    [SerializeField] bool openSurvivalWheelInput = false;


    [Header("Qued Inputs")]
    private bool is_Input_Que_Active = false;
    [SerializeField] float default_Que_Input_Time = 0.35f;
    [SerializeField] float qued_Input_Timer = 0;
    [SerializeField] bool que_RB_Input = false;










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
            
            playerControls.PlayerActions.InteractionKey.performed += i => interaction_Input = true;

            playerControls.PlayerActions.TwoHandWeapon.performed += i => Two_Hand_Input = true;

          

            //Bumpers
            playerControls.PlayerActions.RB.performed += i => RB_Input = true;
            playerControls.PlayerActions.LB.performed += i => LB_Input = true;
            playerControls.PlayerActions.LB.canceled += i => player.isBlocking = false;
            
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

            //Camera Switch
            playerControls.PlayerActions.CameraSwitch.performed += i => cameraSwitch_Input = true;

            //UI Inputs
            playerControls.PlayerActions.OpenCharacterMenu.performed += i => openCharacterMenuInput = true;
            //playerControls.PlayerActions.OpenSurvivalWheel.performed += i => openSurvivalWheelInput = true;
            playerControls.PlayerActions.OpenSurvivalWheel.performed += i => openSurvivalWheelInput = true;
            playerControls.PlayerActions.OpenSurvivalWheel.canceled += i => openSurvivalWheelInput = false;

            //Qued Inputs
            playerControls.PlayerActions.QueRB.performed += i => QueInput(ref que_RB_Input);



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

        HandleAllQuedInput();

        HandlePlayerMovementInput();


        HandleCameraMovementInput();
        HandleDodgeInput();
        HandleJumpInput();
        HandleSprinting();
        
        HandleRBInput();
        HandleLBInput();

        HandleTwoHandInput();

        HandleHoldRBInput();

        HandleSwitchRightWeaponInput();
        HandleSwitchLefttWeaponInput();
        HandleLockOnInput();
        HandleLockOnSwitchTargetInput();
        HandleInteractionInput();
        HandleOpenCharacterMenuInput();
        HandleOpenSurvivalWheelInput();
        HandleCameraSwitch();
        HandleRTInput();
        HandleChargeRTInput();

    }

    private void HandleAllQuedInput()
    {
        if(is_Input_Que_Active)
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


                qued_Input_Timer = 0;
            }
        }

    }

    private void ProcessQuedInput()
    {

        if(player.playerStatsManager.isDead) 
            return;


        if (que_RB_Input)
        {
            RB_Input = true;

        }
    }

    private void HandleLockOnInput()
    {
        if (!PlayerCamera.instance.FPCameraSwitch)
        {

            if (player.playerCombatManager.isLockedOn)
            {
                if (player.playerCombatManager.currentTarget==null)
                {
                    return;

                }
                if (player.characterStatsManager.isDead)
                {
                    player.playerCombatManager.isLockedOn = false;

                }

                //Attempt to Find new Target 

                //This Ensures that Couroutine never runs multiple times overlapping itself
                if (lockOnCoroutine !=null)
                    StopCoroutine(lockOnCoroutine);

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenfindNewTarget());



            }
        }

        if(LockOn_Input && player.playerCombatManager.isLockedOn)
        {

            LockOn_Input = false;//Disable Lock On
            PlayerCamera.instance.ClearLockOnTargets();
            player.playerCombatManager.isLockedOn = false;
            player.playerCombatManager.currentTarget = null; // Clear current target
            return;
        }

        if (LockOn_Input && !player.playerCombatManager.isLockedOn)
        {
            LockOn_Input = false;//Enable Lock On

            PlayerCamera.instance.HandleLocatingLockOnTarget();

            if (PlayerCamera.instance.nearestLockOnTarget!=null)
            {
                player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                player.playerCombatManager.isLockedOn = true;
               
            }
            return;
            
        }
    }

    private void HandleTwoHandInput()
    {
        if (Two_Hand_Input == true)
        {
            Two_Hand_Input = false;



            if (player.playerEquipmentManager.isTwoHandingWeapon)
            {
                //IF WE ARE TWO HANDING A WEAPON ALREADY, CHANGE THE IS TWOHANDING WEAPON BOOL FALSE WHICH TRIGGERS AN "ONVALUECHANGED" FUNCTION, WHICH UNTWO HANDS CURRENT WEAPON
                player.playerEquipmentManager.isTwoHandingWeapon = false;
                player.playerEquipmentManager.UnTwoHandWeapon();
                player.playerInventoryManager.currentTwoHandWeapon = null;
                return;
            }
            else
            {
                //IF WE ARE NOT ALREADY TWO HANDING A WEAPON, CHANGE THE ISTWOHANDING WEAPON BOOL TO TRUE, WHICH TRIGGERS ONVALUE CHANGE FUNCTION
                //THIS FUNCTION TWO HANDS RIGHT WEAPON
                player.playerEquipmentManager.isTwoHandingWeapon = true;
                player.playerEquipmentManager.OnIsTwoHandingRightWeapon(false,true);
               
            }



        }
    }

    private void HandleJumpInput()
    {

        if (jumpInput)
        {
            jumpInput = false;

            player.playerLocomotionManager.Atte
        }

    }

    private void HandleLockOnSwitchTargetInput()
    {
        if(LockOn_Left_Input)
        {
            LockOn_Left_Input = false;

            if(player.playerCombatManager.isLockedOn)
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
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput)+ Mathf.Abs(horizontalInput));

        if(!player.canMove)
            return;

        
        if (moveAmount<=0.5 && moveAmount > 0)
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

        if (!PlayerCamera.instance.FPCameraSwitch)
        {

            if (!player.playerCombatManager.isLockedOn || player.isSprinting)
            {
                if (PlayerCamera.instance.FPCameraSwitch == false)
                {
                    player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);


                }


            }
            else
            {

                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting);

            }
        }

        
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;


    }
    private void HandleDodgeInput()
    {
        if(dodgeInput == true)
        {
            dodgeInput = false;

            player.playerLocomotionManager.AttemptToPerformDodge();
        }

    }
    private void HandleSprinting()
    {
        // Immediately stop sprinting if stamina is 0
        if (player.characterStatsManager.currentStamina <= 0)
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
        if(jumpInput)
        {
            jumpInput= false;


           
            player.playerLocomotionManager.AttemptToPerformJump();

        }

    }
    public void ForceStateUpdate()
    {
        // This method forces a reevaluation of the player's sprinting state
        // Call this method at appropriate times, e.g., after stamina changes or at the end of each frame/update cycle
        HandleSprinting();


    }

    private void HandleRBInput()
    {
        if(RB_Input == true)
        {
            RB_Input = false;
            player.isUsingRightHand = true;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_RB_Action,player.playerInventoryManager.currentRightHandWeapon);

            



        }

    }

    private void QueInput(ref bool  quedInput)
    {
        que_RB_Input = false;


        if(player.isPerformingAction)
        {
            quedInput = true;
            qued_Input_Timer = default_Que_Input_Time;
            is_Input_Que_Active = true;
            

        }

    }

    private void HandleRTInput()
    {
        if (RT_Input)
        {
            RT_Input = false;
            player.isUsingRightHand = true;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.OH_RT_Action, player.playerInventoryManager.currentRightHandWeapon);

            player.isUsingRightHand = false;



        }

    }

    private void HandleLBInput()
    {
        if (LB_Input == true)
        {
            LB_Input = false;
            player.isUsingLeftHand = true;

            player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentLeftHandWeapon.OH_LB_Action, player.playerInventoryManager.currentLeftHandWeapon);

            player.isUsingLeftHand = false;



        }

    }

    private void HandleChargeRTInput()
    {
        if(Hold_RT_Input)
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
        if (Switch_Right_Weapon_Input)
        {
            Switch_Right_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

            if (player.playerEquipmentManager.isTwoHandingWeapon)
            {
                player.playerEquipmentManager.isTwoHandingWeapon = false;
                player.playerEquipmentManager.UnTwoHandWeapon();
                
                if (player.playerInventoryManager.currentRightHandWeapon.weaponClass == WeapomClass.Bow)
                {
                    player.playerInventoryManager.currentTwoHandWeapon = null;
                    
                    player.playerEquipmentManager.rightHandSlot.UnLoadWeapon();
                    player.playerEquipmentManager.SwitchRightWeapon();

                }
                else
                {
                    player.playerEquipmentManager.SwitchRightWeapon();
                }
                
            }
            
            player.playerEquipmentManager.SwitchRightWeapon();


           
        }


    }
    private void HandleSwitchLefttWeaponInput()
    {
        if (Switch_Left_Weapon_Input)
        {
            Switch_Left_Weapon_Input = false;

            if (PlayerUIManager.instance.menuWindowOpen)
                return;

             if (player.playerEquipmentManager.isTwoHandingWeapon)
            {
                player.playerEquipmentManager.isTwoHandingWeapon = false;
                player.playerEquipmentManager.UnTwoHandWeapon();
                
                if (player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
                {
                    player.playerInventoryManager.currentTwoHandWeapon = null;
                    
                    player.playerEquipmentManager.rightHandSlot.UnLoadWeapon();
                    player.playerEquipmentManager.SwitchRightWeapon();

                }
                else
                {
                    player.playerEquipmentManager.SwitchRightWeapon();
                }
                
            }



            player.playerEquipmentManager.SwitchLeftWeapon();
            
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




        }
         else
          {
            // Open the menu if it's not open
            PlayerUIManager.instance.playerUICharacterMenuManager.OpenCharacterMenu();
          }
        }
    }

    private void HandleOpenSurvivalWheelInput()
    {
        if (openSurvivalWheelInput)
        {
            openSurvivalWheelInput = true;

            PlayerUIManager.instance.playerUICharacterMenuManager.OpenSurvivalWheel();

        }
        else
        {
            openSurvivalWheelInput= false;
            PlayerUIManager.instance.playerUICharacterMenuManager.CloseSurvivalWheel();
        }
           

        }
    

    private void HandleCameraSwitch()
    {
        if (cameraSwitch_Input)
        {
            cameraSwitch_Input = false;

            if (!PlayerCamera.instance.FPCameraSwitch)
            {
                PlayerCamera.instance.FPCameraSwitch =true;

            }
            else
            {
                PlayerCamera.instance.FPCameraSwitch =false;

            }
           
        }
        else
        {


        }
    }

    private void HandleHoldRBInput()
    {
        if (Hold_RB_Input)
        {
            player.playerInventoryManager.isHoldingArrow = true;
            player.animator.SetBool("isHoldingArrow", player.playerInventoryManager.isHoldingArrow);
        }
        else
        {
            player.playerInventoryManager.isHoldingArrow=false;
            player.animator.SetBool("isHoldingArrow", player.playerInventoryManager.isHoldingArrow);
        }

    }
}