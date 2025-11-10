using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.ProBuilder.Shapes;

public class DoorLockManager : MonoBehaviour
{
    [SerializeField] GameObject passCodeMenu;
    [SerializeField] GameObject lockKeyMenu;

    [Header("Door and Lock Display ")]
    [SerializeField]Door currentDoor;
    [SerializeField] TextMeshProUGUI passCodeDisplayText;
    [SerializeField] private bool hidePasscode = false;
    [SerializeField] int maxLength = 8;

    [SerializeField] private string currentInput = "";

    [SerializeField] Button deleteButton;
    [SerializeField] Button submit;

    [Header("Key Lock Menu")]
    [SerializeField] Image requiredKeyIcon;
    [SerializeField] TextMeshProUGUI keyname;
    [SerializeField] Button useKeyButton;
    [SerializeField] private bool consumeKeyOnUse = false;
    public KeyItem requiredKeyInInventory;





    public void OpenDoorlockUI(Door door, playerManager player)
    {

        PlayerUIManager.instance.mobileControls.DisableMobileControls();

        if(door!= null && door.isLocked && !door.requireKey)
        {
            PlayerUIManager.instance.menuWindowOpen = true;
            passCodeMenu.SetActive(true);

        }
        if(door!= null && door.isLocked && door.requireKey)
        {
            PlayerUIManager.instance.menuWindowOpen = true;
            lockKeyMenu.SetActive(true);

            requiredKeyInInventory = null;
            foreach (Item item in player.playerInventoryManager.itemsInInventory)
            {
                if (item is KeyItem key && key.keyID == door.keyID)
                {
                    requiredKeyInInventory = key;
                    break;
                }
            }

            if(requiredKeyInInventory != null)
            {
                requiredKeyIcon.sprite = requiredKeyInInventory.itemIcon;
                keyname.text = requiredKeyInInventory.itemName;

            }
            else
            {
                keyname.text = door.keyName;

            }



        }
        




        currentDoor = door;
    }

    public void CloseDoorLockUI()
    {
        
        passCodeMenu.SetActive(false);
        lockKeyMenu.SetActive(false);
        PlayerUIManager.instance.mobileControls.EnableMobileControls();
        PlayerUIManager.instance.menuWindowOpen = false;
        currentDoor = null;
    }

    public void OnPasscodeButtonPressed(int input)
    {
        currentInput += input.ToString();
        WorldSoundFXManager.instance.playDoorUnlockButtonPressed();

        UpdatePassCodeDisplay();

    }

    public void OnDeleteButtonPressed()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            WorldSoundFXManager.instance.playDoorUnlockButtonPressed();
            UpdatePassCodeDisplay();
        }
    }

    public void OnSubmitPressed()
    {
        if(currentDoor!= null)
        {
            string currentDoorPassCode = currentDoor.GetDoorPassCode();

            if(currentInput.Length > 0)
            {
                if(currentInput == currentDoorPassCode)
                {
                    WorldSoundFXManager.instance.PlayCorrectPasscodeSfx();
                    currentDoor.UnlockDoor();
                    CloseDoorLockUI();
                }
                else
                {
                    currentInput = "";
                    WorldSoundFXManager.instance.PlayWrongPasscodeSfx();
                    UpdatePassCodeDisplay();
                }
            }



        }

        else
        {
            currentInput = "";
            UpdatePassCodeDisplay();
        }
    }

    public void OnUseKeyButtonPressed()
    {
        if(requiredKeyInInventory!= null)
        {
            currentDoor.UnlockDoor();
            WorldSoundFXManager.instance.PlayUseKeySfx();
            CloseDoorLockUI();
            requiredKeyInInventory =null;
        }
    }
    private void UpdatePassCodeDisplay()
    {
        passCodeDisplayText.text = currentInput;

        
    }


}
