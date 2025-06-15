using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Animator animator;

    [SerializeField] bool isDoorOpen = false;

    

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    public override void Interact(playerManager player)
    {
        DoorAnimation();

        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();


    }

    public void DoorAnimation()
    {
        

        if (!isDoorOpen)
        {
          
            animator.Play("Open");
            isDoorOpen = true;
        }
        else if (isDoorOpen) 
        {
            
            animator.Play("Close");
            isDoorOpen=false;
        }
    }

}
