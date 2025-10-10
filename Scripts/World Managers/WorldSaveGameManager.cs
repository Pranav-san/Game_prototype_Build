using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSaveGameManager : MonoBehaviour
{

    [SerializeField] public  playerManager player;
    [SerializeField] public CanvasGroup[] mobileControls;

    [Header("Save/load")]
    [SerializeField] bool saveGame;
    [SerializeField] bool loadGame;
    [SerializeField] UniversalRenderPipelineAsset urpAsset;


    [Header("Loading Screen UI")]
    


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

    [Header("Stage ID")]
    public int namelessKnightStageID = 0;

    [Header("Dialogues")]
    [SerializeField] List<CharacterDialogue> namelessKinghtDialogues = new List<CharacterDialogue>();
    

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
        urpAsset = (UniversalRenderPipelineAsset)GraphicsSettings.defaultRenderPipeline;
        LoadAllCharacterProfiles();
    }

    public bool HasFreeCharacterSlot()
    {
        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        return true;


        //Check to see if we can crate a Create a New Game File

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);


        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;


        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;
        
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
            return true;
       

        return false;

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



        PlayerCamera.instance.touchSensitivity = GameSettingsData.TouchSensitivity;
        PlayerCamera.instance.CameraFOV = GameSettingsData.FOV;
        
        float safeRenderScale = Mathf.Clamp(GameSettingsData.RenderScale, 0.1f, 1f);
        urpAsset.renderScale = safeRenderScale;








        saveFileDataWriter = new SaveFileDataWriter();
        saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            LoadWorldScene();
            
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
            LoadWorldScene();
           

            return;
        }

        
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            LoadWorldScene();
           
            return;
        }
       
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            LoadWorldScene();
            
            return;
        }
        saveFileDataWriter.saveFileName = DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

        if (!saveFileDataWriter.CheckToSeeIfFileExists())
        {
            currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
            currentCharacterData = new CharacterSaveData();
            currentCharacterData.currentStamina = player.characterStatsManager.maxStamina; // Initialize to max value
            currentCharacterData.currentHealth = player.characterStatsManager.maxHealth;
            LoadWorldScene();
           
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

        LoadWorldScene();





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

    public void LoadWorldScene()
    {

        PlayerUIManager.instance.playerUILoadingScreenManager.ActivateLoadingScreen();

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

        loadOperation.completed += (AsyncOperation op) =>
        {




            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                player.transform.position = player.defaultPlayerposition;
            }
            else
            {
                player.LoadGameDataToCharacterData(ref currentCharacterData);


            }

            // When the scene has fully loaded, Enable Mobile Controls
           



        };
       
    }

    public void DisableMobileControlsOnSceneChange()
    {
        for (int i = 0; i <mobileControls.Length; i++)
        {
            mobileControls[i].alpha=0;
            mobileControls[i].interactable = false;
            mobileControls[i].blocksRaycasts = false;


        }

    }

    public void EnableMobileControlsOnSceneChange()
    {
        for (int i = 0; i <mobileControls.Length; i++)
        {
            mobileControls[i].alpha=1;
            mobileControls[i].interactable = true;
            mobileControls[i].blocksRaycasts = true;


        }


    }
    public int GetWorldSceneIndex()
    {
        return worldSceneIndex;
    }

    public SerializableWeapon GetSerializableWeaponFromWeaponItem(WeaponItem weapon)
    {
        SerializableWeapon serializableWeapon = new SerializableWeapon();

        //Get Weapon ID
        serializableWeapon.itemID = weapon.itemID;

        return serializableWeapon;


    }


    //Dialouge
    public CharacterDialogue GetCharacterDialogueByEnum(characterDialogueID characterDialogueID)
    {
        CharacterDialogue dialogue = null;
        switch(characterDialogueID)
        {
            case characterDialogueID.NoDialogueID:
                break;
            case characterDialogueID.NameLessKnightDialogueID:
                dialogue = FindCharacterDialogueByStageID(namelessKnightStageID, namelessKinghtDialogues);
                break;
            default:
                break;
        }

        if(dialogue!=null)
            dialogue = Instantiate(dialogue);

        return dialogue;
    }


    private CharacterDialogue FindCharacterDialogueByStageID(int stageID, List<CharacterDialogue>dialogueList)
    {
        CharacterDialogue dialogue = null;


        for(int i = 0; i< dialogueList.Count; i++)
        {
            if (dialogueList[i] == null)
                continue;
            if (dialogueList[i].requiredStageID == stageID)
            {
                dialogue = dialogueList[i];
                break;
            }
        }
        return dialogue;

    }

    public void SetStageOfDialogue(characterDialogueID CharacterDialogue, int stageIndex)
    {
        switch (CharacterDialogue)
        {
            case characterDialogueID.NoDialogueID:
                break;
            case characterDialogueID.NameLessKnightDialogueID:
                namelessKnightStageID = stageIndex;
                break;
            default:
                break;

        }

    }



}
