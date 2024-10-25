using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string interactableText;
    [SerializeField]protected Collider interactableCollider;
    [SerializeField] protected bool hostOnlyInteractable;

    protected virtual void Awake()
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

        //interactableCollider.enabled = false;

        player.playerInteractionManager.RemoveInteractionFromList(this);

        
        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        


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

          

            PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();

        }

    }



}
