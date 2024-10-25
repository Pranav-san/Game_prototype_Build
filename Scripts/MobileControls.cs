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

    }
    public void EnableMobileControls()
    {
        analogStick.SetActive(true);
        sprintRollButton.SetActive(true);
        LockOnButton.SetActive(true);

    }
    
}
