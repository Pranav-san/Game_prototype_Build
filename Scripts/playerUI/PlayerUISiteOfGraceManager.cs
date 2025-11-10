using UnityEngine;

public class PlayerUISiteOfGraceManager : PlayerUIMenu
{
    

    public void OpenSiteOfGraceManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);
      

    }
    public void CloseSiteOfGraceManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);

    }

    public void OpenTeleportLocationMenu()
    {
        CloseSiteOfGraceManagerMenu();
        PlayerUIManager.instance.playerUITeleportLocationManager.OpenTeleportLocationManagerMenu();

    }


    public void OpenLevelUpMenu()
    {
        CloseMenu();
        PlayerUIManager.instance.playerUILevelUpManager.OpenMenu();

    }
}
