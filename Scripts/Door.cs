using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public Animator animator;

    [SerializeField] bool isDoorOpen = false;


    [Header("Lock/Passcode")]
    [SerializeField] public bool isLocked = false;
    [SerializeField] public bool requireKey = false;
    [SerializeField] string passCode = "123";
    [SerializeField] public int keyID = -1;
    [SerializeField] public string keyName = "Door Key";




    public override void Awake()
    {
        base.Awake(); 
        
        animator = GetComponentInChildren<Animator>();
    }
    
    

    public override void Interact(playerManager player)
    {

        if(isLocked)
        {
            
                PlayerUIManager.instance.doorLockManager.OpenDoorlockUI(this, player);
                return;
           
        }




        DoorAnimation();

        PlayerUIManager.instance.playerUIPopUPManager.CloseAllPopUpWindow();


    }

    public  string GetDoorPassCode()
    {
        return passCode;
    }

    public void UnlockDoor()
    {
        isLocked = false;
        requireKey = false;

        DoorAnimation();

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
