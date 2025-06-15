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
    int MaxFrameRate = 120;
    public float TargetFrameRate = 60.0f;
    float currentFrameTime;



    [Header("Menu objects")]
    [SerializeField] GameObject titleScreenMainMenu;
    [SerializeField] GameObject titleScreenLoadMenu;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject titleScreenCharacterCreationMenu;


    [Header("Buttons")]
    [SerializeField] Button mainMenuNewGameButton;
    [SerializeField] Button loadMenuReturnButton;
    [SerializeField] Button mainMenuLoadGameButton;
    [SerializeField] Button SettingsMenuButton;
    [SerializeField] Button deleteCharacterPopUpConfirmbutton;

    [Header("Character Creation Main panel Buttions")]
    [SerializeField] Button characterNameButton;
    [SerializeField] Button characterClassButton;
    [SerializeField] Button startGameButton;

    [Header("Character Creation Secondary panel Buttons")]
    [SerializeField] GameObject characterClassMenu;
    [SerializeField] GameObject characterNameMenu;
    [SerializeField] TMP_InputField characterNameInputField;


    [Header("Character Creation Main panel Buttons")]
    
    [SerializeField] Button[] characterClassButtons;
   

    [Header("PoPups")]
    [SerializeField] GameObject noCharacterSlotPopUp;
    [SerializeField] Button noCharacterSlotPopUpOkaybutton;
    [SerializeField] GameObject deleteCharacterSlotPopUp;

    [Header("Character SLots")]
    public CharacterSlot currentSelectedCharacterSlot = CharacterSlot.N0_SLOT;

    [Header("Classes")]
    public CharacterClass[] startingClasses;



    

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
       

        WorldSaveGameManager.instance.AttemptCreateNewGame();
        CloseCharacterCreationMenu();








    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void OpenCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(true);

    }
    public void CloseCharacterCreationMenu()
    {
        titleScreenCharacterCreationMenu.SetActive(false);

    }

    public void OpenChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(false);
        characterClassMenu.SetActive(true);

        if(characterClassButtons.Length > 0)
        {
            characterClassButtons[0].Select();
            characterClassButtons[0].OnSelect(null);
        }

    }
    public void CloseChooseCharacterClassSubMenu()
    {
        ToggleCharacterCreationScreenMainMenuButtons(true);
        characterClassMenu.SetActive(false);

        characterClassButton.Select();
        characterClassButton.OnSelect(null);

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

        player.characterName = characterNameInputField.text;

    }

    private void ToggleCharacterCreationScreenMainMenuButtons(bool status)
    {
        characterNameButton.enabled = status;
        characterClassButton.enabled=status;
        startGameButton.enabled = status;

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

    public void SelectClass (int ClassID)
    {
        playerManager player = WorldSaveGameManager.instance.player;

        if (startingClasses.Length<=0)
            return;
        startingClasses[ClassID].SetClass(player);
        CloseChooseCharacterClassSubMenu();
    }
    public void PreviewClass(int ClassID)
    {
        playerManager player = WorldSaveGameManager.instance.player;

        if (startingClasses.Length<=0)
            return;
        startingClasses[ClassID].SetClass(player);
    }
    public void SetCharacterClass(playerManager player, int vitality, int stamina, int mind, int strength, 
        int dexterity, int faith, WeaponItem[] mainHandWeapons, WeaponItem[]offHandWeapons, HeadEquipmentItem headEquipment,
        BodyEquipmentItem bodyEquipment, LegEquipmentItem legEquipment, HandEquipmentItem handEquipment)
    {
        //Set Stats
        player.playerStatsManager.vitality = vitality;
        player.playerStatsManager.stamina = stamina;
        player.playerStatsManager.mind = mind;
        player.playerStatsManager.strength = strength;
        player.playerStatsManager.dexterity = dexterity;
        player.playerStatsManager.faith = faith;

        //Set Weapons
        player.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[0]);
        player.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[1]);
        player.playerInventoryManager.weaponsInRightHandSlot[0]=Instantiate(mainHandWeapons[2]);
        player.playerInventoryManager.currentRightHandWeapon = player.playerInventoryManager.weaponsInRightHandSlot[0];

        player.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[0]);
        player.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[1]);
        player.playerInventoryManager.weaponsInLeftHandSlot[0]=Instantiate(offHandWeapons[2]);
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
        }
        if (bodyEquipment !=null)
        {
            BodyEquipmentItem equipment = Instantiate(bodyEquipment);
            player.playerInventoryManager.bodyEquipment = equipment;
        }
        else
        {
            player.playerInventoryManager.bodyEquipment = null;
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
        }
        if (handEquipment !=null)
        {
            HandEquipmentItem equipment = Instantiate(handEquipment);
            player.playerInventoryManager.handEquipment = equipment;
            
        }
        else
        {
            player.playerInventoryManager.handEquipment = null;
        }

        



    }







}
