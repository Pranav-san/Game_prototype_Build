using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUICharacterMenuManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;
    

    public void OpenCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);

    }
    public void CloseCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);

    }

    



    public void ReturnToMainMenu()
    {
        
        CloseCharacterMenu();

        WorldAIManager.instance.DeSpawnAllCharacters();


        // Load the Title Screen Scene (replace "TitleScreenScene" with the actual name of your title screen scene)
        SceneManager.LoadScene("Main menu");
        WorldSaveGameManager.instance.TitleScreen.SetActive(true);

        

       
    }

    public void CloseCharacterMenuAfterFixedUpdtate()
    {
        StartCoroutine(WaitThenCloseMenu());

    }

    private IEnumerator WaitThenCloseMenu()
    {
        yield return new WaitForFixedUpdate();
        
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);
    }

    public void Quicksave()
    {
        WorldSaveGameManager.instance.SaveGame();
        

    }
}
