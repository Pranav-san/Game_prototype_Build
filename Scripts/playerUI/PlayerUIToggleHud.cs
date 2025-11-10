using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIToggleHud : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(false);
        PlayerUIManager.instance.mobileControls.DisableMobileControls();
        
        
        
        
    }
    private void OnDisable()
    {
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
        PlayerUIManager.instance.mobileControls.EnableMobileControls();



    }
}
