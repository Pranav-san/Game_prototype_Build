using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableText;
    [SerializeField]public Collider interactableCollider;
    //[SerializeField] protected bool hostOnlyInteractable;


    [Header("Puzzle/Inspect Camera Settings")]
    public bool useCameraFocus = false;
    public Transform focusPoint;



    public virtual void Awake()
    {
        if(interactableCollider ==  null)
        {
            interactableCollider = GetComponent<Collider>();    
        }

        
    }

    protected virtual void Start()
    {

       

    }

    public virtual void Interact(playerManager player)
    {

        Debug.Log("You Have Interacted");

        interactableCollider.enabled = false;

        player.playerInteractionManager.RemoveInteractionFromList(this);
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        //Save Game
        WorldSaveGameManager.instance.SaveGame();

        
        


    }

    public virtual void OnRayCastHit(playerManager player)
    {
        if(player!=null)
        {

        }

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        playerManager player =other.GetComponent<playerManager>();

        if(player != null)
        {
            player.playerInteractionManager.AddInteractionToList(this);
        }
        

    }
    public virtual void OnTriggerExit(Collider other)
    {
        playerManager player = other.GetComponent<playerManager>();

        if (player != null)
        {

            player.playerInteractionManager.RemoveInteractionFromList(this);
            player.playerInteractionManager.currentInteractable = null;

          

            PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        }

    }

    public void RefreshInteractableColliders()
    {
        interactableCollider.enabled = true;

    }



}
