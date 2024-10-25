using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipmentInventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public Image highlightedItemIcon;
    [SerializeField] GameObject equipButton;
    [SerializeField] public Item currentItem;

    public void AddItem(Item item)
    {
        currentItem = item;

        if (currentItem == null)
        {
            itemIcon.enabled = false;
            return;

        }
        itemIcon.enabled = true;

        
        itemIcon.sprite = item.itemIcon;

    }

    public void SelectSlot()
    {
        highlightedItemIcon.enabled = true;
        equipButton.SetActive(true);
    }

    public void DeSelectSlot()
    {
        highlightedItemIcon.enabled = false;
        equipButton.SetActive(false);
    }

    public void EquipItem()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();
        WeaponItem currentWeapon;

        switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:

                //If our current weapon in this slot is not unArmed item, Add to inventory
                 currentWeapon = player.playerInventoryManager.weaponsInRightHandSlot[0];
                if(currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[0] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if(player.playerInventoryManager.rightHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            
            case EquipmentType.RightWeapon02:

                //If our current weapon in tis slot is not unArmed item, Add to inventory
                currentWeapon = player.playerInventoryManager.weaponsInRightHandSlot[1];
                if (currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[1] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.rightHandWeaponIndex ==1)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.RightWeapon03:

                //If our current weapon in tis slot is not unArmed item, Add to inventory
                currentWeapon = player.playerInventoryManager.weaponsInRightHandSlot[2];
                if (currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[2] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.rightHandWeaponIndex ==2)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon01:

                //If our current weapon in tis slot is not unArmed item, Add to inventory
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[0];
                if (currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[0] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon02:

                //If our current weapon in tis slot is not unArmed item, Add to inventory
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[1];
                if (currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[1] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==1)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon03:

                //If our current weapon in tis slot is not unArmed item, Add to inventory
                currentWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[2];
                if (currentWeapon.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(currentWeapon);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[2] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==3)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;




        }

        PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipment();



    }


}

