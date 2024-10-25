using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Playables;

public class TitleScreenManager : MonoBehaviour
{

    public static TitleScreenManager Instance;

    [Header("Frame Settings")]
    int MaxFrameRate = 120;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;



    [Header("Menu objects")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;
    [SerializeField] GameObject SettingsMenu;


    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button SettingsMenuButton;
    [SerializeField] Button deleteCharacterPopUpConfirmbutton;

    [Header("PoPups")]
    [SerializeField] GameObject noCharacterSlotPopUp;
    [SerializeField] Button noCharacterSlotPopUpOkaybutton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character SLots")]
    public CharacterSlot currentSelectedCharacterSlot = CharacterSlot.N0_SLOT;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
           
            
        }
        else
        {
            Destroy(gameObject);
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxFrameRate;
        currentFrameTime = Time.realtimeSinceStartup;
        StartCoroutine("WaitForNextFrame");


    }

    IEnumerator WaitForNextFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            currentFrameTime += 1.0f / TargetFrameRate;
            var t = Time.realtimeSinceStartup;
            var sleepTime = currentFrameTime - t - 0.01f;
            if (sleepTime > 0)
                Thread.Sleep((int)(sleepTime * 1000));
            while (t < currentFrameTime)
                t = Time.realtimeSinceStartup;
        }
    }

    public void StartNewGame()
    {
        

        WorldSaveGameManager.instance.AttemptCreateNewGame();

        



    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenLoadGameMenu()
    {
        //Close Main Menu
        titleScreenMainMenu.SetActive(false);

        //Open Load Menu
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();

    }

    public void CloseLoadGameMenu()
    {
        //Open Main Menu
        titleScreenLoadMenu.SetActive(false);

        //Close Load Menu
        titleScreenMainMenu.SetActive(true);

        

        mainMenuLoadGameButton.Select();


    }

    public void OpenSettingsMenu()
    {
        //Close Main Menu
        titleScreenMainMenu.SetActive(false);

        //Open Load Menu
        SettingsMenu.SetActive(true);

        

    }
    public void CloseSettingsMenu()
    {
        //Open Main Menu
        SettingsMenu.SetActive(false);

        //Close Load Menu
        titleScreenMainMenu.SetActive(true);



        mainMenuLoadGameButton.Select();


    }
    public void DisplayNoFreeCharacterSlots()
    {
        noCharacterSlotPopUp.SetActive(true);
        noCharacterSlotPopUpOkaybutton.Select();

    }
    public void CloseNoFreeCharacterSlots()
    {
        noCharacterSlotPopUp.SetActive(false);
        mainMenuNewGameButton.Select();

    }
    public void SelectCharacterSlot(CharacterSlot characterSlot)
    {
        currentSelectedCharacterSlot = characterSlot;
        
    }
    public void SelectNoSLot()
    {
        currentSelectedCharacterSlot = CharacterSlot.N0_SLOT;
    }
    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelectedCharacterSlot!=CharacterSlot.N0_SLOT)
        {
            deleteCharacterSlotPopUp.SetActive(true);
            deleteCharacterPopUpConfirmbutton.Select(); 

        }
        
    }

    public void DeleteCharacterSlot()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        WorldSaveGameManager.instance.Deletegame(currentSelectedCharacterSlot);
        
        //disable and Enable Load Menu To refresh Deleted slots
        titleScreenLoadMenu.SetActive(false );
        titleScreenLoadMenu.SetActive(true);

        loadMenuReturnButton.Select();
        

    }
    public void CloseDeleteCharacterSlotPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();

    }



    


}
