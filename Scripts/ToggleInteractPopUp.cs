using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractPopUp : MonoBehaviour
{
    [SerializeField] GameObject LightAttack_RB_Input;

    void Update()
    {
        ToggleInteractPopUpButton();
    }
    public void ToggleInteractPopUpButton()
    {
        if (PlayerUIManager.instance.playerUIPopUPManager.popUpMessageGameObject.activeSelf)
        {
            // If the Interact popUp is active, disable LightAttack_RB_Input
            LightAttack_RB_Input.SetActive(false);
        }
        else
        {
            // Re-enable LightAttack_RB_Input if popUPMessageGameObject is not active
            LightAttack_RB_Input.SetActive(true);
        }

    }
}
