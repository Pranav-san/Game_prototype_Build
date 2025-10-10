using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIInventoryManager : MonoBehaviour
{

    [Header("Menu")]
    [SerializeField] GameObject menu;
    [SerializeField]MenuSlideAnimator menuAnimator;

    [Header("Highligt Icons")]
    [SerializeField] Image highlightedAllItemsIcon;

    [Header("Display Item Info")]
    public UI_ItemInfo ItemInfo;
    public GameObject itemInfoGameObject; 


    [Header("Inventory")]
    public InventoryCategory currentSelectedInventoryCategory;
    [SerializeField] GameObject itemTypeWindow;
    [SerializeField] GameObject inventoryWindow;
    [SerializeField] Transform inventoryContentWindow;
    [SerializeField] GameObject inventorySlotPrefab;


    public void OpenInventoryManagermenu()
    {
        PlayerUIManager.instance.menuWindowOpen =true;
        menu.SetActive(true);
        menuAnimator.ShowMenu();
        inventoryWindow.SetActive(true);

    }
    public void CloseInventoryManagermenu()
    {
        PlayerUIManager.instance.menuWindowOpen =false;
        menuAnimator.HideMenu();    
        menu.SetActive(false);
    }

    public void OpenItemInfoWindow()
    {
        itemInfoGameObject.SetActive(true);

    }

    public void CloseItemInfoWindow()
    {
        itemInfoGameObject.SetActive(false);

    }

    private void ClearEquipmentInventory()
    {
        foreach(Transform item in inventoryContentWindow)
        {
           
            
               Destroy(item.gameObject);

            
                
        }
    }
    public void LoadItemsInInventory()
    {
        inventoryWindow.SetActive(true);

        ClearEquipmentInventory();

        switch (currentSelectedInventoryCategory)
        {
            case InventoryCategory.All:
                LoadAllItemsInInventory();
                break;

            default:
                break;
        }
    }

    public void LoadAllItemsInInventory()
    {
        playerManager player = playerManager.instance?.GetComponent<playerManager>();

        List<Item> itemsInInventory = new List<Item>();

        for (int i = 0; i<player.playerInventoryManager.itemsInInventory.Count; i++)
        {
            Item item = player.playerInventoryManager.itemsInInventory[i];

            itemsInInventory.Add(item);
        }

        if(itemsInInventory.Count <= 0)
        {
            return;
        }

        bool hasSelectedFirstInventorySlot = false;

        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            GameObject inventorySlotGameObject = Instantiate(inventorySlotPrefab, inventoryContentWindow);
            UI_InventorySlot InventorySlot = inventorySlotGameObject.GetComponent<UI_InventorySlot>();
            InventorySlot.uI_ItemInfo = ItemInfo;
            InventorySlot.AddItem(itemsInInventory[i]);
            

            //This Will Select The First Button In The List
            //if (!hasSelectedFirstInventorySlot)
            //{
            //    hasSelectedFirstInventorySlot = true;
            //    Button inventorySlotButton = inventorySlotGameObject.GetComponent<Button>();
            //    inventorySlotButton.Select();
            //    inventorySlotButton.OnSelect(null);
            //}
        }

    }


    public void selectInventoryCategory(int Category)
    {
        currentSelectedInventoryCategory = (InventoryCategory)Category;

        if(currentSelectedInventoryCategory == InventoryCategory.All)
        {
            highlightedAllItemsIcon.enabled = true;

        }
        else
        {
            highlightedAllItemsIcon.enabled = false;
        }

    }

   





}
