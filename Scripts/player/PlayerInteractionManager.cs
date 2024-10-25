using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    playerManager player;

    private List<Interactable> currentInteractableActions;    

    private void Awake()
    {
        player = GetComponent<playerManager>();
    }
    private void Start()
    {
        currentInteractableActions = new List<Interactable>();
    }

    private void FixedUpdate()
    {
        if(!PlayerUIManager.instance.menuWindowOpen && !PlayerUIManager.instance.popUpWindowIsOpen)
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
            PlayerUIManager.instance.playerUIPopUPManager.SendPlayerMessagePopUp(currentInteractableActions[0].interactableText);


    }

    public void RefreshInteractionList()
    { 
        for(int i = currentInteractableActions.Count -1 ; i > - 1; i--)
        {
            if(currentInteractableActions[i]==null)
            {
                currentInteractableActions.RemoveAt(i);
            }

        }


    }


    public void Interact()
    {
        // IF WE PRESS THE INTERACT BUTTON WITH OR WITHOUT AN INTERACTABLE, IT WILL CLEAR THE POP UP WINDOWS (ITEM PICK UPS, MESSAGES, ECT)
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        if(currentInteractableActions.Count == 0) 
            return;

        if (currentInteractableActions[0] !=null)
        {
            currentInteractableActions[0].Interact(player);
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

}
