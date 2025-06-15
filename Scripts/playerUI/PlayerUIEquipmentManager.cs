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
    
    [SerializeField] Image headEquipmentSlot;
    [SerializeField] Image bodyEquipmentSlot;
    [SerializeField] Image handEquipmentSlot;
    [SerializeField] Image legEquipmentSlot;

    [Header("Highlight Selected Slot")]
    public Image highlightedRightItemIcon01;
    public Image highlightedRightItemIcon02;
    public Image highlightedRightItemIcon03;
    
    public Image highlightedLeftItemIcon01;
    public Image highlightedLeftItemIcon02;
    public Image highlightedLeftItemIcon03;

    public Image highlightedHeadEquipmentItemIcon;
    public Image highlightedBodyEquipmentItemIcon;
    public Image highlightedHandEquipmentItemIcon;
    public Image highlightedLegEquipmentItemIcon;

    [Header("unEquipButton ")]
    public GameObject unEquipButtonRightItemIcon01;
    public GameObject unEquipButtonRightItemIcon02;
    public GameObject unEquipButtonRightItemIcon03;

    public GameObject unEquipButtonLeftItemIcon01;
    public GameObject unEquipButtonLeftItemIcon02;
    public GameObject unEquipButtonLeftItemIcon03;

    public GameObject unEquipButtonHeadEquipmentItemIcon;
    public GameObject unEquipButtonBodyEquipmentItemIcon;
    public GameObject unEquipButtonHandEquipmentItemIcon;
    public GameObject unEquipButtonLegEquipmentItemIcon;




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

    public void SelectEquipmentSlot(int EquipmentSlot)
    {
        currentSelectedEquipmentSlot = (EquipmentType)EquipmentSlot;

        if(currentSelectedEquipmentSlot == EquipmentType.RightWeapon01)
        {
            highlightedRightItemIcon01.enabled = true;
            unEquipButtonRightItemIcon01.SetActive(true);
        }
        else
        {
            highlightedRightItemIcon01.enabled = false;
            unEquipButtonRightItemIcon01.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.RightWeapon02)
        {
            highlightedRightItemIcon02.enabled = true;
            unEquipButtonRightItemIcon02.SetActive(true);
        }
        else
        {
            highlightedRightItemIcon02.enabled = false;
            unEquipButtonRightItemIcon02.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.RightWeapon03)
        {
            highlightedRightItemIcon03.enabled = true;
            unEquipButtonRightItemIcon03.SetActive(true);
        }
        else
        {
            highlightedRightItemIcon03.enabled = false;
            unEquipButtonRightItemIcon03.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon01)
        {
            highlightedLeftItemIcon01.enabled = true;
            unEquipButtonLeftItemIcon01.SetActive(true);
        }
        else
        {
            highlightedLeftItemIcon01.enabled = false;
            unEquipButtonLeftItemIcon01.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon02)
        {
            highlightedLeftItemIcon02.enabled = true;
            unEquipButtonLeftItemIcon02.SetActive(true);
        }
        else
        {
            highlightedLeftItemIcon02.enabled = false;
            unEquipButtonLeftItemIcon02.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.leftWeapon03)
        {
            highlightedLeftItemIcon03.enabled = true;
            unEquipButtonLeftItemIcon03.SetActive(true);
        }
        else
        {
            highlightedLeftItemIcon03.enabled = false;
            unEquipButtonLeftItemIcon03.SetActive(false);
        }

        if (currentSelectedEquipmentSlot == EquipmentType.Head)
        {
            highlightedHeadEquipmentItemIcon.enabled = true;
            unEquipButtonHeadEquipmentItemIcon.SetActive(true);
        }
        else
        {
            highlightedHeadEquipmentItemIcon.enabled = false;
            unEquipButtonHeadEquipmentItemIcon.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Body)
        {
            highlightedBodyEquipmentItemIcon.enabled = true;
            unEquipButtonBodyEquipmentItemIcon.SetActive(true);
        }
        else
        {
            highlightedBodyEquipmentItemIcon.enabled = false;
            unEquipButtonBodyEquipmentItemIcon.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Hand)
        {
            highlightedHandEquipmentItemIcon.enabled = true;
            unEquipButtonHandEquipmentItemIcon.SetActive(true);
        }
        else
        {
            highlightedHandEquipmentItemIcon.enabled = false;
            unEquipButtonHandEquipmentItemIcon.SetActive(false);
        }
        if (currentSelectedEquipmentSlot == EquipmentType.Leg)
        {
            highlightedLegEquipmentItemIcon.enabled = true;
            unEquipButtonLegEquipmentItemIcon.SetActive(true);
        }
        else
        {
            highlightedLegEquipmentItemIcon.enabled = false;
            unEquipButtonLegEquipmentItemIcon.SetActive(false);
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
                    //player.playerEquipmentManager.SwitchRightWeapon();
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
                    //player.playerEquipmentManager.SwitchRightWeapon();
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
                    //player.playerEquipmentManager.SwitchRightWeapon();
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
