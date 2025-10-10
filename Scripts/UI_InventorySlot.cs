using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public Image highlightedItemIcon;
    //[SerializeField] GameObject equipButton;

    [SerializeField] public Item currentItem;

    PlayerUIInventoryManager inventoryManager;


    [Header("Display Item Info")]
    public UI_ItemInfo uI_ItemInfo;

    private void Awake()
    {
        if (inventoryManager == null)
        {
            inventoryManager = GetComponentInParent<PlayerUIInventoryManager>();
        }
    }

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
        uI_ItemInfo.DisplayItem(currentItem);
        inventoryManager.OpenItemInfoWindow();

    }

    public void DeSelectSlot()
    {
        highlightedItemIcon.enabled = false;
        inventoryManager.CloseItemInfoWindow();


    }




}
