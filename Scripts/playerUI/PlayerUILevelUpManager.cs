using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerUILevelUpManager : PlayerUIMenu
{
    [Header("Levels")]
    [SerializeField] int[] playerLevels = new int[100];
    [SerializeField] int baseLevelCost = 83;
    [SerializeField] int totalLevelUpCost=0;

    [Header("Confirm Level PopUp")]
    [SerializeField] CanvasGroup confirmLevelPopUpCanvasGroup;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;
    [SerializeField] TextMeshProUGUI upgradeLevelText;
    [SerializeField] TextMeshProUGUI CurrentSelectedAttributeText;
    [SerializeField] TextMeshProUGUI CurrentSelectedAttributeLevelText;
    [SerializeField] TextMeshProUGUI CurrentSelectedAttributeProjectedLevelText;




    [Header("Character Stats")]
    [SerializeField] TextMeshProUGUI characterLevelText;
    [SerializeField] TextMeshProUGUI runesHeldText;
    [SerializeField] TextMeshProUGUI runesNeededText;
    [SerializeField] TextMeshProUGUI vitalityLevelText;
    [SerializeField] TextMeshProUGUI enduranceLevelText;
    [SerializeField] TextMeshProUGUI strengthLevelText;
    [SerializeField] TextMeshProUGUI dexterityLevelText;
    [SerializeField] TextMeshProUGUI luckLevelText;

    [Header("Projected Character Stats")]
    [SerializeField] TextMeshProUGUI ProjectedCharacterLevelText;
    [SerializeField] TextMeshProUGUI ProjectedRunesHeldText;
    [SerializeField] TextMeshProUGUI projectedVitalityLevelText;
    [SerializeField] TextMeshProUGUI projectedEnduranceLevelText;
    [SerializeField] TextMeshProUGUI projectedStrengthLevelText;
    [SerializeField] TextMeshProUGUI projectedDexterityLevelText;
    [SerializeField] TextMeshProUGUI projectedLuckLevelText;

    [Header("Slider")]
    public CharacterAttributes currentSelectedAttribute;
    public Slider vitalitySlider;
    public Slider enduranceSlider;
    public Slider strengthSlider;
    public Slider dexteritySlider;
    public Slider luckSlider;

    [Header("Attribute Highlight Images")]
    public  Image vitalityHighlightImage;
    public  Image enduranceHighlightImage;
    public  Image strengthHighlightImage;
    public  Image dexterityHighlightImage;
    public  Image luckHighlightImage;

    private void Awake()
    {
        confirmLevelPopUpCanvasGroup.alpha = 0f;
        confirmLevelPopUpCanvasGroup.interactable = false;
        confirmLevelPopUpCanvasGroup.blocksRaycasts = false;
        SetAllLevelsCost();
    }



    public override void OpenMenu()
    {
        base.OpenMenu();

        SetCurrentStats();
    }

    public void SetCurrentStats()
    {
        //Character Level
        characterLevelText.text = PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();
        ProjectedCharacterLevelText.text = PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes().ToString();

        //Runes Held
        runesHeldText.text = PlayerUIManager.instance.playerStatsManager.runes.ToString();
        ProjectedRunesHeldText.text = PlayerUIManager.instance.playerStatsManager.runes.ToString();
        //runesNeededText.text = "0";

        //Attributes
        vitalityLevelText.text = PlayerUIManager.instance.playerStatsManager.vitality.ToString();
        projectedVitalityLevelText.text = PlayerUIManager.instance.playerStatsManager.vitality.ToString();
        vitalitySlider.minValue = PlayerUIManager.instance.playerStatsManager.vitality;
        vitalitySlider.value = PlayerUIManager.instance.playerStatsManager.vitality;


        enduranceLevelText.text = PlayerUIManager.instance.playerStatsManager.endurance.ToString();
        projectedEnduranceLevelText.text = PlayerUIManager.instance.playerStatsManager.endurance.ToString();
        enduranceSlider.minValue = PlayerUIManager.instance.playerStatsManager.endurance;
        enduranceSlider.value = PlayerUIManager.instance.playerStatsManager.endurance;

        strengthLevelText.text = PlayerUIManager.instance.playerStatsManager.strength.ToString();
        projectedStrengthLevelText.text = PlayerUIManager.instance.playerStatsManager.strength.ToString();
        strengthSlider.minValue = PlayerUIManager.instance.playerStatsManager.strength;
        strengthSlider.value = PlayerUIManager.instance.playerStatsManager.strength;

        dexterityLevelText.text = PlayerUIManager.instance.playerStatsManager.dexterity.ToString();
        projectedDexterityLevelText.text = PlayerUIManager.instance.playerStatsManager.dexterity.ToString();
        dexteritySlider.minValue = PlayerUIManager.instance.playerStatsManager.dexterity;
        dexteritySlider.value = PlayerUIManager.instance.playerStatsManager.dexterity;

        luckLevelText.text = PlayerUIManager.instance.playerStatsManager.luck.ToString();
        projectedLuckLevelText.text = PlayerUIManager.instance.playerStatsManager.luck.ToString();
        luckSlider.minValue = PlayerUIManager.instance.playerStatsManager.luck;
        luckSlider.value = PlayerUIManager.instance.playerStatsManager.luck;

        //vitalitySlider.Select();
        //vitalitySlider.OnSelect(null);

        int currentLevel = PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes();
        int nextLevel = currentLevel + 1;

        CalculateLevelCost(currentLevel, nextLevel);
        ProjectedCharacterLevelText.text = nextLevel.ToString();
        


    }


    public void UpdateSliderBasedOnCurrentSelectedAttribute()
    {
        switch (currentSelectedAttribute)
        {
            case CharacterAttributes.Vitality:
                projectedVitalityLevelText.text = vitalitySlider.value.ToString();
                break;

            case CharacterAttributes.Endurance:
                projectedEnduranceLevelText.text = enduranceSlider.value.ToString();
                break;

            case CharacterAttributes.Strength:
                projectedStrengthLevelText.text = strengthSlider.value.ToString();
                break;

            case CharacterAttributes.Dexterity:
                projectedDexterityLevelText.text = dexteritySlider.value.ToString();
                break;

            case CharacterAttributes.Luck:
                projectedLuckLevelText.text = luckSlider.value.ToString();
                break;
            default:
                break;


        }

      
        //CalculateLevelCost(PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes(), PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes(true));

        //ProjectedCharacterLevelText.text = PlayerUIManager.instance.playerStatsManager.CalculateCharacterLevelBasedOnAttributes(true).ToString();



    }


    public void ConfirmLevels()
    {
        PlayerStatsManager playerStatsManager = PlayerUIManager.instance.playerStatsManager;

        //Deduct Cost From Total Runes
        playerStatsManager.runes -= totalLevelUpCost;




        //Set New Stats
        playerStatsManager.vitality = Mathf.RoundToInt(vitalitySlider.value);
        playerStatsManager.endurance = Mathf.RoundToInt(enduranceSlider.value);
        playerStatsManager.strength = Mathf.RoundToInt(strengthSlider.value);
        playerStatsManager.dexterity = Mathf.RoundToInt(dexteritySlider.value);
        playerStatsManager.luck = Mathf.RoundToInt(luckSlider.value);


        SetCurrentStats();
        DeactivateConfirmLevelPopUp();


        //Save Game
        //WorldSaveGameManager.instance.SaveGame();





    }

    public void ActivateConfirmLevelPopUp()
    {
        string SelectedAttribute = currentSelectedAttribute.ToString();
        upgradeLevelText.text = "Upgrade " + SelectedAttribute;
        CurrentSelectedAttributeText.text = SelectedAttribute;
        confirmLevelPopUpCanvasGroup.alpha =1;
        confirmLevelPopUpCanvasGroup.blocksRaycasts = true;
        confirmLevelPopUpCanvasGroup.interactable = true;

    }

    public void SetCurrentSelectedAttributeLevelsInPopUp(CharacterAttributes currentSlelectedCharacterAttribute)
    {
        switch (currentSlelectedCharacterAttribute)
        {
            case CharacterAttributes.Vitality:
                CurrentSelectedAttributeLevelText.text = PlayerUIManager.instance.playerStatsManager.vitality.ToString();
                CurrentSelectedAttributeProjectedLevelText.text = vitalitySlider.value.ToString();
                break;

            case CharacterAttributes.Endurance:
                CurrentSelectedAttributeLevelText.text = PlayerUIManager.instance.playerStatsManager.endurance.ToString();
                CurrentSelectedAttributeProjectedLevelText.text = enduranceSlider.value.ToString();
                break;

            case CharacterAttributes.Strength:
                CurrentSelectedAttributeLevelText.text = PlayerUIManager.instance.playerStatsManager.strength.ToString();
                CurrentSelectedAttributeProjectedLevelText.text = strengthSlider.value.ToString();
                break;

            case CharacterAttributes.Dexterity:
                CurrentSelectedAttributeLevelText.text = PlayerUIManager.instance.playerStatsManager.dexterity.ToString();
                CurrentSelectedAttributeProjectedLevelText.text = dexteritySlider.value.ToString();
                break;

            case CharacterAttributes.Luck:
                CurrentSelectedAttributeLevelText.text = PlayerUIManager.instance.playerStatsManager.luck.ToString();
                CurrentSelectedAttributeProjectedLevelText.text = luckSlider.value.ToString();
                break;
            default:
                break;

        }

    }

    public void DeactivateConfirmLevelPopUp()
    {
        DecreaseAttribute();
        confirmLevelPopUpCanvasGroup.alpha =0;
        confirmLevelPopUpCanvasGroup.interactable= false;
        confirmLevelPopUpCanvasGroup.blocksRaycasts= false;
        SetCurrentStats();

    }

    private void SetAllLevelsCost()
    {
        for (int i = 0; i<playerLevels.Length; i++)
        {
            if(i==0) 
                continue;
            playerLevels[i] =  baseLevelCost +(50 * i);
        }
    }

    private void CalculateLevelCost(int currentLevel, int projectedLevel)
    {
        //We Dont Want To Charge For Levels We Already Paid For

        int totalCost =0;

        for (int i=0; i < projectedLevel; i++)
        {
            if(i < currentLevel)
                continue;

            //This Is A Condition To Stop Adding Cost If Player Level Somehow Exceeds The Size Of the Array We Have Created
            if(i>playerLevels.Length)
                continue;
            
            totalCost += playerLevels[i];

        }

        totalLevelUpCost = totalCost;

        runesNeededText.text = totalLevelUpCost.ToString();
        ProjectedRunesHeldText.text = (PlayerUIManager.instance.playerStatsManager.runes - totalLevelUpCost).ToString();

        if(totalLevelUpCost> PlayerUIManager.instance.playerStatsManager.runes)
        {
            ProjectedRunesHeldText.color = Color.red;
        }
        else
        {
            ProjectedRunesHeldText.color = Color.white;

        }



    }

    public void OnClickIncreaseButtonPressed(UI_Character_Attribute_Slider currentSlelectedCharacterAttribute)
    {
        currentSelectedAttribute = currentSlelectedCharacterAttribute.GetCurrentSelectedAttribute();
        IncreaseAttribute();


    }

    public void OnClickDecreaseButtonPressed(UI_Character_Attribute_Slider currentSlelectedCharacterAttribute)
    {
        currentSelectedAttribute = currentSlelectedCharacterAttribute.GetCurrentSelectedAttribute();
        DecreaseAttribute();


    }

    public void IncreaseAttribute()
    {
        switch (currentSelectedAttribute)
        {
            case CharacterAttributes.Vitality:
                vitalitySlider.value = Mathf.RoundToInt(vitalitySlider.value+1);
                SetCurrentSelectedAttributeLevelsInPopUp(currentSelectedAttribute);
                ActivateConfirmLevelPopUp();
                break;

            case CharacterAttributes.Endurance:
                enduranceSlider.value = Mathf.RoundToInt(enduranceSlider.value+1);
                SetCurrentSelectedAttributeLevelsInPopUp(currentSelectedAttribute);
                ActivateConfirmLevelPopUp();
                break;

            case CharacterAttributes.Strength:
                strengthSlider.value = Mathf.RoundToInt(strengthSlider.value+1);
                SetCurrentSelectedAttributeLevelsInPopUp(currentSelectedAttribute);
                ActivateConfirmLevelPopUp();
                break;

            case CharacterAttributes.Dexterity:
                dexteritySlider.value = Mathf.RoundToInt(dexteritySlider.value+1);
                SetCurrentSelectedAttributeLevelsInPopUp(currentSelectedAttribute);
                ActivateConfirmLevelPopUp();
                break;

            case CharacterAttributes.Luck:
                luckSlider.value = Mathf.RoundToInt(luckSlider.value+1);
                SetCurrentSelectedAttributeLevelsInPopUp(currentSelectedAttribute);
                ActivateConfirmLevelPopUp();
                break;
            default:
                break;



        }


        PlayerStatsManager playerStatsManager = PlayerUIManager.instance.playerStatsManager;
        //Calculate Cost
        if (totalLevelUpCost> playerStatsManager.runes)
        {
            //Disable OK Button
            confirmButton.interactable = false;


        }
        else
        {
            confirmButton.interactable = true;
        }




    }

    public void DecreaseAttribute()
    {
        switch (currentSelectedAttribute)
        {
            case CharacterAttributes.Vitality:
                vitalitySlider.value = Mathf.RoundToInt(vitalitySlider.value-1);
                break;

            case CharacterAttributes.Endurance:
                enduranceSlider.value = Mathf.RoundToInt(enduranceSlider.value-1);
                break;

            case CharacterAttributes.Strength:
                strengthSlider.value = Mathf.RoundToInt(strengthSlider.value-1);
                break;

            case CharacterAttributes.Dexterity:
                dexteritySlider.value = Mathf.RoundToInt(dexteritySlider.value-1);
                break;

            case CharacterAttributes.Luck:
                luckSlider.value = Mathf.RoundToInt(luckSlider.value-1);
                break;
            default:
                break;

        }


    }

}
