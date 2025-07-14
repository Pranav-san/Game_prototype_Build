using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInteractionManager : MonoBehaviour
{
    private playerManager player;
    [SerializeField] private Image RayCastCrossHairObject;
    [SerializeField] public Interactable currentInteractable;
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Inspect Object")]

    [SerializeField]float horizontalInput;
    [SerializeField]float verticalInput;
    [SerializeField]float upDownrotationSpeed = 0.5f;
    [SerializeField]float leftRightrotationSpeed = 0.2f;
    [SerializeField] Transform ObjectTransform;
    [SerializeField] private Vector3 initialObjectPosition;
    [SerializeField] private Quaternion initialObjectRotation;
    [SerializeField] private Vector3 initialObjectScale;
    [SerializeField] GameObject inspectionPoint;
    [SerializeField] GameObject FPInspectionPoint;
    [SerializeField] float InspectObjecttouchSensitivity = 5;

    [Header("Inspect object PC")]
    [SerializeField] float inspectrotationSpeedPC = 10;
    



    [SerializeField] public bool isInspectingObject = false;

    private List<Interactable> currentInteractableActions;

    private void Awake()
    {
        player = GetComponent<playerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
        inspectionPoint = PlayerCamera.instance.inspectionPoint;
        FPInspectionPoint = PlayerCamera.instance.FPInspectionPoint;
        currentInteractable=null;
    }

    private void FixedUpdate()
    {
        if (!PlayerUIManager.instance.menuWindowOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
        {
            CheckForInteractableUsingRayCast();
            CheckForInteractable();
            



        }
        
    }

    private void CheckForInteractable()
    {
        if (currentInteractableActions.Count == 0)
            return;
        if (currentInteractableActions[0]==null)
        {
            currentInteractableActions.RemoveAt(0);
            return;
        }

        if (currentInteractableActions !=null)
        {
            PlayerUIManager.instance.playerUIPopUPManager.ToggleInteractButton();
            PlayerUIManager.instance.playerUIPopUPManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }
            


    }

    private void CheckForInteractableUsingRayCast()
    {
        if (player.playerStatsManager.isDead|| player.isSprinting||player.isPerformingAction||player.playerCombatManager.isLockedOn || isInspectingObject)
            return;

        
        Ray ray;

        if (!PlayerCamera.instance.FPCameraSwitch)
        {
            ray = Camera.main.ScreenPointToRay(RayCastCrossHairObject.rectTransform.position);
            
            if (PlayerCamera.instance.cameraPivotAngle >= 45)
            {
                interactionDistance =12f;

            }
            else
            {
                interactionDistance = 8.5f;
            }

        }
        else
        {
            ray = PlayerCamera.instance.FPCameraObject.ScreenPointToRay(RayCastCrossHairObject.rectTransform.position);

        }
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponentInChildren<Interactable>();
            if (interactable != null && interactable.isRayCastInteractable)
            {
                currentInteractable = interactable;
                PlayerUIManager.instance.playerUIPopUPManager.ToggleInteractButton();
                return;
            }
            else
            {
                PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
                currentInteractable = null;
            }
        }
        
        

        // Clear UI if no interactable is hit
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
        currentInteractable = null;


    }

    public void Interact()
    {
        // Clear pop-up windows on interaction
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        if (currentInteractable != null)
        {
            currentInteractable.Interact(player);
            BringObjectNearCamera(currentInteractable);
            //currentInteractable = null;
        }
        else if (currentInteractableActions.Count > 0 && currentInteractable == null)
        {
            currentInteractableActions[0]?.Interact(player);
            RefreshInteractionList();
        }
    }

    public void AddInteractionToList(Interactable interactableObject)
    {
        RefreshInteractionList();
        if (!currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Add(interactableObject);
        }
    }

    public void RemoveInteractionFromList(Interactable interactableObject)
    {
        if (currentInteractableActions.Contains(interactableObject))
        {
            currentInteractableActions.Remove(interactableObject);
        }
        RefreshInteractionList();
    }

    private void RefreshInteractionList()
    {
        currentInteractableActions.RemoveAll(interactable => interactable == null);
    }

    public void BringObjectNearCamera(Interactable interactable)
    {
        float distance = 5f;
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(false);
        MobileControls.instance.DisableMobileControls();
        PlayerUIManager.instance.inspectObjectTouchField.SetActive(true);
        

        if (interactable != null)
        {
            if (!PlayerCamera.instance.FPCameraSwitch)
            {
                isInspectingObject = true;

                // Set the target distance the object should be from the camera
               

                //Store initial position & rotation of the Object
                initialObjectPosition = interactable.transform.position;
                initialObjectRotation = interactable.transform.rotation;
                initialObjectScale = interactable.transform.parent.localScale;

                

                // Ensure the inspection point is valid
                if (inspectionPoint == null)
                {
                    Debug.LogError("Inspection Point is not assigned!");
                    return;
                }

                // Get the camera's position and the inspection point position
                Vector3 cameraPosition = Camera.main.transform.position;
                Vector3 cameraForward = Camera.main.transform.forward;




                // Calculate the direction from the inspection point to the object
                Vector3 directionToInspectionPoint = (inspectionPoint.transform.position - interactable.transform.position).normalized;

                // Calculate the target position of the object near the camera (inspection point)
                Vector3 targetPosition = inspectionPoint.transform.position + cameraForward * distance + directionToInspectionPoint * distance / 2f;

                // Instantly move the object to the target position
                interactable.transform.parent.position = targetPosition;

                

                interactable.transform.parent.rotation = Quaternion.Euler(0,0,0);



                //scale the object if desired
                if(interactable.transform.parent.localScale.x < 0.5)
                {
                    interactable.transform.parent.localScale = new Vector3(0.3f, 0.3f, 0.3f);

                }

                
            }
            if (PlayerCamera.instance.FPCameraSwitch)
            {
                isInspectingObject = true;

                Vector3 FPCameraPosition = PlayerCamera.instance.FPCameraObject.transform.position;
                Vector3 FPCameraForward = PlayerCamera.instance.FPCameraObject.transform.forward;

                Vector3 FPDirectionToInspectionPoint = (FPInspectionPoint.transform.position - interactable.transform.position).normalized;
                Vector3 FPTargetPosition = FPInspectionPoint.transform.position + FPCameraForward * distance + FPDirectionToInspectionPoint * distance / 2f;
                interactable.transform.parent.position = FPTargetPosition;
            }
        }


    }
    public void ResetObjectPosition(GameObject targetObject)
    {
        if (targetObject != null)
        {
            // Set the object's position back to its initial position
            targetObject = currentInteractable.transform.parent.gameObject;
            currentInteractable.interactableCollider.enabled = true;
            targetObject.transform.position = initialObjectPosition;
            targetObject.transform.rotation = initialObjectRotation;
            targetObject.transform.localScale = initialObjectScale; 
            isInspectingObject = false;
        }
    }

    public void RotateObject(GameObject targetObject)
    {
        if (targetObject == null) return;

        // Get the camera's right and up directions
        Transform cameraTransform = Camera.main.transform;
        Vector3 cameraRight = cameraTransform.right;
        Vector3 cameraUp = cameraTransform.up;
        Vector3 cameraForward = cameraTransform.forward;

        if (PlayerCamera.instance.inspectObjectTouchField != null && PlayerCamera.instance.inspectObjectTouchField.SwipeDelta != Vector2.zero)
        {
            // For mobile
            float rotationX = (PlayerCamera.instance.inspectObjectTouchField.SwipeDelta.y * InspectObjecttouchSensitivity * upDownrotationSpeed) * Time.deltaTime;
            float rotationY = -(PlayerCamera.instance.inspectObjectTouchField.SwipeDelta.x * InspectObjecttouchSensitivity * leftRightrotationSpeed) * Time.deltaTime;
            float rotationZ = 0;

            // Apply rotation based on camera's orientation
            targetObject.transform.rotation = Quaternion.AngleAxis(rotationX * leftRightrotationSpeed, cameraRight) *
                                              Quaternion.AngleAxis(rotationY * upDownrotationSpeed, cameraUp) *
                                              Quaternion.AngleAxis(rotationZ, cameraForward) *
                                              targetObject.transform.rotation;
        }
        else if (PlayerInputManager.Instance.cameraHorizontalInput != 0 || PlayerInputManager.Instance.cameraVerticalInput != 0)
        {
            // For desktop
            float rotationX = (PlayerInputManager.Instance.cameraVerticalInput * inspectrotationSpeedPC) * Time.deltaTime;
            float rotationY = -(PlayerInputManager.Instance.cameraHorizontalInput * inspectrotationSpeedPC) * Time.deltaTime;
            float rotationZ = 0;

            // Apply rotation based on camera's orientation
            targetObject.transform.rotation = Quaternion.AngleAxis(rotationX * leftRightrotationSpeed, cameraRight) *
                                              Quaternion.AngleAxis(rotationY * upDownrotationSpeed, cameraUp) *
                                              Quaternion.AngleAxis(rotationZ, cameraForward) *
                                              targetObject.transform.rotation;
        }
    }



    public void LeaveItem()
    {
        if (isInspectingObject)
        {

            isInspectingObject = false;
            
            ResetObjectPosition(currentInteractable.transform.parent.gameObject);
            PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
            PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
            PlayerUIManager.instance.inspectObjectTouchField.SetActive(false);
            MobileControls.instance.EnableMobileControls();
            




        }
       


    }
    public void PickUpItem()
    {
        if (currentInteractable != null)
        {
            
            PickUpItemInteractable pickUpItemInteractable = currentInteractable as PickUpItemInteractable;
            // Add item to the player's inventory
            player.playerInventoryManager.AddItemToInventory(pickUpItemInteractable.item);

            // Play the pick-up sound effect
            player.characterSoundFxManager.PlaySoundfx(WorldSoundFXManager.instance.pickUpItemSfx);

            // Show UI pop-up for the item pick-up
            PlayerUIManager.instance.playerUIPopUPManager.SendItemPopUp(pickUpItemInteractable.item, 1);

            // Handle item loot status if it's a world spawn item
            if (pickUpItemInteractable.pickUpType == ItemPickUpType.WorldSpwan)
            {
                if (WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.ContainsKey((int)pickUpItemInteractable.itemID))
                {
                    WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Remove(pickUpItemInteractable.itemID);
                }
                WorldSaveGameManager.instance.currentCharacterData.worldItemsLooted.Add((int)pickUpItemInteractable.itemID, true);
            }

            // Destroy the item after it has been picked up (or deactivate it if it's a world spawn)
            
                Destroy(pickUpItemInteractable.gameObject);
                Destroy(pickUpItemInteractable.transform.parent.gameObject);

            isInspectingObject = false;
            PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
            PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
            PlayerUIManager.instance.inspectObjectTouchField.SetActive(false);
            MobileControls.instance.EnableMobileControls();




            //pickUpItemInteractable.gameObject.SetActive(false);

        }
    }





    private void OnDrawGizmos()
    {
        //if (player.isDead || player.isSprinting || player.isPerformingAction || player.playerCombatManager.isLockedOn)
        //    return;
        // Ray Gizmo
        Gizmos.color = Color.green;
        Vector3 crosshairScreenPosition = RayCastCrossHairObject.rectTransform.position;
        Ray ray = Camera.main.ScreenPointToRay(crosshairScreenPosition);
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * interactionDistance);

        // Hit point Gizmo
        if (currentInteractable != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ray.origin + ray.direction * interactionDistance, 0.2f);
        }
    }
}
