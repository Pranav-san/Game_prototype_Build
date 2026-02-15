using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering.Universal;
using static CartoonFX.CFXR_Effect;
using static Unity.Cinemachine.AxisState;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using InputTouchPhase = UnityEngine.InputSystem.TouchPhase;



public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera cameraObject;

    public Transform cameraPivotTransform;
    public GameObject ProjectileFireDirection;

    public playerManager player;

    public PlayerLocomotionManager playerLocomotionManager;

    [Header("Debug")]
    [SerializeField] private bool isGamepadConnected;

    [Header("Camera offset")]
    [SerializeField] Vector3 titleScreenOffset;
    public float titleScreenCameraPivotAngle;
    public float titleScreenCameraUpDownAngle;
    public Vector3 resetCameraPosition;

    [Header("Camera Values")]
    [SerializeField] float cameraZPosition;
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    private float targetCameraZPosition;
    public float cameraPivotAngle;
    public float leftAndRightLookAngle;
    public float upAndDownLookAngle;

    [Header("Camera Settings")]
    public float cameraSmoothSpeed = 1;
    public float cameraRollSmoothSpeed = 1;
    public float cameraAimSmoothSpeed = 1;
    public float leftAndRightRotationSpeed = 220;
    public float upAndDownRotationSpeed = 220;
    public float minimumpivot = -30;
    public float maximumpivot = 60;
    public float cameraCollisionRadius = 0.2f;
    public LayerMask collideWithLayers;
    public float CameraFOV = 60;
    public float CameraAimingFOV = 45;

    [Header("Mobile")]
    [SerializeField] private bool isInputActive;
    private int cameraFingerId = -1;
    private Vector2 lastTouchPos;
    [SerializeField] FloatingJoystick joystick;
    [SerializeField] RectTransform joystickRect;
    public float touchSensitivity = 1f;
    [SerializeField] private bool cameraInputEnabled = true;

    [Header("Camera Recentering")]
    [SerializeField] private bool enableRecentering;
    [SerializeField] private float lastCameraInputTime;
    [SerializeField] private bool isRecentering;
    [SerializeField] private float recenterTimer;
    [SerializeField] private float recenterDelay = 2f;
    public float cameraRotateSpeedWhenPlayerIsTurning = 1;
    [SerializeField] float camerPivotAngleRecenterThreshold = 75;




    [Header("Aim")]
    public Transform followTransformWhenAiming;
    [SerializeField] float cameraAimZPosition;
    public float leftAndRightAimRotationSpeed = 220;
    public float upAndDownAimRotationSpeed = 220;
    public float aimedMinimumpivot = -30;
    public float aimedMaximumpivot = 60;
    private bool hasInitializedAim = false;
    [SerializeField] float CameraOffsetLeftRight = 0.76f;
    [SerializeField] float AimCameraHeight = 2f;
    private Coroutine aimCameraHeightCoroutine;
    public Vector3 aimDirection;



    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20f;
    [SerializeField] float minimumViewableAngle = -50f;
    [SerializeField] float maximumViewableAngle = 50f;
    [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] float unlockedCameraHeight = 1.64f;
    [SerializeField] float lockedCameraHeight = 2.0f;
    [SerializeField] float lockedonTiltAngle = 20f;
    [SerializeField] float cameraHeightSpeed = 1f;
    private Coroutine cameraLockOnCoroutine;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager RightLockOnTarget;

    [Header("LockOn Settings")]
    public bool autoLockOn = true;
    public bool autoSwitchToNearestTarget = false;

    [Header("Camera Shake")]
    CameraShake cameraShake;
    public float shakeMagnitude = 0.1f;










    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        EnhancedTouchSupport.Enable();

        TitleScreenCameraOffset();

        cameraShake = GetComponentInChildren<CameraShake>();


    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
 

        joystick = PlayerUIManager.instance.mobileControls.joystick;
        joystickRect =  PlayerUIManager.instance.mobileControls.joystick.joyStickrect;







    }

    private void Update()
    {
        if (PlayerUIManager.instance.isLoadingScreenActive)
            return;

        bool fingerStillActive = false;

        foreach (var touch in ETouch.Touch.activeTouches)
        {
            if (touch.finger.index == cameraFingerId)
            {
                fingerStillActive = true;
                break;
            }
        }

        if (!fingerStillActive)
        {
            cameraFingerId = -1;
            lastTouchPos = Vector2.zero;
        }


        
        HandleAllCameraActions();

        if (player.playerCombatManager.currentTarget!=null)
        {
            PlayerUIManager.instance.UpdateLockOnDotPosition();
        }

    }

    

    public void HandleAllCameraActions()
    {
        if (player != null)
        {


            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();

        }
    }


    private void HandleFollowTarget()
    {

        



        if (player.isAiming)
        {

            
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, followTransformWhenAiming.transform.position, ref cameraVelocity, cameraAimSmoothSpeed);
            transform.position = targetCameraPosition;
            cameraObject.fieldOfView = CameraAimingFOV;
            




        }
        else if (player.playerLocomotionManager.isRolling)
        {
                Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraRollSmoothSpeed);
                transform.position = targetCameraPosition;


            }
            else if (player.isGrappled)
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, followTransformWhenAiming.transform.position, ref cameraVelocity, cameraSmoothSpeed);
            transform.position = targetCameraPosition;

        }
        else
        {

            cameraObject.fieldOfView = CameraFOV;
            


            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed);
            transform.position = targetCameraPosition;


        }

    }

    private void HandleRotation()
    {
        if (!cameraInputEnabled)
            return;

        if (player.isAiming)
        {
            HandndleAimedRotation();

        }

        else
        {
            HandleStandardRotation();

        }


    }


    //Old HandleStandardRotation Logic
    //private void HandleStandardRotation()
    //{
    //    //if (character.playerInteractionManager.isInspectingObject & character.playerInteractionManager.currentInteractable!=null)
    //    //{
    //    //    GameObject parentObject = character.playerInteractionManager.currentInteractable.transform.parent.gameObject;
    //    //    character.playerInteractionManager.RotateObject(parentObject);
    //    //    return;


    //    //}
    //    //IF Locked on Force Rotation towards target 

    //    cameraObject.transform.localRotation = Quaternion.Euler(0, 0, 0);



    //    if (player.playerCombatManager.isLockedOn)
    //    {
    //        Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - transform.position;
    //        rotationDirection.Normalize();
    //        rotationDirection.y = 0;

    //        Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

    //        //Rotates The Pivot  Object
    //        rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - cameraPivotTransform.position;
    //        rotationDirection.Normalize();

    //        targetRotation = Quaternion.LookRotation(rotationDirection);
    //        cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);


    //        float tiltAmount = 13.5f; // degrees of downward camera tilt

    //        Quaternion tiltRotation = Quaternion.Euler(tiltAmount, 0f, 0f);
    //        cameraPivotTransform.localRotation = tiltRotation;

    //        //Save our rotation to our look Angles, so when we unlock it doesnt snap too far away
    //        leftAndRightLookAngle = transform.eulerAngles.y;
    //        upAndDownLookAngle = transform.eulerAngles.x;
    //        return;

    //    }

    //    //other wise Rotate regularly



    //    else
    //    {
    //        bool hasCameraInput = false;


    //        if (isInputActive)
    //        {

    //            recenterTimer = 0f;
    //            isRecentering = false;



    //            leftAndRightLookAngle = (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
    //            upAndDownLookAngle = -(PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

    //            if (isGamepadConnected)
    //            {
    //                leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * 100) * Time.deltaTime;
    //                upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * 100) * Time.deltaTime;

    //            }

    //            if (touchField != null && touchField.SwipeDelta != Vector2.zero)
    //            {
    //                leftAndRightLookAngle = (touchField.SwipeDelta.x * touchSensitivity * leftAndRightRotationSpeed / Screen.width) * Time.deltaTime;
    //                upAndDownLookAngle = -(touchField.SwipeDelta.y * touchSensitivity * upAndDownRotationSpeed / Screen.height) * Time.deltaTime;
    //            }

    //            Vector3 cameraRotation = Vector3.zero;
    //            Quaternion targetRotation;

    //            cameraRotation.y = leftAndRightLookAngle;
    //            targetRotation = Quaternion.Euler(cameraRotation);
    //            transform.rotation *= targetRotation;

    //            cameraPivotAngle += upAndDownLookAngle;
    //            cameraPivotAngle = Mathf.Clamp(cameraPivotAngle, minimumpivot, maximumpivot);

    //            cameraRotation = Vector3.zero;
    //            cameraRotation.x = cameraPivotAngle;
    //            targetRotation = Quaternion.Euler(cameraRotation);
    //            cameraPivotTransform.localRotation = targetRotation;

    //            hasCameraInput = true;
    //        }

    //        else
    //        {
    //            if (enableRecentering)
    //            {

    //                recenterTimer += Time.deltaTime;

    //                if (recenterTimer < recenterDelay) // <<=== delay before starting to recenter
    //                {
    //                    isRecentering = false;
    //                    return;
    //                }



    //                // Check if the character is moving forward or backward
    //                if (PlayerInputManager.Instance.movementInput.magnitude > 0.1f)
    //                {
    //                    // Only lock the camera to the character's direction if moving forward
    //                    if (playerLocomotionManager.horizontalMovement > 0.2f ||playerLocomotionManager.horizontalMovement < -0.2f)
    //                    {
    //                        // Lock the camera to the character's direction
    //                        Vector3 direction = player.transform.forward;
    //                        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);
    //                        isRecentering = true;

    //                    }
    //                    else
    //                    {
    //                        // Allow free look when moving backward or when no input is present
    //                        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);
    //                        isRecentering = false;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                // Allow free look when the character is not moving
    //                transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, cameraSmoothSpeed * Time.deltaTime);

    //                recenterTimer = 0f;
    //                isRecentering = false;
    //            }




    //        }
    //    }

    //}

    private void HandleStandardRotation()
    {
        //if (character.playerInteractionManager.isInspectingObject & character.playerInteractionManager.currentInteractable!=null)
        //{
        //    GameObject parentObject = character.playerInteractionManager.currentInteractable.transform.parent.gameObject;
        //    character.playerInteractionManager.RotateObject(parentObject);
        //    return;
        //}
        //IF Locked on Force Rotation towards target 

        cameraObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        bool hasTouchInput =false;
        if (player.playerCombatManager.isLockedOn)
        {
            Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - transform.position;
            rotationDirection.Normalize();
            rotationDirection.y = 0;

            Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            //Rotates The Pivot  Object
            rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.LockOnTransform.position - cameraPivotTransform.position;
            rotationDirection.Normalize();

            targetRotation = Quaternion.LookRotation(rotationDirection);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

            //Set Camera TileAngle When LockedOn
            Quaternion tiltRotation = Quaternion.Euler(lockedonTiltAngle, 0f, 0f);
            cameraPivotTransform.localRotation = tiltRotation;

            //Save our rotation to our look Angles, so when we unlock it doesnt snap too far away
            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = transform.eulerAngles.x;
            return;
        }

        // Otherwise rotate regularly
        else
        {
            bool hasCameraInput = PlayerInputManager.Instance.cameraHorizontalInput != 0 || PlayerInputManager.Instance.cameraVerticalInput != 0;

            foreach (var touch in ETouch.Touch.activeTouches)
            {
                // Ignore joystick touches
                if (IsTouchOverJoystick(touch.startScreenPosition))
                    continue;

                // Only treat real drags as camera input
                if (touch.phase == InputTouchPhase.Moved &&
                    touch.delta.sqrMagnitude > 2f) // small noise filter
                {
                    hasTouchInput = true;
                    break;
                }
            }

            if (hasTouchInput || hasCameraInput)
            {
                //Disable Recenter
                //Camera Is Fully Under Player Control
                recenterTimer = 0f;
                isRecentering = false;

                //Gather camerahorizontal and vertical inputs
                leftAndRightLookAngle = (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                upAndDownLookAngle = -(PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

                if (isGamepadConnected)
                {
                    leftAndRightLookAngle += (PlayerInputManager.Instance.cameraHorizontalInput * 100) * Time.deltaTime;
                    upAndDownLookAngle -= (PlayerInputManager.Instance.cameraVerticalInput * 100) * Time.deltaTime;
                }


                //Gather touch/swipe inputs
                foreach (var touch in ETouch.Touch.activeTouches)
                {
                    //Ignore touches that are moving the joystick
                    if (IsTouchOverJoystick(touch.startScreenPosition))
                        continue;

                    if (touch.phase != InputTouchPhase.Moved)
                        continue;

                    Vector2 delta = touch.delta;

                    leftAndRightLookAngle += (delta.x * touchSensitivity * leftAndRightRotationSpeed / Screen.width) * Time.deltaTime;

                    upAndDownLookAngle += -(delta.y * touchSensitivity * upAndDownRotationSpeed / Screen.height) * Time.deltaTime;

                    break;
                }


                //Rotate the camera

                Vector3 cameraRotation = Vector3.zero;
                Quaternion targetRotation;

                cameraRotation.y = leftAndRightLookAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                transform.rotation *= targetRotation;

                cameraPivotAngle += upAndDownLookAngle;
                cameraPivotAngle = Mathf.Clamp(cameraPivotAngle, minimumpivot, maximumpivot);

                cameraRotation = Vector3.zero;
                cameraRotation.x = cameraPivotAngle;
                targetRotation = Quaternion.Euler(cameraRotation);
                cameraPivotTransform.localRotation = targetRotation;

                hasCameraInput = true;
            }
            else
            {
                // Auto-align/recenter behavior when there is no Camera Input
                if (enableRecentering)
                {
                    //Check if the camera is misaligned
                    if(cameraPivotAngle >= camerPivotAngleRecenterThreshold)
                        return;

                    // Check if player is moving before we try to recenter
                    bool isPlayerMoving = PlayerInputManager.Instance.movementInput.magnitude > 0.1f;

                    if (!isPlayerMoving)
                    {
                        // Player is idle don't recenter at all
                        recenterTimer = 0f;
                        isRecentering = false;
                        return;
                    }

                    Vector3 characterForward = player.transform.forward;
                    characterForward.y = 0f; // Keep horizontal plane

                    //Recenter camera when there is horizontal input 
                    if (PlayerInputManager.Instance.horizontalInput > 0.2f || PlayerInputManager.Instance.horizontalInput < -0.2f)
                    { 
                        
                        if (characterForward != Vector3.zero)
                        {
                            Vector3 cameraForward = transform.forward;
                            cameraForward.y = 0f;

                            if (cameraForward != Vector3.zero)
                            {
                                float angleDifference = Vector3.SignedAngle(cameraForward, characterForward, Vector3.up);
                                recenterTimer += Time.deltaTime;

                                // Start recentering only after delay and if camera is sufficiently misaligned
                                if (recenterTimer >= recenterDelay && Mathf.Abs(angleDifference) > 5f)
                                {
                                    isRecentering = true;

                                    //Calculate next horizontal camera rotation value
                                    // Moves camera Y-axis rotation (yaw) toward player facing direction
                                    //MoveTowards(): slowly reaches the target over multiple frames
                                    float targetYaw = Mathf.MoveTowardsAngle(transform.eulerAngles.y, player.transform.eulerAngles.y,cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);

                                    //Apply horizontal camera rotation
                                    transform.rotation = Quaternion.Euler(0f, targetYaw, 0f);

                                    //Calculate next vertical camera rotation value
                                    // Moves camera X-axis rotation (pitch) toward neutral (0)
                                    float targetPitch = Mathf.MoveTowards(cameraPivotAngle, 0f, cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);
                                    //Apply vertical camera rotation
                                    cameraPivotAngle = targetPitch;
                                    cameraPivotTransform.localRotation = Quaternion.Euler(cameraPivotAngle, 0f, 0f);

                                    //Sync the values with the actual camera rotation
                                    leftAndRightLookAngle = targetYaw;
                                    upAndDownLookAngle = targetPitch;
                                }
                            }
                            else
                            {
                                isRecentering = false;
                            }
                        }
                    }
                }
                else
                {
                    // Maintain current rotation when recentering is disabled
                    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, cameraSmoothSpeed * Time.deltaTime);
                    recenterTimer = 0f;
                    isRecentering = false;
                }
            }
        }
    }



    private void HandndleAimedRotation()
    {

        
        if (!player.isGrounded)
            player.isAiming = false;

        if (player.isPerformingAction)
            return;


        cameraPivotTransform.localRotation = Quaternion.Euler(0, 0, 0);

        aimDirection = cameraObject.transform.forward.normalized;
        bool hasTouchInput = false;


        bool hasCameraInput = PlayerInputManager.Instance.cameraHorizontalInput != 0 || PlayerInputManager.Instance.cameraVerticalInput != 0;

        foreach (var touch in ETouch.Touch.activeTouches)
        {
            // Ignore joystick touches
            if (IsTouchOverJoystick(touch.startScreenPosition))
                continue;

            // Only treat real drags as camera input
            if (touch.phase == InputTouchPhase.Moved &&
                touch.delta.sqrMagnitude > 2f) // small noise filter
            {
                hasTouchInput = true;
                break;
            }
        }

        if (hasCameraInput || hasTouchInput)
        {
            //Gather Inputs
            float horizontal = PlayerInputManager.Instance.cameraHorizontalInput* upAndDownAimRotationSpeed * Time.deltaTime;
            float vertical = PlayerInputManager.Instance.cameraVerticalInput*leftAndRightAimRotationSpeed * Time.deltaTime;



            foreach (var touch in ETouch.Touch.activeTouches)
            {
                // Ignore touches that are moving the joystick
                if (IsTouchOverJoystick(touch.startScreenPosition))
                    continue;

                if (touch.phase != InputTouchPhase.Moved)
                    continue;

                Vector2 delta = touch.delta;

                leftAndRightLookAngle += (delta.x * touchSensitivity * leftAndRightRotationSpeed / Screen.width) * Time.deltaTime;

                upAndDownLookAngle += -(delta.y * touchSensitivity * upAndDownRotationSpeed / Screen.height) * Time.deltaTime;

                break;
            }




            //upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, aimedMinimumpivot, aimedMaximumpivot);

            leftAndRightLookAngle += horizontal;
            upAndDownLookAngle += vertical;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, aimedMinimumpivot, aimedMaximumpivot);
            // Rotate Cam



        }
        transform.rotation = Quaternion.Euler(0, leftAndRightLookAngle, 0);
        //cameraObject.transform.localEulerAngles= new Vector3(upAndDownLookAngle, 0f, 0f);
        cameraPivotTransform.transform.localEulerAngles = new Vector3(upAndDownLookAngle, 0, 0);

    }


    bool IsTouchOverJoystick(Vector2 screenPos)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(
            joystickRect,
            screenPos,
            null   // null = screen space overlay
        );
    }


    public void OnIsAimingChanged(bool isAiming)
    {
        //Reset Local EulerAngles Of the Camera Object When Not Aiming
        Vector3 velocity = Vector3.zero;
        Vector3 aimcamHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, AimCameraHeight);
        Vector3 unlockedAimcamHeight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);
        if (!isAiming ||player.isExitingToEmptyAfterReload)
        {
            hasInitializedAim = false;
            cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            if (player.playerInventoryManager.currentTwoHandWeapon != null && player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun)
            {
                player.playerAnimatorManager.EnableDisableIK(0, 0);
                player.playerAnimatorManager.UpdateAimedConstraints();
                aimCameraHeightCoroutine = StartCoroutine(SetAimingCameraHeight(false));
                PlayerUIManager.instance.playerUIHUDManager.ToggleCrossHair(false);


            }

            else if (player.playerInventoryManager.currentTwoHandWeapon != null && player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
            {
                aimCameraHeightCoroutine = StartCoroutine(SetAimingCameraHeight(false));
                player.playerAnimatorManager.EnableDisableIK(0, 0);
                player.playerAnimatorManager.UpdateAimedConstraintsForBow();
                PlayerUIManager.instance.playerUIHUDManager.ToggleCrossHair(false);

            }
            player.isExitingToEmptyAfterReload = false;


        }
        //Reset Local EulerAngles of CameraPivot And Main Parent Object
        else
        {

            
            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = 7f;
            hasInitializedAim = true;
            if(player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Gun)
            {
                player.playerAnimatorManager.EnableDisableIK(1, 1);
                player.playerAnimatorManager.UpdateAimedConstraints();
                aimCameraHeightCoroutine = StartCoroutine(SetAimingCameraHeight(true));
                PlayerUIManager.instance.playerUIHUDManager.ToggleCrossHair(true);
                cameraObject.transform.localPosition = new Vector3(0f, 0f, cameraAimZPosition);
                Debug.Log("Setting aim height");



            }
            else if (player.playerInventoryManager.currentTwoHandWeapon.weaponClass == WeapomClass.Bow)
            {
                aimCameraHeightCoroutine = StartCoroutine(SetAimingCameraHeight(true));
                player.playerAnimatorManager.EnableDisableIK(0, 0);
                player.playerAnimatorManager.UpdateAimedConstraintsForBow();
                PlayerUIManager.instance.playerUIHUDManager.ToggleCrossHair(true);
                cameraObject.transform.localPosition = new Vector3(0f, 0f, cameraAimZPosition);



            }


                Debug.Log("Reset CamPivot");

        }
        player.animator.SetBool("isAiming", player.isAiming);
    }




    private IEnumerator SetAimingCameraHeight(bool aiming)
    {
        float duration = 0.25f;  // how smooth the transition is
        float timer = 0f;

        Vector3 velocity = Vector3.zero;

        // Current pivot position
        Vector3 startPos = cameraPivotTransform.localPosition;

        // Target height when aiming
        Vector3 aimedHeight = new Vector3(
            startPos.x,
            AimCameraHeight,
            startPos.z
        );

        // Target height when not aiming
        Vector3 unAimedHeight = new Vector3(
            startPos.x,
            unlockedCameraHeight,
            startPos.z
        );

        Vector3 targetPos = aiming ? aimedHeight : unAimedHeight;

        // Smooth transition
        while (timer < duration)
        {
            timer += Time.deltaTime;

            cameraPivotTransform.localPosition =
                Vector3.SmoothDamp(cameraPivotTransform.localPosition, targetPos, ref velocity, cameraHeightSpeed);

            yield return null;
        }

        // Ensure exact value on finish
        cameraPivotTransform.localPosition = targetPos;
    }




    private void HandleCollisions()
    {

        if (player.isAiming)
        {
            targetCameraZPosition = cameraAimZPosition;

        }
        else
        {
            targetCameraZPosition = cameraZPosition;
        }    
        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            float collisionOffset = cameraCollisionRadius;
            targetCameraZPosition = -(distanceFromHitObject - collisionOffset);
        }
        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }
        cameraObjectPosition = cameraObject.transform.localPosition;
        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

    public void HandleLocatingLockOnTarget()
    {
        nearestLockOnTarget = null;
        leftLockOnTarget   = null;
        RightLockOnTarget  = null;
        availableTargets.Clear();

        float shortestDistance = Mathf.Infinity;  //Used to determine the target closest to Player

        float shortestDistanceOfRightTarget = Mathf.Infinity;// Closest Target to the Right Of Current Target(+)
        float shortestDistanceOfLeftTarget = -Mathf.Infinity;//Will be used to determine Shortest distance on one Axis To the Left Of Current Target (Closest Target to the Left Of Current Target(-))

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayer());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if (lockOnTarget.isFriendly)
                return;

            if (lockOnTarget != null)
            {

                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float distancefromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                //If Target is dead, Check for next potential Target
                if (lockOnTarget.characterStatsManager.isDead)
                {
                    continue;
                }
                // if Target is us, Check for next potential Target 
                if (lockOnTarget.transform.root==player.transform.root)
                {

                    continue;
                }

                //If the target is outside of field of view or blocked by enviro, check next potential Target
                if (viewableAngle > minimumViewableAngle && viewableAngle <maximumViewableAngle)
                {
                    RaycastHit hit;

                    if (Physics.Linecast(player.playerCombatManager.LockOnTransform.position, lockOnTarget.characterCombatManager.LockOnTransform.position
                        , out hit, WorldUtilityManager.Instance.GetEnviroLayer()))
                    {
                        continue;
                    }
                    else
                    {
                        //Otherwise Add them to Potential target list 
                        availableTargets.Add(lockOnTarget);

                        //Debug.Log("We made it");


                    }

                }


            }
        }
        //Sort through potential targets to see which one to lock onto first
        for (int k = 0; k < availableTargets.Count; k++)
        {
            if (availableTargets[k] != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);


                if (distanceFromTarget<shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];


                }

                if (player.playerCombatManager.isLockedOn)
                {
                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (availableTargets[k] == player.playerCombatManager.currentTarget)
                        continue;

                    //Check Left Side For Targets 
                    if (relativeEnemyPosition.x <=0.00 &&  distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];

                    }
                    //Check Right side For targets 
                    else if (relativeEnemyPosition.x >= 00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                    {
                        shortestDistanceOfRightTarget -= distanceFromRightTarget;
                        RightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else
            {
                ClearLockOnTargets();
                player.playerCombatManager.isLockedOn = false;
                
            }

        }
    }


    public void ClearLockOnTargets()
    {



        nearestLockOnTarget = null;
        leftLockOnTarget = null;
        RightLockOnTarget = null;





        availableTargets.Clear();
    }

    public void SetLockCameraHeight()
    {
        if(player.playerStatsManager.isDead) 
            return;

        if (cameraLockOnCoroutine != null)
        {
            StopCoroutine(cameraLockOnCoroutine);
        }
        cameraLockOnCoroutine = StartCoroutine(SetCameraHeight());

    }
    private IEnumerator SetCameraHeight()
    {
        float duration = 1;
        float timer = 0;

        Vector3 velocity = Vector3.zero;
        Vector3 newLockedCameraheight = new Vector3(cameraPivotTransform.transform.localPosition.x, lockedCameraHeight);
        Vector3 newUnLockedCameraheight = new Vector3(cameraPivotTransform.transform.localPosition.x, unlockedCameraHeight);

        while (timer < duration)
        {
            timer+=Time.deltaTime;

            if (player!=null)
            {
                if (player.playerCombatManager.currentTarget!=null)
                {
                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newLockedCameraheight, ref velocity, cameraHeightSpeed);
                    //cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);

                }
                else
                {

                    cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnLockedCameraheight, ref velocity, cameraHeightSpeed);


                }
            }
            yield return null;
        }
        if (player !=null)
        {

            if (player.playerCombatManager.currentTarget!=null)
            {
                cameraPivotTransform.transform.localPosition = newLockedCameraheight;
                cameraPivotTransform.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (!player.playerCombatManager.isLockedOn && player.playerCombatManager.currentTarget!=null)
            {
                cameraPivotTransform.transform.localPosition = newUnLockedCameraheight;

            }
            else if (!player.playerCombatManager.isLockedOn)
            {
                cameraPivotTransform.transform.localPosition = newUnLockedCameraheight;

            }
            else
            {

                cameraPivotTransform.transform.localPosition = newUnLockedCameraheight;
            }

        }
        yield return null;


    }
    public IEnumerator WaitThenfindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }
        ClearLockOnTargets();
        HandleLocatingLockOnTarget();

        if (nearestLockOnTarget != null)
        {
            player.playerCombatManager.SetTarget(nearestLockOnTarget);
            PlayerUIManager.instance.SetLockOnTarget(nearestLockOnTarget.characterCombatManager.LockOnTransform);
            player.playerCombatManager.isLockedOn = true;
        }
        yield return null;
    }


    public void TitleScreenCameraOffset()
    {
        cameraObject.transform.localPosition = titleScreenOffset;

        Quaternion titleScreenCameraPivotangle = Quaternion.Euler(titleScreenCameraPivotAngle, 0f, 0f);
        cameraPivotTransform.localRotation = titleScreenCameraPivotangle;


        Quaternion CameraUpDownAngle = Quaternion.Euler(0, titleScreenCameraUpDownAngle, 0f);

        transform.rotation = CameraUpDownAngle;

        DisableCameraInput();

    }

    public void ResetCamera()
    {
        transform.rotation = Quaternion.identity;
        cameraPivotTransform.rotation = Quaternion.identity;
        cameraObject.transform.localPosition = new Vector3(0f, 0f, cameraZPosition);
        leftAndRightLookAngle = 0;
        upAndDownLookAngle = 0;
        cameraPivotAngle = 10;

    }

    public void DisableCameraInput()
    {
        cameraInputEnabled = false;
        EnhancedTouchSupport.Disable();

        cameraFingerId = -1;
        lastTouchPos = Vector2.zero;
    }

    public void EnableCameraInput()
    {
        cameraInputEnabled = true;
        EnhancedTouchSupport.Enable();
    }

    public void shakeCamera()
    {
        StartCoroutine(cameraShake.shake(shakeMagnitude, shakeMagnitude));

    }






}









