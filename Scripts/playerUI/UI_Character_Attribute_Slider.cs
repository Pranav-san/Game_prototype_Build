using UnityEngine;

public class UI_Character_Attribute_Slider : MonoBehaviour
{
    [SerializeField] CharacterAttributes sliderAttribute;

    public CharacterAttributes GetCurrentSelectedAttribute() 
    {
        return sliderAttribute;
    } 

    public void SetCurrentSelectedAttribute()
    {
        //Set CurrentSelected Attribute In PlayerUILevelUPManager
        PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute = sliderAttribute;
         
        //Enable Highlighted Image Based On  The Current Selected Attribute
        if(PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute == CharacterAttributes.Vitality)
        {
            PlayerUIManager.instance.playerUILevelUpManager.vitalityHighlightImage.enabled = true;

        }
        else
        {
            PlayerUIManager.instance.playerUILevelUpManager.vitalityHighlightImage.enabled = false;
        }

        if (PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute == CharacterAttributes.Endurance)
        {
            PlayerUIManager.instance.playerUILevelUpManager.enduranceHighlightImage.enabled = true;
        }
        else
        {
            PlayerUIManager.instance.playerUILevelUpManager.enduranceHighlightImage.enabled = false;
        }

        if(PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute == CharacterAttributes.Strength)
        {
            PlayerUIManager.instance.playerUILevelUpManager.strengthHighlightImage.enabled = true;
        }
        else
        {
            PlayerUIManager.instance.playerUILevelUpManager.strengthHighlightImage.enabled = false;

        }

        if (PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute == CharacterAttributes.Dexterity)
        {
            PlayerUIManager.instance.playerUILevelUpManager.dexterityHighlightImage.enabled = true;
        }
        else
        {
            PlayerUIManager.instance.playerUILevelUpManager.dexterityHighlightImage.enabled = false;

        }

        if (PlayerUIManager.instance.playerUILevelUpManager.currentSelectedAttribute == CharacterAttributes.Luck)
        {
            PlayerUIManager.instance.playerUILevelUpManager.luckHighlightImage.enabled = true;
        }
        else
        {
            PlayerUIManager.instance.playerUILevelUpManager.luckHighlightImage.enabled = false;

        }
    }

    
}
