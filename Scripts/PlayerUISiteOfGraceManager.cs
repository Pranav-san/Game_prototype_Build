using UnityEngine;

public class PlayerUISiteOfGraceManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

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
}
