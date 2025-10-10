using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerInteractionManager : MonoBehaviour
{
    private playerManager player;
    public Image RayCastCrossHairObject;
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
    
    [SerializeField] float InspectObjecttouchSensitivity = 5;

    [Header("Inspect object PC")]
    [SerializeField] float inspectrotationSpeedPC = 10;
    



    [SerializeField] public bool isInspectingObject = false;


    public List<Interactable> currentInteractableActions;
    public AICharacterManager npc;

    private void Awake()
    {
        player = GetComponent<playerManager>();
    }

    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
        
        
        currentInteractable=null;
    }

    private void FixedUpdate()
    {
        if (!PlayerUIManager.instance.menuWindowOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
        {
           
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
            currentInteractable = currentInteractableActions[0];
            PlayerUIManager.instance.playerUIPopUPManager.ToggleInteractButton();
            PlayerUIManager.instance.playerUIPopUPManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);
        }
            


    }

  

    public void Interact()
    {
        // Clear pop-up windows on interaction

        if(player.playerLocomotionManager.isClimbingLadder || player.playerLocomotionManager.isExitingLadder)
            return;

        if(player.playerStatsManager.isDead)
            return;


        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        if (currentInteractable != null)
        {

            currentInteractable.Interact(player);


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

    



    public void LeaveItem()
    {
        if (isInspectingObject)
        {

            isInspectingObject = false;
            
           
            PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();
            PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
            PlayerUIManager.instance.inspectObjectTouchField.SetActive(false);
        
            




        }
       


    }
    public void PickUpItem()
    {
        if (currentInteractable != null)
        {
            
            PickUpItemInteractable pickUpItemInteractable = currentInteractable as PickUpItemInteractable;
            // Add item to the character's inventory
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





    //private void OnDrawGizmos()
    //{
    //    //if (character.isDead || character.isSprinting || character.isPerformingAction || character.playerCombatManager.isLockedOn)
    //    //    return;
    //    // Ray Gizmo
    //    Gizmos.color = Color.green;
    //    Vector3 crosshairScreenPosition = RayCastCrossHairObject.rectTransform.position;
    //    Ray ray = Camera.main.ScreenPointToRay(crosshairScreenPosition);
    //    Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * interactionDistance);

    //    // Hit point Gizmo
    //    if (currentInteractable != null)
    //    {
    //        Gizmos.color = Color.yellow;
    //        Gizmos.DrawWireSphere(ray.origin + ray.direction * interactionDistance, 0.2f);
    //    }
    //}
}
