using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIToggleHud : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(false);
        MobileControls.instance.DisableMobileControls();
        
        
        
        
    }
    private void OnDisable()
    {
        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
        MobileControls.instance.EnableMobileControls();



    }
}
