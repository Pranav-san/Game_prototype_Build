using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUICharacterMenuManager : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] GameObject menu;
    [SerializeField] MenuSlideAnimator menuAnimator;




    public void OpenCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);
        menuAnimator.ShowMenu();
        

    }
    public void CloseCharacterMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menuAnimator.HideMenu();
        menu.SetActive(false);

    }

   
  
   





    public void ReturnToMainMenu()
    {

        StartCoroutine(ReturnToMainMenuRoutine());








    }

    private IEnumerator ReturnToMainMenuRoutine()
    {
        PlayerUIManager.instance.CloseAllMenu();

        WorldSaveGameManager.instance.DisableMobileControlsOnSceneChange();
        

        // 1. Activate loading screen
        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();
        yield return new WaitForSeconds(0.2f); // small delay to let screen appear

        // 2. Close menu
        

        // 3. Despawn AI characters
        WorldAIManager.instance.DeSpawnAllCharacters();

        // 4. Wait for characters to finish despawning
        while (WorldAIManager.instance.isPerformingLoadingOperation)
        {
            yield return null;
        }

        // 5. (Optional) Reset character position if needed
        PlayerInputManager.Instance.player.transform.position = PlayerInputManager.Instance.player.defaultPlayerposition;
        PlayerInputManager.Instance.player.transform.rotation = Quaternion.identity;

        PlayerCamera.instance.transform.rotation = Quaternion.identity;
        PlayerCamera.instance.cameraPivotTransform.transform.rotation = Quaternion.identity;

        // 6. Load Main Menu scene
        SceneManager.LoadScene("Main menu");

        // 7. No need to manually deactivate the loading screen — it's handled in OnSceneChanged
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
