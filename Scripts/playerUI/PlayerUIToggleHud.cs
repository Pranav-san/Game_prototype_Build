using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUIToggleHud : MonoBehaviour
{
    private void OnEnable()
    {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            return;


        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(false);
        PlayerUIManager.instance.mobileControls.DisableMobileControls();
        
        
        
        
    }
    private void OnDisable()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
            return;

        PlayerUIManager.instance.playerUIHUDManager.ToggleHUD(true);
        PlayerUIManager.instance.mobileControls.EnableMobileControls();



    }
}
