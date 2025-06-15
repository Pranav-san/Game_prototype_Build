using UnityEngine;

public class PlayerUITeleportLocationManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;

    public void OpenTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);


    }
    public void CloseTeleportLocationManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);

    }
}
