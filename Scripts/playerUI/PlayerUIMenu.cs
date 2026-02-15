using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerUIMenu : MonoBehaviour
{
    [Header("Menu")]
    [SerializeField] protected GameObject menu;
    [SerializeField] protected MenuSlideAnimator menuAnimator;
    [SerializeField] protected MenuFadeInOut menuFadeInOut;

    [Header("Settings ")]
    [SerializeField] GameObject settingsButton;


    public virtual void OpenMenu()
    {
       
        menu.SetActive(true);

        //if (menuAnimator != null)
        //{
        //    menuAnimator.ShowMenu();
        //}

        //if (menuFadeInOut != null)
        //{
        //    menuFadeInOut.FadeIn();
        //}
        PlayerUIManager.instance.menuWindowOpen =true;



    }
    public virtual void CloseMenu()
    {
        
        //if (menuAnimator != null )
        //{
        //    menuAnimator.HideMenu();
        //}

        //if (menuFadeInOut != null)
        //{
        //    menuFadeInOut.FadeOut(true);
        //}
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);


    }
    public virtual void ReturnToMainMenu()
    {

        StartCoroutine(ReturnToMainMenuRoutine());


    }

    protected IEnumerator ReturnToMainMenuRoutine()
    {
        PlayerUIManager.instance.CloseAllMenu();
        PlayerUIManager.instance.mobileControls.ToggleInventoryButton(false);

        WorldSaveGameManager.instance.DisableMobileControlsOnSceneChange();


        // 1. Activate loading screen
        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();


        // 3. Despawn AI characters
        WorldAIManager.instance.DeSpawnAllCharacters();

        // 4. Wait for characters to finish despawning
        while (WorldAIManager.instance.isPerformingLoadingOperation)
        {
            yield return null;
        }

        PlayerInputManager.Instance.player.transform.position = PlayerInputManager.Instance.player.titleScreenPlayerPosition;
        PlayerInputManager.Instance.player.transform.rotation = PlayerInputManager.Instance.player.titleScreenPlayerRotation;

        PlayerInputManager.Instance.player.playerAnimatorManager.PlayTargetActionAnimationInstantly("Title Screen Animation", false);


        PlayerCamera.instance.TitleScreenCameraOffset();

        //PlayerCamera.instance.touchField.gameObject.SetActive(false);


        

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync("Main menu");
        loadOperation.completed += (AsyncOperation op) =>
        {
            Vector3 position = PlayerCamera.instance.player.titleScreenPlayerPosition;
            PlayerCamera.instance.player.transform.position = position;

        };

        // 7. No need to manually deactivate the loading screen — it's handled in OnSceneChanged



    }

    public virtual void CloseMenuAfterFixedUpdtate()
    {
        StartCoroutine(WaitThenCloseMenu());

    }

    protected virtual IEnumerator WaitThenCloseMenu()
    {

        yield return new WaitForFixedUpdate();

        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);


    }

    public void Quicksave()
    {
        WorldSaveGameManager.instance.SaveGame();


    }

    public void ToggleSettingsButton(bool value = false)
    {
        if (value)
        {
            settingsButton.SetActive(true);
        }
        else
        {
            settingsButton.SetActive(false);
        }

    }
}
