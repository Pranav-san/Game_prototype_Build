using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHUDManager : MonoBehaviour
{

    [SerializeField] CanvasGroup[] canvasGroup;
    
    public UI_StatBar staminaBar;
    public UI_StatBar healthBar;

    [Header("Survial Stats")]
    //public UI_StatBar bodyTemperatureMeter;
    //public UI_StatBar thirst;
    //public UI_StatBar Hunger;
    //public UI_StatBar fatigue;

    [Header("Quick Slots")]
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;


    
    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }
    
    public void SetMaxHealthValue(int maxHealth)
    {

        healthBar.SetMaxStat(maxHealth);

    }

    public void ToggleHUD(bool status)
    {
        if (status)
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha= 1.0f;
            }

        }

        
        else
        {
            foreach (var canvas in canvasGroup)
            {
                canvas.alpha= 0.0f;
            }
        }

    }


   


    public void SetNewStaminaValue(int oldValue, int newValue)
    {
        staminaBar.SetStat(newValue);
    }
    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetStat(maxStamina);


    }



    public void SetRightweaponQuickSlotIcon(int weaponID)
    {

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weapon == null)
        {
            Debug.Log("Item is Null");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }
        if(weapon.itemIcon == null)
        {
            Debug.Log("Item has No ICON");
            rightWeaponQuickSlotIcon.enabled = false;
            rightWeaponQuickSlotIcon.sprite = null;
            return;
        }

        rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        rightWeaponQuickSlotIcon.enabled = true;

    }

    public void SetLeftweaponQuickSlotIcon(int weaponID)
    {

        WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
        if (weapon == null)
        {
            Debug.Log("Item is Null");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }
        if (weapon.itemIcon == null)
        {
            Debug.Log("Item has No ICON");
            leftWeaponQuickSlotIcon.enabled = false;
            leftWeaponQuickSlotIcon.sprite = null;
            return;
        }

        leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        leftWeaponQuickSlotIcon.enabled = true;

    }



}