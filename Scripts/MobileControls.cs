using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public static MobileControls instance;

    [Header("Canvas Group")]
    [SerializeField] CanvasGroup Actionbuttons;

    [SerializeField]FloatingJoystick joystick;
    Canvas canvas;

   





    [SerializeField] GameObject floatingJoyStick;
    
    
    [SerializeField] GameObject lightAttackButtton;
   
    
    
    
    
    [SerializeField] GameObject TouchField;

    [SerializeField] Image TwoHandIcon;
    [SerializeField] Image OneHandIcon;



    //[SerializeField] GameObject postProcessingVolumeObject;


    private void Start()
    {
        //TouchField = GetComponentInChildren<TouchField>().gameObject;
        joystick = GetComponentInChildren<FloatingJoystick>();

    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this GameObject is not destroyed when loading new scenes

        }
        else
        {
            Destroy(gameObject);
        }

        

        
        canvas = GetComponentInChildren<Canvas>();
        
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
   
    
}
