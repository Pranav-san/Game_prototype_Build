using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIEquipmentManager : MonoBehaviour
{
    
    [Header("Menu")]
    [SerializeField] GameObject menu;

    [Header("Weapon Slots")]
    [SerializeField] Image rightHandSlot01;
    [SerializeField] Image rightHandSlot02;
    [SerializeField] Image rightHandSlot03;

    [SerializeField] Image leftHandSlot01;
    [SerializeField] Image leftHandSlot02;
    [SerializeField] Image leftHandSlot03;

    [Header("Equipment Inventory")]
    [SerializeField] GameObject equipmentInventoryWindow;
    public EquipmentType currentSelectedEquipmentSlot;
    [SerializeField] Transform equipmentInventoryContentWindow;
    [SerializeField] GameObject equipmentInventorySlotPrefab;
    
    [SerializeField] Item currentSelectedItem;


    public void OpenEquipmentManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);
        equipmentInventoryWindow.SetActive(false);
        ClearEquipmentInventory();
        RefreshWeaponSlotsIcons();

    }
    public void CloseEquipmentManagerMenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menu.SetActive(false);

    }

    private void RefreshWeaponSlotsIcons()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        if (player == null)
        {
            Debug.LogError("playerManager is null. Ensure it's initialized correctly.");
            return;
        }

        //Right Hand Weapon 01
        WeaponItem rightHandWeapon01 = player.playerInventoryManager.weaponsInRightHandSlot[0];

       
        if(rightHandWeapon01.itemIcon != null)
        {
            rightHandSlot01.enabled = true;
            rightHandSlot01.sprite = rightHandWeapon01.itemIcon;
        }
        else
        {
            rightHandSlot01.enabled = false;
        }


        //Right Hand Weapon 02
        WeaponItem rightHandWeapon02 = player.playerInventoryManager.weaponsInRightHandSlot[1];

        if (rightHandWeapon02.itemIcon != null)
        {
            rightHandSlot02.enabled = true;
            rightHandSlot02.sprite = rightHandWeapon02.itemIcon;
        }
        else
        {
            rightHandSlot02.enabled = false;
        }

        //Right Hand Weapon 03
        WeaponItem rightHandWeapon03 = player.playerInventoryManager.weaponsInRightHandSlot[2];

        if (rightHandWeapon03.itemIcon != null)
        {
            rightHandSlot03.enabled = true;
            rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
        }
        else
        {
            rightHandSlot03.enabled = false;
        }

        //Left Hand Weapon 01
        WeaponItem leftHandWeapon01 = player.playerInventoryManager.weaponsInLeftHandSlot[0];


        if (leftHandWeapon01.itemIcon != null)
        {
            leftHandSlot01.enabled = true;
            leftHandSlot01.sprite = leftHandWeapon01.itemIcon;
        }
        else
        {
            leftHandSlot01.enabled = false;
        }


        //Left Hand Weapon 02
        WeaponItem leftHandWeapon02 = player.playerInventoryManager.weaponsInLeftHandSlot[1];

        if (leftHandWeapon02.itemIcon != null)
        {
            leftHandSlot02.enabled = true;
            leftHandSlot02.sprite = leftHandWeapon02.itemIcon;
        }
        else
        {
            leftHandSlot02.enabled = false;
        }

        //Left Hand Weapon 03
        WeaponItem leftHandWeapon03 = player.playerInventoryManager.weaponsInLeftHandSlot[2];

        if (leftHandWeapon03.itemIcon != null)
        {
            leftHandSlot03.enabled = true;
            leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
        }
        else
        {
            leftHandSlot03.enabled = false;
        }
    }

    private void ClearEquipmentInventory()
    {
        foreach(Transform item in equipmentInventoryContentWindow)
        {
           
            
               Destroy(item.gameObject);

            
                
        }
    }

    public void LoadEquipmentInventory()
    {
        equipmentInventoryWindow.SetActive(true);

        ClearEquipmentInventory();

        switch(currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.RightWeapon03:
                LoadWeaponInventory();
                break;    
            case EquipmentType.leftWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.leftWeapon02:
                LoadWeaponInventory();
                break;
            case EquipmentType.leftWeapon03:
                LoadWeaponInventory();
                break;
            default:
                break;
        }



    }

    private void LoadWeaponInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<WeaponItem> weaponsInInventory = new List<WeaponItem>();

       

        for(int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

            if (weapon != null)
            {
                weaponsInInventory.Add(weapon);
            }

        }

        if(weaponsInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < weaponsInInventory.Count;i++)
        {
           GameObject inventorySlotGameObject =  Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot  = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(weaponsInInventory[i]);

            //This Will Select The First Button In The List
            if(!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    public void SelectEquipmentSlot(int EquipmentSlot)
    {
        currentSelectedEquipmentSlot = (EquipmentType)EquipmentSlot;

    }

    public void UnEquipSelectedItem()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();
        Item unEquippedItem;

        switch (currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                 unEquippedItem = player.playerInventoryManager.weaponsInRightHandSlot[0];

                if(unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlot[0] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if(unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;

            case EquipmentType.RightWeapon02:
                 unEquippedItem = player.playerInventoryManager.weaponsInRightHandSlot[1];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlot[1] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex ==1)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;

            case EquipmentType.RightWeapon03:
                unEquippedItem = player.playerInventoryManager.weaponsInRightHandSlot[2];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInRightHandSlot[2] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.rightHandWeaponIndex ==2)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;

            case EquipmentType.leftWeapon01:
                 unEquippedItem = player.playerInventoryManager.weaponsInLeftHandSlot[0];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlot[0] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;

            case EquipmentType.leftWeapon02:
                unEquippedItem = player.playerInventoryManager.weaponsInLeftHandSlot[1];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlot[1] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex ==1)
                {
                    
                    player.playerInventoryManager.currentLeftHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;

            case EquipmentType.leftWeapon03:
                unEquippedItem = player.playerInventoryManager.weaponsInLeftHandSlot[2];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInLeftHandSlot[2] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.leftHandWeaponIndex ==2)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                }
                break;
        }
        //Refresh Menu
        RefreshMenu();
    }

    public void RefreshMenu()
    {
        ClearEquipmentInventory();
        RefreshWeaponSlotsIcons();

    }

    public void SelectLastSelectedEquipment()
    {
        Button lastSelectedButton = null;

        switch(currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:
                lastSelectedButton = rightHandSlot01.GetComponent<Button>();
                break;
            case EquipmentType.RightWeapon02:
                lastSelectedButton = rightHandSlot02.GetComponent<Button>();
                break;
            case EquipmentType.RightWeapon03:
                lastSelectedButton = rightHandSlot03.GetComponent<Button>();
                break;
            case EquipmentType.leftWeapon01:
                lastSelectedButton = leftHandSlot01.GetComponent<Button>();
                break;
            case EquipmentType.leftWeapon02:
                lastSelectedButton = leftHandSlot02.GetComponent<Button>();
                break;
            case EquipmentType.leftWeapon03:
                lastSelectedButton = leftHandSlot03.GetComponent<Button>();

                break;

                default:
                break;

        }
        if (lastSelectedButton != null)
        {
            lastSelectedButton.Select();
            lastSelectedButton.OnSelect(null);
        }

    }

  

 
}
