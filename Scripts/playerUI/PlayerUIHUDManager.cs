using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHUDManager : MonoBehaviour
{

    [SerializeField] public CanvasGroup[] canvasGroup;
    
    public UI_StatBar staminaBar;
    public UI_StatBar healthBar;

    [Header("Boss Health Bar")]
    public Transform BossHealthBarParent;
    public GameObject BossHealthBar;

    [Header("Runes")]
    [SerializeField] float runeUpdateCountDelayTimer = 2.5f;
    private int PendingRunesToAdd = 0;
    private Coroutine waitThenAddRunesCoroutine;
    [SerializeField] TextMeshProUGUI runesToAddText;
    [SerializeField] TextMeshProUGUI runesCountText;

    [Header("Quick Slots")]
    
    [SerializeField] Image rightWeaponQuickSlotIcon;
    [SerializeField] Image leftWeaponQuickSlotIcon;
    [SerializeField] Image quickslotItemQuickSlotIcon;
    [SerializeField] TextMeshProUGUI quickslotItemCount;


    
    public void SetNewHealthValue(int oldValue, int newValue)
    {
        healthBar.SetStat(newValue);
    }
    
    public void SetMaxHealthValue(int maxHealth)
    {

        healthBar.SetMaxStat(maxHealth);

    }

    public void SetRunesCount(int runesToAdd)
    {
        PendingRunesToAdd += runesToAdd;

        if(waitThenAddRunesCoroutine != null)
            StopCoroutine(waitThenAddRunesCoroutine);

        waitThenAddRunesCoroutine = StartCoroutine(waitThenUpdateRuneCount());
        
    }

    private IEnumerator waitThenUpdateRuneCount()
    {
        //Wait for timer to reach 0, Increase more runes qued up
        float timer = runeUpdateCountDelayTimer;
        int runesToAdd = PendingRunesToAdd;
        runesToAddText.text = "+" + runesToAdd.ToString();
        runesToAddText.enabled = true;
        while(timer > 0)
        {
            timer -=Time.deltaTime;

            //If more runes are QuedUp, Re-Update Total New rune Count
            if(runesToAdd != PendingRunesToAdd)
            {
                runesToAdd = PendingRunesToAdd;
                runesToAddText.text = "+" + runesToAdd.ToString();
            }
            yield return null;
        }
        // Update Rune Count, Reset pending Runes Count and Hide Pending Runes Text
        runesToAddText.enabled= false;
        PendingRunesToAdd = 0;
        runesCountText.text = PlayerUIManager.instance.playerStatsManager.runes.ToString();
        yield return null;

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

    public void SetQuickSlotItemQuickSlotIcon(int itemID)
    {

        QuickSlotItem quickslotItem = WorldItemDatabase.instance.GetQuickSlotItemByID(itemID);
        if (quickslotItem == null)
        {
            Debug.Log("Item is Null");
            quickslotItemQuickSlotIcon.enabled = false;
            quickslotItemQuickSlotIcon.sprite = null;
            quickslotItemCount.enabled = false;
            return;
        }
        if (quickslotItem.itemIcon == null)
        {
            Debug.Log("Item has No ICON");
            quickslotItemQuickSlotIcon.enabled = false;
            quickslotItemQuickSlotIcon.sprite = null;
            quickslotItemCount.enabled = false;
            return;
        }

        //Update Quantity left, Show in The UI
        //Fade out icon if none left

        quickslotItemQuickSlotIcon.sprite = quickslotItem.itemIcon;
        quickslotItemQuickSlotIcon.enabled = true;

        if (quickslotItem.isConsumable)
        {
            quickslotItemCount.enabled = true;
            quickslotItemCount.text = quickslotItem.GetCurrentAmount(PlayerCamera.instance.player).ToString();

        }
        else
        {
            quickslotItemCount.enabled = false;
        }

    }



}