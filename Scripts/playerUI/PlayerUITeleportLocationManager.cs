using UnityEngine;
using UnityEngine.Rendering;

public class PlayerUITeleportLocationManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

    public void OpenTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        PlayerUIManager.instance.mobileControls.DisableMobileControls();
        menu.SetActive(true);


    }
    public void CloseTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        PlayerUIManager.instance.mobileControls.EnableMobileControls();
        menu.SetActive(false);

    }

    public void TeleportToSiteOfGrace(int siteId)
    {
        for (int i = 0; i< WorldObjectManager.instance.sitesOfGrace.Count; i++)
        {
            if (WorldObjectManager.instance.sitesOfGrace[i].siteOfGraceID == siteId)
            {
                
                
                //Teleport
                WorldObjectManager.instance.sitesOfGrace[i].TeleportToSiteOfGrace();
                CloseTeleportLocationManagerMenu();



                return;
            }

        }

    }
}
