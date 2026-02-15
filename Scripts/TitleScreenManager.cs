using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading;
using static UnityEngine.GraphicsBuffer;
using TMPro;



public class TitleScreenManager : MonoBehaviour
{

    public static TitleScreenManager Instance;

    [Header("Frame Settings")]
    int MaxFrameRate = 60;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;

    [Header("Menu objects")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject titleScreenCharacterCreationMenu;

    [Header("Settings Menu Objects")]
    [SerializeField] GameObject gameSettings;
    [SerializeField] GameObject cameraSettings;
    [SerializeField] GameObject soundSettings;
    [SerializeField] GameObject graphicsSettings;



    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button loadMenuDeleteCharacterSlotButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button SettingsMenuButton;
    [SerializeField] Button deleteCharacterPopUpConfirmbutton;

    [Header("Character Creation Main panel Buttons")]
    [SerializeField] Button characterNameButton;
    [SerializeField] TextMeshProUGUI characterNameText;
    [SerializeField] Button characterClassButton;
    [SerializeField] Button characterSexButton;
    [SerializeField] TextMeshProUGUI characterSexText;
    [SerializeField] Button CharacterHairButton;
    [SerializeField]bool isMale = true;
    [SerializeField] Button startGameButton;

    [Header("Character Creation Secondary panel Buttons")]
    [SerializeField] GameObject characterClassMenu;
    MenuSlideAnimator characterClassMenuAnimator;
    [SerializeField] GameObject characterNameMenu;
    [SerializeField] GameObject characterHairMenu;
    MenuSlideAnimator characterHairMenuAnimator;
    [SerializeField] TMP_InputField characterNameInputField;


    [Header("Character Creation Main panel Buttons")]
    [SerializeField] Button[] characterClassButtons;
    [SerializeField] Button[] characterHairButtons;
   

    [Header("PoPups")]
    [SerializeField] GameObject noCharacterSlotPopUp;
    [SerializeField] Button noCharacterSlotPopUpOkaybutton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character SLots")]
    public CharacterSlot currentSelectedCharacterSlot = CharacterSlot.NO_SLOT;
    


    [Header("Classes")]
    public CharacterClass[] startingClasses;
    [SerializeField] int currentSelectedClassID = -1;

    [Header("Settings")]
    [SerializeField] Color defaultSettingMenuTextColor;
    [SerializeField] Color selectedSettingMenuTextColor;
    [SerializeField] TextMeshProUGUI gameSettingsButtonText;
    [SerializeField] TextMeshProUGUI cameraSettingsButtonText;
    [SerializeField] TextMeshProUGUI soundSettingsButtonText;
    [SerializeField] TextMeshProUGUI graphicsSettingsButtonText;



    
   

    private void OnEnable()
    {
        

    }



    private void Start()
    {
        WorldSaveGameManager.instance.DisableMobileControlsOnSceneChange();


        for (int i = 0; i< PlayerUIManager.instance.playerUIHUDManager.canvasGroup.Length; i++)
        {
            PlayerUIManager.instance.playerUIHUDManager.canvasGroup[i].alpha = 0f;

        }

        //WorldSoundFXManager.instance.PlayTitleScreenMusic();
       






    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
          
           
            
        }
        else
        {
            Destroy(gameObject);
        }










        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = MaxFrameRate;

        characterClassMenuAnimator = characterClassMenu.GetComponent<MenuSlideAnimator>();
        characterHairMenuAnimator = characterHairMenu.GetComponent<MenuSlideAnimator>();


       


    }

    

    public void AttemptToCreateNewCharacter()
    {
        if (WorldSaveGameManager.instance.HasFreeCharacterSlot())
        {
            OpenCharacterCreationMenu();

        }
        else
        {
            DisplayNoFreeCharacterSlots();
        }

    }

    public void StartNewGame()
    {
        playerManager player = WorldSaveGameManager.instance.player;
        string inputName = characterNameInputField?.text;

        //Set A Name For Character If Its Null
        if (string.IsNullOrWhiteSpace(inputName))
        {
            string saveFileName = "Log";
            characterNameInputField.text = saveFileName;
            player.characterName = characterNameInputField.text;

            player.ismale  = isMale;

        }

        WorldSaveGameManager.instance.AttemptCreateNewGame();

        //Assign Character Info in Save Slot
        player.characterName = characterNameInputField.text;
        player.ismale  = isMale;



        CloseCharacterCreationMenu();
        








        for (int i = 0; i< PlayerUIManager.instance.playerUIHUDManager.canvasGroup.Length; i++)
        {
            PlayerUIManager.instance.playerUIHUDManager.canvasGroup[i].alpha = 1f;

        }








    }

    public void ContinueGame()
    {
        if (!PlayerPrefs.HasKey("LastUsedSlot"))
        {
            DisplayNoFreeCharacterSlots();
            return;
        }

        CharacterSlot lastSlot = (CharacterSlot)PlayerPrefs.GetInt("LastUsedSlot");

        WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = lastSlot;

        // Check if file actually exists
        SaveFileDataWriter writer = new SaveFileDataWriter();
        writer.saveDataDirectoryPath = Application.persistentDataPath;
        writer.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(lastSlot);

        if (!writer.CheckToSeeIfFileExists())
        {
            DisplayNoFreeCharacterSlots();
            return;
        }

        WorldSaveGameManager.instance.LoadGame();

    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenCharacterCreationMenu()
    {
        PlayerCamera.instance.player.transform.position = PlayerCamera.instance.player.defaultPlayerposition;
        PlayerCamera.instance.player.transform.rotation = Quaternion.identity;
        titleScreenCharacterCreationMenu.SetActive(true);


        isMale = true;
        characterSexText.text = "Male";
        PlayerInputManager.Instance.player.playerAnimatorManager.PlayTargetActionAnimationInstantly("Empty", false);
        PlayerCamera.instance.player.playerBodyManager.ToggleBodyType(isMale);
        


    }
    public void CloseCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(false);
        CloseChooseCharacterClassSubMenu();
        CloseChooseNameSubMenu();
        PlayerInputManager.Instance.player.transform.position =PlayerInputManager.Instance.player.titleScreenPlayerPosition;


    }

    public void ReturnToTitleScreenMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(false);
        CloseChooseCharacterClassSubMenu();
        CloseChooseNameSubMenu();
        PlayerInputManager.Instance.player.transform.position =PlayerInputManager.Instance.player.titleScreenPlayerPosition;
        PlayerInputManager.Instance.player.transform.rotation = PlayerInputManager.Instance.player.titleScreenPlayerRotation;
        PlayerInputManager.Instance.player.playerAnimatorManager.PlayTargetActionAnimationInstantly("Title Screen Animation", false);



    }

    public void OpenChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);
        characterClassMenu.SetActive(true);
        if(characterClassMenuAnimator != null)
        {
            characterClassMenuAnimator.ShowMenu();
        }
        

        //if(characterClassButtons.Length > 0)
        //{
        //    characterClassButtons[0].Select();
        //    characterClassButtons[0].OnSelect(null);
        //}

    }
    public void CloseChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);
        characterClassMenu.SetActive(false);

    }

    public void OpenChooseHairStyleSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);
        characterHairMenu.SetActive(true);

        if (characterHairMenuAnimator != null)
        { 
            characterHairMenuAnimator.ShowMenu(); 
        }

        if (characterHairButtons.Length > 0)
        {
            characterHairButtons[0].Select();
            characterHairButtons[0].OnSelect(null);
        }



    }

    public void CloseChooseHairStyleSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);
        characterHairMenu.SetActive(false);

        CharacterHairButton.Select(); 
        CharacterHairButton.OnSelect(null);


    }

    public void OpenChooseNameSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);

        characterNameButton.gameObject.SetActive(false);
        characterNameMenu.SetActive(true);

        characterNameInputField.Select();

    }

    public void CloseChooseNameSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);

        characterNameButton.gameObject.SetActive(true);
        characterNameMenu.SetActive(false);

        characterNameButton.Select();

        playerManager player = WorldSaveGameManager.instance.player;

        characterNameText.text = characterNameInputField.text;

        player.characterName = characterNameInputField.text;

    }

    private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
    {
        characterNameButton.enabled = status;
        characterClassButton.enabled=status;
        CharacterHairButton.enabled=status;
        startGameButton.enabled = status;

    }

    public void OpenLoadGameMenu()
    {
        //Close Main Menu
        titleScreenMainMenu.SetActive(false);

        //Open Load Menu
        titleScreenLoadMenu.SetActive(true);

        //loadMenuReturnButton.Select();

    }

    public void CloseLoadGameMenu()
    {
        //Open Main Menu
        titleScreenLoadMenu.SetActive(false);

        //Close Load Menu
        titleScreenMainMenu.SetActive(true);

        currentSelectedCharacterSlot = CharacterSlot.NO_SLOT;


    }



    // Game Settings

    public void OpenSettingsMenu()
    {
       
        PlayerUIManager.instance.settingsManager.OpenMenu();



    }
    public void CloseSettingsMenu()
    {

        PlayerUIManager.instance.settingsManager.CloseMenu();
        
        

    }

    public void OpenGameSettingsMenu()
    {
        gameSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = selectedSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseGameSettingsMenu()
    {
        gameSettings.SetActive(false);
    }

    public void OpenCameraSettingsMenu()
    {
        cameraSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = selectedSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseCameraSettingsMenu()
    {
        cameraSettings.SetActive(false);
    }

    public void OpenSoundSettingsMenu()
    {
        soundSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = selectedSettingMenuTextColor;
        graphicsSettingsButtonText.color = defaultSettingMenuTextColor;
    }

    public void CloseSoundSettingsMenu()
    {
        soundSettings.SetActive(false);
    }

    public void OpenGraphicsSettingsMenu()
    {
        graphicsSettings.SetActive(true);
        WorldSoundFXManager.instance.PlaySettingCategoryClickSFX();

        gameSettingsButtonText.color = defaultSettingMenuTextColor;
        cameraSettingsButtonText.color = defaultSettingMenuTextColor;
        soundSettingsButtonText.color = defaultSettingMenuTextColor;
        graphicsSettingsButtonText.color = selectedSettingMenuTextColor;
    }

    public void CloseGraphicsSettingsMenu()
    {
        graphicsSettings.SetActive(false);
    }

    public void CloseAllSettingsMenu()
    {
        CloseGameSettingsMenu();
        CloseCameraSettingsMenu();
        CloseSoundSettingsMenu();
        CloseGraphicsSettingsMenu();

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
        currentSelectedCharacterSlot = CharacterSlot.NO_SLOT;
    }

    public void DeselectSlot()
    {
        currentSelectedCharacterSlot = CharacterSlot.NO_SLOT;

    }
    public void AttemptToDeleteCharacterSlot()
    {
        if(currentSelectedCharacterSlot!=CharacterSlot.NO_SLOT)
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
        

    }
    public void CloseDeleteCharacterSlotPopUp()
    {
        deleteCharacterSlotPopUp.SetActive(false);
        loadMenuReturnButton.Select();

    }

    public void SelectClass (int ClassID)
    {
        currentSelectedClassID = ClassID;

        playerManager player = WorldSaveGameManager.instance.player;

        if (startingClasses.Length<0)
            return;
        startingClasses[ClassID].SetClass(player);
        CloseChooseCharacterClassSubMenu();
    }

    public void ToggleBodyType()
    {
        if (isMale)
        {
            isMale = false;
            PlayerCamera.instance.player.playerBodyManager.ToggleBodyType(isMale);
            characterSexText.text = "Female";

            if (currentSelectedClassID >= 0 && currentSelectedClassID < startingClasses.Length)
            {
                startingClasses[currentSelectedClassID].SetClass(WorldSaveGameManager.instance.player);
            }
        }
        else
        {
            isMale = true;
            PlayerCamera.instance.player.playerBodyManager.ToggleBodyType(isMale);
            characterSexText.text = "Male";

        }

    }
    public void PreviewClass(int classID)
    {
        playerManager player = WorldSaveGameManager.instance.player;

        if (startingClasses.Length<0)
            return;
        startingClasses[classID].SetClass(player);

        
    }
    public void SetCharacterClass(playerManager player, int vitality, int endurance, int mind, int strength, 
        int dexterity, int luck, WeaponItem[] mainHandWeapons, WeaponItem[]offHandWeapons, HeadEquipmentItem headEquipment,
        BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment)
    {
        //Set Stats
        player.playerStatsManager.vitality = vitality;
        player.playerStatsManager.endurance = endurance;
        player.playerStatsManager.strength = strength;
        player.playerStatsManager.dexterity = dexterity;
        player.playerStatsManager.luck = luck;

        //Set Weapons
        player.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[0]);
        player.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[1]);
        //character.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[2]);
        player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlot[0];

        player.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[0]);
        player.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[1]);
        //character.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[2]);
        player.playerInventoryManager.currentLeftHandWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[0];

        //Set Armor
        if(headEquipment !=null)
        {
            HeadEquipmentItem equipment = Instantiate(headEquipment);
            player.playerInventoryManager.headEquipment = equipment;
            player.playerEquipmentManager.LoadHeadEquipment(equipment);
        }
        else
        {
            player.playerInventoryManager.headEquipment = null; 
            player.playerEquipmentManager.LoadHeadEquipment(null);
        }
        if (bodyEquipment !=null)
        {
            BodyEquipmentItem equipment = Instantiate(bodyEquipment);
            player.playerInventoryManager.bodyEquipment = equipment;
            player.playerEquipmentManager.LoadBodyEquipment(equipment);

        }
        else
        {
            player.playerInventoryManager.bodyEquipment = null;
            player.playerEquipmentManager.LoadBodyEquipment(null);
        }
        if (legEquipment !=null)
        {
            LegEquipmentItem equipment = Instantiate(legEquipment);
            player.playerInventoryManager.legEquipment = equipment;
            player.playerEquipmentManager.LoadLegEquipment(equipment);
        }
        else
        {
            player.playerInventoryManager.legEquipment = null;
            player.playerEquipmentManager.LoadLegEquipment(null);  
        }
        if (handEquipment !=null)
        {
            HandEquipmentItem equipment = Instantiate(handEquipment);
            player.playerInventoryManager.handEquipment = equipment;
            
        }
        else
        {
            player.playerInventoryManager.handEquipment = null;
            player.playerEquipmentManager.LoadHeadEquipment(null);
        }

        



    }



    //Character Hair

    public void SelectHair(int hairID)
    {
        

        playerManager player = WorldSaveGameManager.instance.player;

        player.playerBodyManager.ToggleHairType(hairID);


        CloseChooseHairStyleSubMenu();
    }

    public void PreviewHair(int hairId)
    {
        playerManager player = WorldSaveGameManager.instance.player;
        player.playerBodyManager.ToggleHairType(hairId);

       


    }








}
