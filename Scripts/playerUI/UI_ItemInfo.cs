using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ItemInfo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("item info To Display")]
    public Image itemImage;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemStatsText;

    


    public void DisplayItem(Item item)
    {
        if (item == null)
        {
            itemImage.enabled = false;
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemStatsText.text = "";
            return;
        }

        itemImage.enabled = true;
        itemImage.sprite = item.itemIcon;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.itemDescription;


    }
}
