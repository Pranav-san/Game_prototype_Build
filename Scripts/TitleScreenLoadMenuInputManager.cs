using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenLoadMenuInputManager : MonoBehaviour
{

    PlayerControls2 playerControls;
    [Header("Title Screen Inputs")]
    [SerializeField] bool deletCharacterSlot = false;

    private void Update()
    {
        if(deletCharacterSlot)
        {
            deletCharacterSlot =false;
            TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
        }
    }



    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls2();
            playerControls.UI.X.performed += i => deletCharacterSlot=true;
            deletCharacterSlot=false;

        }
        playerControls.Enable();
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        
    }
}
