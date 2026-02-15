using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{

    [Header("Canvas Group")]
    [SerializeField] CanvasGroup Actionbuttons;

    public FloatingJoystick joystick;


    [SerializeField]GameObject floatingJoyStick;
    [SerializeField] GameObject lightAttackButtton;
    [SerializeField] GameObject defendButton;
    [SerializeField] GameObject leftFireButton;
   


  
    [SerializeField] GameObject TouchField;

    [SerializeField] Image TwoHandIcon;
    [SerializeField] Image OneHandIcon;
    [SerializeField] Image defendIcon;
    [SerializeField] Image aimIcon;
    [SerializeField] Image aimLocedIcon;

    [SerializeField] Image lightAttackButton;
    [SerializeField] Sprite gunIcon;
    [SerializeField] Sprite arrowIcon;
    [SerializeField] Sprite meleeIcon;

    [Header("Lock On")]
    [SerializeField] GameObject lockoff;
    [SerializeField] GameObject lockon;
    [SerializeField] GameObject aimlockedOn;

    [Header("InventoryButton")]
    public GameObject inventoryButton;



    //[SerializeField] GameObject postProcessingVolumeObject;


    private void Start()
    {
        //TouchField = GetComponentInChildren<TouchField>().gameObject;
        joystick = GetComponentInChildren<FloatingJoystick>();

    }
    private void Awake()
    {
        
    }

    public void DisableMobileControls()
    {
        if (Actionbuttons != null)
        {
            Actionbuttons.alpha = 0;
            Actionbuttons.interactable = false;
            Actionbuttons.blocksRaycasts = false;
        }

        if (joystick != null)
        {
            joystick.HideJoystickCompletely();
        }






        //postProcessingVolumeObject.SetActive(true);

    }
    public void EnableMobileControls()
    {
        if (Actionbuttons != null)
        {
            Actionbuttons.alpha = 1;
            Actionbuttons.interactable = true;
            Actionbuttons.blocksRaycasts = true;
        }

        if (joystick != null)
        {
            joystick.UnHideJoystick();
        }



        //postProcessingVolumeObject.SetActive(false);

    }


    public void ToggleTwoHandButtonIcon(bool isTwoHandingWeapon)
    {
        if (isTwoHandingWeapon)
        {
            TwoHandIcon.enabled = true;
            TwoHandIcon.gameObject.SetActive(true);
            OneHandIcon.enabled = false;
            OneHandIcon.gameObject.SetActive(false) ;
        }
        else
        {
            TwoHandIcon.enabled = false;
            TwoHandIcon.gameObject.SetActive(false);
            OneHandIcon.enabled = true;
            OneHandIcon.gameObject.SetActive(true);
        }
        

    }

    public void ToggleAimIcon(bool isRangedWeapon)
    {
        if(isRangedWeapon)
        {
            if(defendButton != null)
            {
                defendButton.SetActive(false);
                defendIcon.enabled = false;
            }
            
            
            aimIcon.gameObject.SetActive(true);
            aimIcon.enabled = true;

        }
        else
        {
            aimIcon.gameObject.SetActive(false);
            aimIcon.enabled = false;
            if(defendButton != null)
            {
                defendButton.SetActive(true);
                defendIcon.enabled = true;
            }
            

        }

    }

    public void ToggleLeftFireButton(bool isAimLockedOn)
    {
        if (isAimLockedOn)
        {
            if (defendButton!=null)
            {
                defendButton.SetActive(false);
                defendIcon.enabled = false;
            }
            
            aimIcon.gameObject.SetActive(false);
            aimIcon.enabled = false;
            leftFireButton.SetActive(true);

        }
        else
        {
            aimIcon.gameObject.SetActive(true);
            aimIcon.enabled = true;
            leftFireButton.SetActive(false);

            if (defendButton!=null)
            {
                defendButton.SetActive(true);
                defendIcon.enabled = true;
            }
              

        }

    }

    public void LoadBulletOrBowSprite(bool isBow)
    {
        if (!isBow)
        {
            lightAttackButton.sprite = gunIcon;
        }
        else
        {
            lightAttackButton.sprite = arrowIcon;
        }
       

    }

    public void LoadMeleeSprite()
    {

        lightAttackButton.sprite = meleeIcon;    
        


    }

    public void EnablelockOn()
    {

        
        if (playerManager.instance.playerCombatManager.isLockedOn)
        {
            aimlockedOn.SetActive(false);
            lockoff.SetActive(false);
            lockon.SetActive(true);
        }
        else
        {
            aimlockedOn.SetActive(false);
            lockon.SetActive(false);
            lockoff.SetActive(true);
        }



    }

    public void EnableAimLockOn()
    {
        if (playerManager.instance.playerCombatManager.isAimLockedOn)
        {
            aimlockedOn.SetActive(true);
            lockoff.SetActive(false);
            lockon.SetActive(false);


        }
        else
        {
            aimlockedOn.SetActive(false);
            lockoff.SetActive(true);
            lockon.SetActive(false);

        }
    }


    public void ToggleInventoryButton(bool toggle = false)
    {
        if (toggle)
        {
            inventoryButton.SetActive(true);
        }
        else
        {
            inventoryButton.SetActive(false);
        }
    }



}
