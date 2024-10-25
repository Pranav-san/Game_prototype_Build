using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Character_Save_Slot : MonoBehaviour
{
    SaveFileDataWriter saveFileWriter;

    [Header("Game Slot")]
    public CharacterSlot characterSlot;

    [Header("Character info")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI timePlayed;

    private void OnEnable()
    {
        LoadSaveSlots();
        
    }
    private void LoadSaveSlots()
    {
        saveFileWriter = new SaveFileDataWriter();
        saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

        /*switch(characterSlot)
        {
            case CharacterSlot.CharacterSlot_01:
                break;
            case CharacterSlot.CharacterSlot_02:
                break;
            case CharacterSlot.CharacterSlot_03:
                break;
            case CharacterSlot.CharacterSlot_04:
                break;
            case CharacterSlot.CharacterSlot_05:
                break;
            case CharacterSlot.CharacterSlot_06:
                break;
            case CharacterSlot.CharacterSlot_07:
                break;
            case CharacterSlot.CharacterSlot_08:
                break;
            case CharacterSlot.CharacterSlot_09:
                break;
            case CharacterSlot.CharacterSlot_10:
                break;
            default: 
                break;
        }*/

        //Save Slot 01
        if(characterSlot == CharacterSlot.CharacterSlot_01)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);
            
            //If File Exists, get information from it
            if(saveFileWriter.CheckToSeeIfFileExists() )
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 02
        else if (characterSlot == CharacterSlot.CharacterSlot_02)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 03
        else if (characterSlot == CharacterSlot.CharacterSlot_03)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 04
        else if (characterSlot == CharacterSlot.CharacterSlot_04)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 05
        else if (characterSlot == CharacterSlot.CharacterSlot_05)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 06
        else if (characterSlot == CharacterSlot.CharacterSlot_06)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 07
        else if (characterSlot == CharacterSlot.CharacterSlot_07)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 08
        else if (characterSlot == CharacterSlot.CharacterSlot_08)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 09
        else if (characterSlot == CharacterSlot.CharacterSlot_09)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        //Save Slot 10
        else if (characterSlot == CharacterSlot.CharacterSlot_10)
        {
            saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNamebasedOnCharacterSlotBeingUsed(characterSlot);

            //If File Exists, get information from it
            if (saveFileWriter.CheckToSeeIfFileExists())
            {
                characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

    }

    public void LoadGameFromCharacterSlots()
    {
        WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
        WorldSaveGameManager.instance.LoadGame();
        
    }
    public void SelectCurrentSlot()
    {
        TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
    }

}