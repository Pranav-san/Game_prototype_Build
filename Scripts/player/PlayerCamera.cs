using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.GraphicsBuffer;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;

    public Camera cameraObject;

    public Transform cameraPivotTransform;
    public GameObject ProjectileFireDirection;

    public playerManager player;

    public PlayerLocomotionManager playerLocomotionManager;

    [Header("Camera Settings")]
    public float cameraSmoothSpeed = 1;
    public float cameraRotateSpeedWhenPlayerIsTurning = 1;
    public float leftAndRightRotationSpeed = 220;
    public float upAndDownRotationSpeed = 220;
    public float minimumpivot = -30;
    public float maximumpivot = 60;
    public float cameraCollisionRadius = 0.2f;
    public LayerMask collideWithLayers;
    public float CameraFOV = 60;

    [Header("Aim")]
    public Transform followTransformWhenAiming;
    public float leftAndRightAimRotationSpeed = 220;
    public float upAndDownAimRotationSpeed = 220;
    public float aimedMinimumpivot = -30;
    public float aimedMaximumpivot = 60;
    private bool hasInitializedAim = false;
    [SerializeField] float CameraOffsetLeftRight = 0.76f;


    public Vector3 aimDirection;

    [Header("Focus")]
    public bool isInFocusMode = false;
    [SerializeField] private float inspectRayDistance =7f;
    [SerializeField] private LayerMask inspectLayerMask;
    [SerializeField] private Transform currentInspectRoot;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float cameraZPosition;
    private float targetCameraZPosition;
    public float cameraPivotAngle;
    public float leftAndRightLookAngle;
    public float upAndDownLookAngle;

    [Header("Mobile")]
    public TouchField touchField;
    public TouchField inspectObjectTouchField;
    public float touchSensitivity = 1f;
    [SerializeField] private bool isInputActive;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20f;
    [SerializeField] float minimumViewableAngle = -50f;
    [SerializeField] float maximumViewableAngle = 50f;
    [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
    [SerializeField] float unlockedCameraHeight = 1.64f;
    [SerializeField] float lockedCameraHeight = 2.0f;
    [SerializeField] float cameraHeightSpeed = 1f;
    private Coroutine cameraLockOnCoroutine;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager RightLockOnTarget;


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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        cameraZPosition = cameraObject.transform.localPosition.z;
        //touchField = FindObjectOfType<TouchField>();
        inspectObjectTouchField = PlayerUIManager.instance.inspectObjectTouchField.GetComponent<TouchField>();



    }

    private void Update()
    {
        CheckInputActivity();
        HandleAllCameraActions();

        if (player.playerCombatManager.currentTarget!=null)
        {
            PlayerUIManager.instance.UpdateLockOnDotPosition();
        }

        if (isInFocusMode)
        {
            //  Mouse input (Editor/PC)
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                InspectObjectBasedOn(Mouse.current.position.ReadValue());
            }

            //  Touch input (Mobile)
            foreach (var touch in ETouch.Touch.activeTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    InspectObjectBasedOn(touch.screenPosition);
                }
            }
        }

    }

    private void CheckInputActivity()
    {
        isInputActive = PlayerInputManager.Instance.cameraHorizontalInput != 0 || PlayerInputManager.Instance.cameraVerticalInput != 0;

        if (touchField != null && !player.playerInteractionManager.isInspectingObject)
        {
            isInputActive = isInputActive || touchField.SwipeDelta != Vector2.zero;
        }
        if (player.playerInteractionManager.isInspectingObject)
        {
            inspectObjectTouchField.enabled = true;
            touchField.enabled = false;

            isInputActive = isInputActive || inspectObjectTouchField.SwipeDelta != Vector2.zero;
        }
        else
        {
            inspectObjectTouchField.enabled=false;
            touchField.enabled = true;
        }
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {

            if (isInFocusMode)
                return;




            HandleFollowTarget();
            HandleRotation();
            HandleCollisions();






        }
    }


    private void HandleFollowTarget()
    {



        if (player.isAiming)
        {

            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, followTransformWhenAiming.transform.position, ref cameraVelocity, cameraSmoothSpeed);
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
        if (player.isAiming)
        {
            HandndleAimedRotation();

        }

        else
        {
            HandleStandardRotation();

        }


    }

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

            //Save our rotation to our look Angles, so when we unlock it doesnt snap too far away
            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = transform.eulerAngles.x;

        }

        //other wise Rotate regularly 
        else
        {


            if (isInputActive)
            {
                leftAndRightLookAngle = (PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed) * Time.deltaTime;
                upAndDownLookAngle = -(PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed) * Time.deltaTime;

                if (touchField != null && touchField.SwipeDelta != Vector2.zero)
                {
                    leftAndRightLookAngle = (touchField.SwipeDelta.x * touchSensitivity * leftAndRightRotationSpeed / Screen.width) * Time.deltaTime;
                    upAndDownLookAngle = -(touchField.SwipeDelta.y * touchSensitivity * upAndDownRotationSpeed / Screen.height) * Time.deltaTime;
                }

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
            }
            else
            {
                // Check if the character is moving forward or backward
                if (PlayerInputManager.Instance.movementInput.magnitude > 0.1f)
                {
                    // Only lock the camera to the character's direction if moving forward
                    if (playerLocomotionManager.verticalMovement > 0)
                    {
                        // Lock the camera to the character's direction
                        Vector3 direction = player.transform.forward;
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);
                    }
                    else
                    {
                        // Allow free look when moving backward or when no input is present
                        transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, cameraRotateSpeedWhenPlayerIsTurning * Time.deltaTime);
                    }
                }
                else
                {
                    // Allow free look when the character is not moving
                    transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation, cameraSmoothSpeed * Time.deltaTime);
                }
            }
        }

    }

    private void HandndleAimedRotation()
    {

        cameraPivotTransform.localRotation = Quaternion.Euler(0, 0, 0);
        if (!player.isGrounded)
            player.isAiming = false;

        if (player.isPerformingAction)
            return;

        aimDirection = cameraObject.transform.forward.normalized;

        if (isInputActive)
        {
            //Gather Inputs
            float horizontal = PlayerInputManager.Instance.cameraHorizontalInput* upAndDownAimRotationSpeed * Time.deltaTime;
            float vertical = PlayerInputManager.Instance.cameraVerticalInput*leftAndRightAimRotationSpeed * Time.deltaTime;

            if (touchField != null && touchField.SwipeDelta != Vector2.zero)
            {
                horizontal = touchField.SwipeDelta.x * touchSensitivity* leftAndRightAimRotationSpeed / Screen.width*Time.deltaTime;
                vertical = -(touchField.SwipeDelta.y * touchSensitivity * upAndDownAimRotationSpeed / Screen.height) * Time.deltaTime;
            }


            //upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, aimedMinimumpivot, aimedMaximumpivot);

            leftAndRightLookAngle += horizontal;
            upAndDownLookAngle += vertical;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, aimedMinimumpivot, aimedMaximumpivot);
            // Rotate Cam



        }
        transform.rotation = Quaternion.Euler(0, leftAndRightLookAngle, 0);
        cameraObject.transform.localEulerAngles= new Vector3(upAndDownLookAngle, 0f, 0f);

    }


    public void OnIsAimingChanged(bool isAiming)
    {
        //Reset Local EulerAngles Of the Camera Object When Not Aiming
        if (!isAiming)
        {
            hasInitializedAim = false;
            cameraObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            player.playerAnimatorManager.EnableDisableIK(0, 0);
            player.playerAnimatorManager.UpdateAimedConstraints();




            Debug.Log("Reset CamObj");


        }
        //Reset Local EulerAngles of CameraPivot And Main Parent Object
        else
        {


            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = -4f;
            hasInitializedAim = true;
            player.playerAnimatorManager.EnableDisableIK(1, 1);
            player.playerAnimatorManager.UpdateAimedConstraints();


            Debug.Log("Reset CamPivot");

        }
        player.animator.SetBool("isAiming", player.isAiming);
    }



    private void HandleCollisions()
    {


        targetCameraZPosition = cameraZPosition;
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
                    cameraPivotTransform.transform.localRotation = Quaternion.Slerp(cameraPivotTransform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed);

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


    public void snapCameraToFocusPoint(Transform target)
    {
        isInFocusMode = true;
        player.canMove=false;
        cameraObject.transform.position = target.position;
        cameraObject.transform.rotation = target.rotation;
        MobileControls.instance.DisableMobileControls();
        player.playerBodyManager.TogglePlayerObject(false);


    }
    public void ResetCameraFromFocus()
    {
        isInFocusMode=false;
        player.canMove = true;
        cameraObject.transform.localPosition = new Vector3(0, 0, cameraZPosition); // Reset and Set it back to default Z offset
        cameraObject.transform.localRotation = Quaternion.identity;//reset Rotation
        MobileControls.instance.EnableMobileControls();
        player.playerBodyManager.TogglePlayerObject(true);

    }

    public void InspectObjectBasedOn(Vector2 screenPos)
    {
        if(!isInFocusMode)
            return;

        Ray ray = cameraObject.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, inspectRayDistance, inspectLayerMask))
        {
            // restrict to children of the inspected object
            if (currentInspectRoot != null && !hit.transform.IsChildOf(currentInspectRoot))
                return;

            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null)
                interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
                interactable.Interact(player);
        }

    }


}









