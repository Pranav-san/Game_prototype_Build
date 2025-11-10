using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIEquipmentManager : PlayerUIMenu
{

    [Header("Weapon Slots")]
    [SerializeField] Image rightHandSlot01;
    [SerializeField] Image rightHandSlot02;
    //[SerializeField] Image rightHandSlot03;

    [SerializeField] Image leftHandSlot01;
    [SerializeField] Image leftHandSlot02;
    //[SerializeField] Image leftHandSlot03;

    [SerializeField] Image TwoHandWeaponSlot;

    [Header("Armor Slots")]
    [SerializeField] Image headEquipmentSlot;
    [SerializeField] Image bodyEquipmentSlot;
    [SerializeField] Image handEquipmentSlot;
    [SerializeField] Image legEquipmentSlot;

    [Header("QuickSlot Item")]
    [SerializeField] Image quickSlotItem_01;
    [SerializeField] TextMeshProUGUI quickSlotItem01Count;
    [SerializeField] Image quickSlotItem_02;
    [SerializeField] TextMeshProUGUI quickSlotItem02Count;

    [Header("Highlight Weapon Selected Slot")]
    public Image highlightedRightItemIcon01;
    public Image highlightedRightItemIcon02;
    //public Image highlightedRightItemIcon03;
    
    public Image highlightedLeftItemIcon01;
    public Image highlightedLeftItemIcon02;
    //public Image highlightedLeftItemIcon03;

    public Image highlightedTwoHandItemIcon01;

    [Header("Highlight Armor Selected Slot")]
    public Image highlightedHeadEquipmentItemIcon;
    public Image highlightedBodyEquipmentItemIcon;
    public Image highlightedHandEquipmentItemIcon;
    public Image highlightedLegEquipmentItemIcon;

    [Header("Highlight QuickSlot Item Selected Slot")]
    public Image highlightedQuickSlotIcon01;
    public Image highlightedQuickSlotIcon02;
    

    [Header("Weapon unEquipButton ")]
    public GameObject unEquipButtonRightItem01;
    public GameObject unEquipButtonRightItem02;
    //public GameObject unEquipButtonRightItem03;

    public GameObject unEquipButtonLeftItem01;
    public GameObject unEquipButtonLeftItem02;
    //public GameObject unEquipButtonLeftItem03;

    public GameObject unEquipTwoHandItem01;

    [Header("Armor unEquipButton ")]
    public GameObject unEquipButtonHeadEquipmentItem;
    public GameObject unEquipButtonBodyEquipmentItem;
    public GameObject unEquipButtonHandEquipmentItem;
    public GameObject unEquipButtonLegEquipmentItem;

    [Header("QuickSlot Item unEquipButton ")]
    public GameObject unEquipButtonQuickSlotItem01;
    public GameObject unEquipButtonQuickSlotItem02;




    [Header("Equipment Inventory")]
    [SerializeField] GameObject equipmentInventoryWindow;
    public EquipmentType currentSelectedEquipmentSlot;
    [SerializeField] Transform equipmentInventoryContentWindow;
    [SerializeField] GameObject equipmentInventorySlotPrefab;
    
    [SerializeField] Item currentSelectedItem;


    public override void OpenMenu()
    {
        base.OpenMenu();

        equipmentInventoryWindow.SetActive(false);
        ClearEquipmentInventory();
        RefreshWeaponSlotsIcons();


    }
    

    public void RefreshWeaponSlotsIcons()
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
        //WeaponItem rightHandWeapon03 = character.playerInventoryManager.weaponsInRightHandSlot[2];

        //if (rightHandWeapon03.itemIcon != null)
        //{
        //    rightHandSlot03.enabled = true;
        //    rightHandSlot03.sprite = rightHandWeapon03.itemIcon;
        //}
        //else
        //{
        //    rightHandSlot03.enabled = false;
        //}


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
        //WeaponItem leftHandWeapon03 = character.playerInventoryManager.weaponsInLeftHandSlot[2];

        //if (leftHandWeapon03.itemIcon != null)
        //{
        //    leftHandSlot03.enabled = true;
        //    leftHandSlot03.sprite = leftHandWeapon03.itemIcon;
        //}
        //else
        //{
        //    leftHandSlot03.enabled = false;
        //}

        WeaponItem twoHandWeapon = player.playerInventoryManager.weaponsInTwoHandSlot[0];

        if (twoHandWeapon != null && twoHandWeapon.itemIcon != null)
        {
            TwoHandWeaponSlot.enabled = true;
            TwoHandWeaponSlot.sprite = twoHandWeapon.itemIcon;
        }
        else
        {
            TwoHandWeaponSlot.enabled = false;
        }

        //Head Equipment
        HeadEquipmentItem headEquipment = player.playerInventoryManager.headEquipment;

        if (headEquipment != null)
        {
            headEquipmentSlot.enabled = true;
            headEquipmentSlot.sprite = headEquipment.itemIcon;
        }
        else
        {
            headEquipmentSlot.enabled = false;
        }

        //Body Equipment
        BodyEquipmentItem bodyEquipment = player.playerInventoryManager.bodyEquipment;

        if (bodyEquipment != null)
        {
            bodyEquipmentSlot.enabled = true;
            bodyEquipmentSlot.sprite = bodyEquipment.itemIcon;
        }
        else
        {
            bodyEquipmentSlot.enabled = false;
        }

        //Hand Equipment
        HandEquipmentItem handEquipment = player.playerInventoryManager.handEquipment;

        if (handEquipment != null)
        {
            handEquipmentSlot.enabled = true;
            handEquipmentSlot.sprite = handEquipment.itemIcon;
        }
        else
        {
            handEquipmentSlot.enabled = false;
        }

        //Leg Equipment
        LegEquipmentItem legEquipment = player.playerInventoryManager.legEquipment;

        if (legEquipment != null)
        {
            legEquipmentSlot.enabled = true;
            legEquipmentSlot.sprite = legEquipment.itemIcon;
        }
        else
        {
            legEquipmentSlot.enabled = false;
        }

        QuickSlotItem QuickSlotEquipment = player.playerInventoryManager.QuickSlotItemsInQuickSlot[0];
        if (QuickSlotEquipment != null && QuickSlotEquipment.itemIcon != null)
        {
            quickSlotItem_01.enabled = true;
            quickSlotItem_01.sprite = QuickSlotEquipment.itemIcon;

            if (QuickSlotEquipment.isConsumable)
            {
                quickSlotItem01Count.enabled = true;
                quickSlotItem01Count.text = QuickSlotEquipment.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlotItem_01.enabled = false;

            }
        }
        else
        {
            quickSlotItem_01.enabled = false;
            quickSlotItem01Count.enabled = false;
        }

        // Quick Slot Item 02
        QuickSlotItem QuickSlotEquipment02 = player.playerInventoryManager.QuickSlotItemsInQuickSlot[1];
        if (QuickSlotEquipment02 != null && QuickSlotEquipment02.itemIcon != null)
        {
            quickSlotItem_02.enabled = true;
            quickSlotItem_02.sprite = QuickSlotEquipment02.itemIcon;

            if (QuickSlotEquipment02.isConsumable)
            {
                quickSlotItem02Count.enabled = true;
                quickSlotItem02Count.text = QuickSlotEquipment02.GetCurrentAmount(player).ToString();
            }
            else
            {
                quickSlotItem_02.enabled = false;

            }
        }
        else
        {
            quickSlotItem_02.enabled = false;
            quickSlotItem02Count.enabled = false;
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
            //case EquipmentType.RightWeapon03:
            //    LoadWeaponInventory();
            //    break;    
            case EquipmentType.leftWeapon01:
                LoadWeaponInventory();
                break;
            case EquipmentType.leftWeapon02:
                LoadWeaponInventory();
                break;
            //case EquipmentType.leftWeapon03:
            //    LoadWeaponInventory();
            //    break;

            case EquipmentType.TwoHandWeapon:
                LoadTwoHandWeaponInventory();
                break;

            case EquipmentType.Head:
                LoadHeadEquipmentInventory();
                break;
            case EquipmentType.Body:
                LoadBodyEquipmentInventory();
                break;
            case EquipmentType.Hand:
                LoadHandEquipmentInventory();
                break;
            case EquipmentType.Leg:
                LoadLegEquipmentInventory();
                break;
            case EquipmentType.QuickSlot01:
                LoadQuickSlotInventory();
                break;
            case EquipmentType.QuickSlot02:
                LoadQuickSlotInventory();
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

            if (weapon != null && weapon.weaponWieldingType == WeaponWieldingType.OneHand)
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

    private void LoadTwoHandWeaponInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<WeaponItem> weaponsInInventory = new List<WeaponItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            WeaponItem weapon = player.playerInventoryManager.itemsInInventory[i] as WeaponItem;

            if (weapon != null && weapon.weaponWieldingType == WeaponWieldingType.TwoHand)
            {
                weaponsInInventory.Add(weapon);
            }

        }

        if (weaponsInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < weaponsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(weaponsInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    private void LoadHeadEquipmentInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<HeadEquipmentItem> HeadEquipmentInInventory = new List<HeadEquipmentItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            HeadEquipmentItem HeadEquipment = player.playerInventoryManager.itemsInInventory[i] as HeadEquipmentItem;

            if (HeadEquipment != null)
            {
                HeadEquipmentInInventory.Add(HeadEquipment);
            }

        }

        if (HeadEquipmentInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < HeadEquipmentInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(HeadEquipmentInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    private void LoadBodyEquipmentInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<BodyEquipmentItem> BodyEquipmentInInventory = new List<BodyEquipmentItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            BodyEquipmentItem BodyEquipment = player.playerInventoryManager.itemsInInventory[i] as BodyEquipmentItem;

            if (BodyEquipment != null)
            {
                BodyEquipmentInInventory.Add(BodyEquipment);
            }

        }

        if (BodyEquipmentInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < BodyEquipmentInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(BodyEquipmentInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    private void LoadLegEquipmentInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<LegEquipmentItem> LegEquipmentInInventory = new List<LegEquipmentItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            LegEquipmentItem LegEquipment = player.playerInventoryManager.itemsInInventory[i] as LegEquipmentItem;

            if (LegEquipment != null)
            {
                LegEquipmentInInventory.Add(LegEquipment);
            }

        }

        if (LegEquipmentInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < LegEquipmentInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(LegEquipmentInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    private void LoadHandEquipmentInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<HandEquipmentItem> HandEquipmentInInventory = new List<HandEquipmentItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            HandEquipmentItem HandEquipment = player.playerInventoryManager.itemsInInventory[i] as HandEquipmentItem;

            if (HandEquipment != null)
            {
                HandEquipmentInInventory.Add(HandEquipment);
            }

        }

        if (HandEquipmentInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < HandEquipmentInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(HandEquipmentInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
            {
                hasSelectedFirstInventorySlot = true;
                Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
                inventorySlotButton.Select();
                inventorySlotButton.OnSelect(null);
            }
        }

    }

    private void LoadQuickSlotInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<QuickSlotItem> quickSlotItemsInInventory = new List<QuickSlotItem>();



        for (int i = 0; i < player.playerInventoryManager.itemsInInventory.Count; i++)
        {
           QuickSlotItem quickSlotItem = player.playerInventoryManager.itemsInInventory[i] as QuickSlotItem;

            if (quickSlotItem != null)
            {
                quickSlotItemsInInventory.Add(quickSlotItem);
            }

        }

        if (quickSlotItemsInInventory.Count <= 0)
        {
            RefreshMenu();
            return;

        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < quickSlotItemsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(equipmentInventorySlotPrefab, equipmentInventoryContentWindow);
            UI_EquipmentInventorySlot equipmentInventorySlot = inventorySlotGameObject.GetComponent<UI_EquipmentInventorySlot>();
            equipmentInventorySlot.AddItem(quickSlotItemsInInventory[i]);

            //This Will Select The First Button In The List
            if (!hasSelectedFirstInventorySlot)
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

        if(currentSelectedEquipmentSlot == EquipmentType.RightWeapon01)
        {
            highlightedRightItemIcon01.enabled = true;
            unEquipButtonRightItem01.SetActive(true);
        }
        else
        {
            highlightedRightItemIcon01.enabled = false;
            unEquipButtonRightItem01.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.RightWeapon02)
        {
            highlightedRightItemIcon02.enabled = true;
            unEquipButtonRightItem02.SetActive(true);
        }
        else
        {
            highlightedRightItemIcon02.enabled = false;
            unEquipButtonRightItem02.SetActive(false);
        }
        //if (currentSelectedEquipmentSlot == EquipmentType.RightWeapon03)
        //{
        //    highlightedRightItemIcon03.enabled = true;
        //    unEquipButtonRightItem03.SetActive(true);
        //}
        //else
        //{
        //    highlightedRightItemIcon03.enabled = false;
        //    unEquipButtonRightItem03.SetActive(false);
        //}
        if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon01)
        {
            highlightedLeftItemIcon01.enabled = true;
            unEquipButtonLeftItem01.SetActive(true);
        }
        else
        {
            highlightedLeftItemIcon01.enabled = false;
            unEquipButtonLeftItem01.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon02)
        {
            highlightedLeftItemIcon02.enabled = true;
            unEquipButtonLeftItem02.SetActive(true);
        }
        else
        {
            highlightedLeftItemIcon02.enabled = false;
            unEquipButtonLeftItem02.SetActive(false);
        }
        //if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon03)
        //{
        //    highlightedLeftItemIcon03.enabled = true;
        //    unEquipButtonLeftItem03.SetActive(true);
        //}
        //else
        //{
        //    highlightedLeftItemIcon03.enabled = false;
        //    unEquipButtonLeftItem03.SetActive(false);
        //}

        if (currentSelectedEquipmentSlot == EquipmentType.TwoHandWeapon)
        {
            highlightedTwoHandItemIcon01.enabled = true;
           unEquipTwoHandItem01.SetActive(true);
        }
        else
        {
            highlightedTwoHandItemIcon01.enabled = false;
            unEquipTwoHandItem01.SetActive(false);
        }

        if (currentSelectedEquipmentSlot == EquipmentType.Head)
        {
            highlightedHeadEquipmentItemIcon.enabled = true;
            unEquipButtonHeadEquipmentItem.SetActive(true);
        }
        else
        {
            highlightedHeadEquipmentItemIcon.enabled = false;
            unEquipButtonHeadEquipmentItem.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Body)
        {
            highlightedBodyEquipmentItemIcon.enabled = true;
            unEquipButtonBodyEquipmentItem.SetActive(true);
        }
        else
        {
            highlightedBodyEquipmentItemIcon.enabled = false;
            unEquipButtonBodyEquipmentItem.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Hand)
        {
            highlightedHandEquipmentItemIcon.enabled = true;
            unEquipButtonHandEquipmentItem.SetActive(true);
        }
        else
        {
            highlightedHandEquipmentItemIcon.enabled = false;
            unEquipButtonHandEquipmentItem.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Leg)
        {
            highlightedLegEquipmentItemIcon.enabled = true;
            unEquipButtonLegEquipmentItem.SetActive(true);
        }
        else
        {
            highlightedLegEquipmentItemIcon.enabled = false;
            unEquipButtonLegEquipmentItem.SetActive(false);
        }

        if(currentSelectedEquipmentSlot== EquipmentType.QuickSlot01)
        {
            highlightedQuickSlotIcon01.enabled = true;
            unEquipButtonQuickSlotItem01.SetActive(true);

        }
        else
        {
            highlightedQuickSlotIcon01.enabled = false;
            unEquipButtonQuickSlotItem01.SetActive(false);

        }
        if (currentSelectedEquipmentSlot== EquipmentType.QuickSlot02)
        {
            highlightedQuickSlotIcon02.enabled = true;
            unEquipButtonQuickSlotItem02.SetActive(true);

        }
        else
        {
            highlightedQuickSlotIcon02.enabled = false;
            unEquipButtonQuickSlotItem02.SetActive(false);

        }




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
                    player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(0, player.playerInventoryManager.currentRightHandWeaponID);
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
                    player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(1, player.playerInventoryManager.currentRightHandWeaponID);
                }
                break;

            //case EquipmentType.RightWeapon03:
            //    unEquippedItem = character.playerInventoryManager.weaponsInRightHandSlot[2];

            //    if (unEquippedItem != null)
            //    {
            //        character.playerInventoryManager.weaponsInRightHandSlot[2] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

            //        if (unEquippedItem.worldSpwanInteractableItemID != WorldItemDatabase.instance.unArmedWeapon.worldSpwanInteractableItemID)
            //        {
            //            character.playerInventoryManager.AddItemToInventory(unEquippedItem);
            //        }
            //    }
            //    if (character.playerInventoryManager.rightHandWeaponIndex ==2)
            //    {
            //        character.playerInventoryManager.currentRightHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.worldSpwanInteractableItemID;
            //        //character.playerEquipmentManager.SwitchRightWeapon();
            //    }
            //    break;

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
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(0, player.playerInventoryManager.currentLeftHandWeaponID);
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
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(1, player.playerInventoryManager.currentLeftHandWeaponID);
                }
                break;

            //case EquipmentType.leftWeapon03:
            //    unEquippedItem = character.playerInventoryManager.weaponsInLeftHandSlot[2];

            //    if (unEquippedItem != null)
            //    {
            //        character.playerInventoryManager.weaponsInLeftHandSlot[2] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

            //        if (unEquippedItem.worldSpwanInteractableItemID != WorldItemDatabase.instance.unArmedWeapon.worldSpwanInteractableItemID)
            //        {
            //            character.playerInventoryManager.AddItemToInventory(unEquippedItem);
            //        }
            //    }
            //    if (character.playerInventoryManager.leftHandWeaponIndex ==2)
            //    {
            //        character.playerInventoryManager.currentLeftHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.worldSpwanInteractableItemID;
            //    }
            //    break;
            case EquipmentType.TwoHandWeapon:
                unEquippedItem = player.playerInventoryManager.weaponsInTwoHandSlot[0];

                if (unEquippedItem != null)
                {
                    player.playerInventoryManager.weaponsInTwoHandSlot[0] = Instantiate(WorldItemDatabase.instance.unArmedWeapon);

                    if (unEquippedItem.itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                    {
                        player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    }
                }
                if (player.playerInventoryManager.twoHandWeaponIndex ==1)
                {

                    player.playerInventoryManager.currentLeftHandWeaponID = WorldItemDatabase.instance.unArmedWeapon.itemID;
                    player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(1, player.playerInventoryManager.currentLeftHandWeaponID);
                }
                break;

            case EquipmentType.Head:
                unEquippedItem = player.playerInventoryManager.headEquipment;

                if (unEquippedItem != null)
                {
                    
                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);
                    
                }

                player.playerInventoryManager.headEquipment = null;
                player.playerEquipmentManager.LoadHeadEquipment(player.playerInventoryManager.headEquipment);
                break;

            case EquipmentType.Body:
                unEquippedItem = player.playerInventoryManager.bodyEquipment;

                if (unEquippedItem != null)
                {

                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);

                }

                player.playerInventoryManager.bodyEquipment = null;
                player.playerEquipmentManager.LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);

                break;

             case EquipmentType.Hand:
                unEquippedItem = player.playerInventoryManager.handEquipment;

                if (unEquippedItem != null)
                {

                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);

                }

                player.playerInventoryManager.handEquipment = null;
                player.playerEquipmentManager.LoadHandEquipment(player.playerInventoryManager.handEquipment);
                break;

            case EquipmentType.Leg:
                unEquippedItem = player.playerInventoryManager.legEquipment;

                if (unEquippedItem != null)
                {

                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);

                }

                player.playerInventoryManager.legEquipment = null;
                player.playerEquipmentManager.LoadLegEquipment(player.playerInventoryManager.legEquipment);
                break;
            case EquipmentType.QuickSlot01:
                unEquippedItem = player.playerInventoryManager.QuickSlotItemsInQuickSlot[0];

                if (unEquippedItem != null)
                {

                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);

                }

                player.playerInventoryManager.QuickSlotItemsInQuickSlot[0] = null;

                if (player.playerInventoryManager.quickSlotItemIndex==0)
                {
                    player.playerInventoryManager.currentQuickSlotItemID = -1;
                }
                
                break;
            case EquipmentType.QuickSlot02:
                unEquippedItem = player.playerInventoryManager.QuickSlotItemsInQuickSlot[1];

                if (unEquippedItem != null)
                {

                    player.playerInventoryManager.AddItemToInventory(unEquippedItem);

                }

                player.playerInventoryManager.QuickSlotItemsInQuickSlot[1] = null;

                if (player.playerInventoryManager.quickSlotItemIndex==1)
                {
                    player.playerInventoryManager.currentQuickSlotItemID = -1;
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
            //case EquipmentType.RightWeapon03:
            //    lastSelectedButton = rightHandSlot03.GetComponent<Button>();
            //    break;
            case EquipmentType.leftWeapon01:
                lastSelectedButton = leftHandSlot01.GetComponent<Button>();
                break;
            case EquipmentType.leftWeapon02:
                lastSelectedButton = leftHandSlot02.GetComponent<Button>();
                break;
            //case EquipmentType.leftWeapon03:
            //    lastSelectedButton = leftHandSlot03.GetComponent<Button>();

                //break;

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
