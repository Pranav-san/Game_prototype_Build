using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUILoadingScreenManager : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGroup;

    private Coroutine fadeLoadingScreenCoroutine;

    private void Start()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        DeactivateLoadingScreen();

    }


    public void ActivateLoadingScreen()
    {
        //If the Loading screen is Active return
        if (loadingScreen.activeSelf)
            return;

             

        canvasGroup.alpha = 1.0f;

        WorldSaveGameManager.instance.DisableMobileControlsOnSceneChange();
        loadingScreen.SetActive(true);
        PlayerUIManager.instance.isLoadingScreenActive = true;

    }

    public void DeactivateLoadingScreen(float delay = 1)
    {
        //If the Loading screen is not Active return
        if (loadingScreen == null||!loadingScreen.activeSelf)
            return;
        if (fadeLoadingScreenCoroutine != null)
            return;

        

        



        fadeLoadingScreenCoroutine = StartCoroutine(FadeLoadingScreen(1, delay));

      

    }

    private IEnumerator FadeLoadingScreen(float duration, float delay)
    {

        while (WorldAIManager.instance.isPerformingLoadingOperation)
        {
            yield return null;
        }
        loadingScreen.SetActive(true);
        PlayerUIManager.instance.isLoadingScreenActive = true;

        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "Main menu")
        {
            WorldSaveGameManager.instance.DisableMobileControlsOnSceneChange();
        }
        else
        {
            WorldSaveGameManager.instance.EnableMobileControlsOnSceneChange();
            PlayerUIManager.instance.mobileControls.DisableMobileControls();
        }

        while(delay > 0)
        {
            delay -=Time.deltaTime;
            yield return null;

        }

        canvasGroup.alpha = 1.0f;
        float elaspsedTime = 0;

        while(elaspsedTime <duration)
        {
            elaspsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1,0,elaspsedTime/duration);

            if (sceneName != "Main menu")
                PlayerUIManager.instance.mobileControls.EnableMobileControls();

            yield return null;
        }

        canvasGroup.alpha = 0f;
        loadingScreen.SetActive(false);
        PlayerUIManager.instance.isLoadingScreenActive = false;
        fadeLoadingScreenCoroutine = null;

        





        yield return null;

    }


}
