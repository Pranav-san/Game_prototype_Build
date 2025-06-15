using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public static MobileControls instance;


    [SerializeField] GameObject analogStick;
    [SerializeField] GameObject sprintRollButton;
    [SerializeField] GameObject LockOnButton;
    [SerializeField] GameObject lightAttackButtton;
    [SerializeField] GameObject HeavyAttackButton;
    [SerializeField] GameObject itemWheelButton;
    [SerializeField] GameObject LB_Input_1;
    [SerializeField] GameObject LB_Input_2;

    //[SerializeField] GameObject postProcessingVolumeObject;

    

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

    public void DisableMobileControls()
    {
        analogStick.SetActive(false);
        sprintRollButton.SetActive(false);
        LockOnButton.SetActive(false);
        lightAttackButtton.SetActive(false);
        HeavyAttackButton.SetActive(false);
        itemWheelButton.SetActive(false);
        LB_Input_1.SetActive(false);
        LB_Input_2.SetActive(false);



        
        //postProcessingVolumeObject.SetActive(true);

    }
    public void EnableMobileControls()
    {
        analogStick.SetActive(true);
        sprintRollButton.SetActive(true);
        LockOnButton.SetActive(true);
        lightAttackButtton.SetActive(true);
        HeavyAttackButton.SetActive(true);
        itemWheelButton.SetActive(true);
        LB_Input_1.SetActive(true);
        LB_Input_2.SetActive(true);

        //postProcessingVolumeObject.SetActive(false);

    }
   
    
}
