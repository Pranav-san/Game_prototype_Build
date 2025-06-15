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
        Item equipedItem;

        switch (PlayerUIManager.instance.playerUIEquipmentManager.currentSelectedEquipmentSlot)
        {
            case EquipmentType.RightWeapon01:

                //If our current weapon slot is not unArmed item, Add to inventory
                 equipedItem = player.playerInventoryManager.weaponsInRightHandSlot[0];
                if(equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[0] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if(player.playerInventoryManager.rightHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(0, player.playerInventoryManager.currentRightHandWeaponID);

                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            
            case EquipmentType.RightWeapon02:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.weaponsInRightHandSlot[1];
                if (equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[1] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.rightHandWeaponIndex ==1)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(1, player.playerInventoryManager.currentRightHandWeaponID);
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.RightWeapon03:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.weaponsInRightHandSlot[2];
                if (equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInRightHandSlot[2] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.rightHandWeaponIndex ==2)
                {
                    player.playerInventoryManager.currentRightHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(2, player.playerInventoryManager.currentRightHandWeaponID);
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon01:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.weaponsInLeftHandSlot[0];
                if (equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[0] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==0)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(0, player.playerInventoryManager.currentLeftHandWeaponID);
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon02:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.weaponsInLeftHandSlot[1];
                if (equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[1] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==1)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(1, player.playerInventoryManager.currentLeftHandWeaponID);
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.leftWeapon03:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.weaponsInLeftHandSlot[2];
                if (equipedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.weaponsInLeftHandSlot[2] = currentItem as WeaponItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                if (player.playerInventoryManager.leftHandWeaponIndex ==2)
                {
                    player.playerInventoryManager.currentLeftHandWeaponID = currentItem.itemID;
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(2, player.playerInventoryManager.currentLeftHandWeaponID);
                }
                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.Head:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.headEquipment;
                if (equipedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.headEquipment = currentItem as HeadEquipmentItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);

                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;
            
            case EquipmentType.Body:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.bodyEquipment;
                if (equipedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.bodyEquipment = currentItem as BodyEquipmentItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;


            case EquipmentType.Hand:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.handEquipment;
                if (equipedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.handEquipment = currentItem as HandEquipmentItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);

                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;

            case EquipmentType.Leg:

                //If our current weapon slot is not unArmed item, Add to inventory
                equipedItem = player.playerInventoryManager.legEquipment;
                if (equipedItem != null)
                {
                    player.playerInventoryManager.AddItemToInventory(equipedItem);
                }

                //Then Replace that weapon in that slot with our new weapon
                player.playerInventoryManager.legEquipment = currentItem as LegEquipmentItem;

                //Then Remove the New Weapon From our inventory
                player.playerInventoryManager.RemoveItemToInventory(currentItem);

                //Re-Equip Weapon If we holding the CurrentWeapon in this slott
                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);

                //Refresh Equipment Window 
                PlayerUIManager.instance.playerUIEquipmentManager.RefreshMenu();
                break;




        }

        PlayerUIManager.instance.playerUIEquipmentManager.SelectLastSelectedEquipment();



    }


}

