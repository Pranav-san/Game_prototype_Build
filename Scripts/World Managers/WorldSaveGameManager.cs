using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSaveGameManager : MonoBehaviour
{

    [SerializeField] playerManager player;

    [Header("Save/load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;

    [Header("Loading Screen UI")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] public GameObject TitleScreen;
    [SerializeField] Slider loadingSlider;
    


    public static WorldSaveGameManager instance;
    [Header("Scene Index")]
    [SerializeField] int worldSceneIndex = 1;

    [Header("Save Data Writer")]
    private SaveFileDataWriter saveFileDataWriter;

    [Header("Current Character Data")]
    public CharacterSlot currentCharacterSlotBeingUsed;
    public CharacterSaveData currentCharacterData;
    private string saveFileName;

    [Header("Character Slots")]
    public CharacterSaveData characterSlot01;
    public CharacterSaveData characterSlot02;
    public CharacterSaveData characterSlot03;
    public CharacterSaveData characterSlot04;
    public CharacterSaveData characterSlot05;
    public CharacterSaveData characterSlot06;
    public CharacterSaveData characterSlot07;
    public CharacterSaveData characterSlot08;
    public CharacterSaveData characterSlot09;
    public CharacterSaveData characterSlot10;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Update()
    {
        if(saveGame)
        {
            saveGame = false;
            SaveGame();
        }
        if(loadGame)
        {
            loadGame = false;
            LoadGame();
        }
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        TitleScreen = TitleScreenManager.Instance.GameObject();  
        LoadAllCharacterProfiles();
    }

    public string DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot characterSlot)
    {
        string fileName = "";
        switch(characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                fileName = "CharacterSlot_01";
                break;
            case CharacterSlot.CharacterSlot_02:
                fileName = "CharacterSlot_02";
                break;
            case CharacterSlot.CharacterSlot_03:
                fileName = "CharacterSlot_03";
                break;
            case CharacterSlot.CharacterSlot_04:
                fileName = "CharacterSlot_04";
                break;
            case CharacterSlot.CharacterSlot_05:
                fileName = "CharacterSlot_05";
                break;
            case CharacterSlot.CharacterSlot_06:
                fileName = "CharacterSlot_06";
                break;
            case CharacterSlot.CharacterSlot_07:
                fileName = "CharacterSlot_07";
                break;
            case CharacterSlot.CharacterSlot_08:
                fileName = "CharacterSlot_08";
                break;
            case CharacterSlot.CharacterSlot_09:
                fileName = "CharacterSlot_09";
                break;
            case CharacterSlot.CharacterSlot_10:
                fileName = "CharacterSlot_10";
                break;
            default:
                break;

        }
        return fileName;
    }

    public void AttemptCreateNewGame()
    {
        loadingScreen.SetActive(true);
        TitleScreen.SetActive(false);
      

        if (PlayerPrefs.HasKey("TouchSensitivity"))
        {
            PlayerCamera.instance.touchSensitivity = PlayerPrefs.GetFloat("TouchSensitivity");
        }
        if (PlayerPrefs.HasKey("FOV"))
        {
            PlayerCamera.instance.CameraFOV = PlayerPrefs.GetFloat("FOV");
            
        }







        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            StartCoroutine(LoadWorldScene());
            return;
        }


        //Check to see if we can crate a Create a New Game File
       
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);


        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            StartCoroutine(LoadWorldScene());
            return;
        }

        
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            StartCoroutine(LoadWorldScene());
            return;
        }
       
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            StartCoroutine(LoadWorldScene());
            return;
        }
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            StartCoroutine(LoadWorldScene());
            return;
        }
        TitleScreenManager.Instance.DisplayNoFreeCharacterSlots();



    }
    public void LoadGame()
    {
        //Load A Previous File
        saveFileName=DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;
        currentCharacterData = saveFileDataWriter.LoadSaveFile();

        StartCoroutine(LoadWorldScene());

        TitleScreen.SetActive(false);
        loadingScreen.SetActive(true);


    }


    //Load All Character Profile On Device When Starting Game
    private void LoadAllCharacterProfiles()
    {
        saveFileDataWriter=new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath=Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
        characterSlot01 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
        characterSlot02 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
        characterSlot03 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
        characterSlot04 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
        characterSlot05 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
        characterSlot06 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
        characterSlot07 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
        characterSlot08 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
        characterSlot09 = saveFileDataWriter.LoadSaveFile();

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
        characterSlot10 = saveFileDataWriter.LoadSaveFile();

    }
    public void SaveGame()
    {
        //Save The Current File Under A File Name Depending on Which Slot You Are Using
        saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(currentCharacterSlotBeingUsed);
        saveFileDataWriter = new SaveFileDataWriter();

        saveFileDataWriter.saveDataDirectoryPath= Application.persistentDataPath;
        saveFileDataWriter.saveFileName = saveFileName;

        player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

        saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
    }

    public void Deletegame(CharacterSlot characterSlot)
    {
        //Load A Previous File
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
        saveFileDataWriter.saveFileName=DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);
        saveFileDataWriter.DeleteSaveFile();


    }

    public IEnumerator LoadWorldScene()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
        player.LoadGameDataToCharacterData(ref currentCharacterData);

        loadingSlider.value = 0f;

        while (!loadOperation.isDone)
        {
            // Progress ranges from 0.0 to 0.9, so normalize it
            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);

            // Update the progress bar fill amount
            loadingSlider.value = progress;

            // Wait for the next frame before continuing the loop
            yield return null;
        }

        // When the scene has fully loaded, deactivate the loading screen
        loadingScreen.SetActive(false);
    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }



}
