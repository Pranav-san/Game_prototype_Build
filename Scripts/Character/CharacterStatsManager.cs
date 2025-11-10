using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    [Header("Status")]
    public bool isDead = false;


    public float maxStamina = 100;
    public float currentStamina = 100;
    public int maxHealth = 100;
    public int currentHealth = 100;

    public CharacterManager character;
    public playerManager player;

    public UI_StatBar uI_Character_HP_Bar;
    public AIHealthUI AIHealthUI;

    [Header("Stats")]
    public int vitality = 1;
    public int endurance = 1;
    public int strength = 1;
    public int dexterity = 1;
    public int luck = 1;

    [Header("Currency")]
    public int runes = 0;

    [Header("Poise")]
    public float totalPoiseDamage; //How Much Poise damage We have Taken
    public float offensivePoiseBonus; //The Poise Bonus Gained From Using Weapons
    public float basePoiseDefense; //The Poise bonus Gained From armor/talisman
    public float defaultPoiseResetTimer = 8; // the time it takes for poise damage To reset (must not be Hit in the time or it will reset)
    public float poiseResetTimer = 0;
    

    [Header("Blocking Absorption")]
    public float blockingPhysicalAbsorption;
    public float blockingFireAbsorption;
    public float blockingLightningAbsorption;
    public float blockingMagicAbsorption;
    public float blockingStability;


    [Header("Armor Absorption")]
    public float ArmorPhysicalDamageAbsorption;
    public float ArmorMagicDamageAbsorption;
    public float ArmorFireDamageAbsorption;
    public float ArmorLightningDamageAbsorption;

    [Header("Resistance")]
    public float ArmorImmunity;
    public float ArmorRobustness;

    [Header("Runes")]
    public int runesDroppedOnDeath = 50;
    public CharacterManager lastAttacker;


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        player = GetComponent<playerManager>();

        uI_Character_HP_Bar = GetComponentInChildren<UI_Character_HP_Bar>();
        AIHealthUI = GetComponentInChildren<AIHealthUI>();
    }

    
    //OnValidate Only runs On Editor
    private void OnValidate()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            isDead = true;

            if (isDead)
            {

                if (character != null)
                {
                    StartCoroutine(character.ProcessDeathEvent());

                }
            }
            
        }

        if (PlayerUIManager.instance != null)
        {
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(currentStamina));
            PlayerUIManager.instance.UpdateHealthBar(currentHealth);
            
        }
    }
    
    
    protected virtual void Update()
    {
        if (currentStamina < maxStamina)
        {
            RegenerateStamina(45);
        }

        HandlePoiseResetTimer();    
        

    }

    public void CheckHP(int oldValue, int newValue)
    {
        if (currentHealth<=0 && !isDead)
        {
            isDead = true;
            StartCoroutine(character.ProcessDeathEvent());

            
            
            
        }
    }

    public void ConsumeHealth(int amount)
    {
        int oldHealth = currentHealth;

        if (currentHealth - amount < 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth -= amount;
        }


        //Ai Character Hp bar 
        if (AIHealthUI != null && !isDead)
        {
            AIHealthUI.OnHealthChanged(oldHealth, currentHealth);
        }

        //Check whether its character or not before updating Player Hp BaR

        if (character is playerManager)  
        {
            PlayerUIManager.instance.UpdateHealthBar(Mathf.RoundToInt(currentHealth));
        }
        



        

        CheckHP(oldHealth, currentHealth);
    }

    public void ConsumeStamina(float amount)
    {
        if (currentStamina - amount < 0)
        {
            currentStamina = 0;
        }
        else
        {
            currentStamina -= amount;
        }

        if(character is playerManager)
        {
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(currentStamina));
        }

        
    }

    public void RegenerateStamina(float amountPerSecond)
    {
        if(character is playerManager)
        {
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(currentStamina));
        }
        

        if (character.isAttacking)
            return;

        if (character.isPerformingAction)
            return;
       

        if (character.isSprinting)
            return ;

        if(character.isBlocking)
            return;

        if (currentStamina < maxStamina)
        {

            currentStamina += amountPerSecond * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            if(character is playerManager)
            {
                PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(currentStamina));
                PlayerInputManager.Instance.ForceStateUpdate();
            }
           
        }
    }
    protected virtual void HandlePoiseResetTimer()
    {
        if (poiseResetTimer>0)
        {
            poiseResetTimer -= Time.deltaTime;
        }
        else
        {
            totalPoiseDamage = 0;
        }
    }


    public int CalculateCharacterLevelBasedOnAttributes(bool calculateProjvtedLevel = false)
    {


        if(calculateProjvtedLevel)
        {
            int totalProjectedAttributes = 
                Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.vitalitySlider.value) +
                Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.enduranceSlider.value)+
                Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.strengthSlider.value)+
                Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.dexteritySlider.value)+
                Mathf.RoundToInt(PlayerUIManager.instance.playerUILevelUpManager.luckSlider.value);

            int projectedCharacterLevel = totalProjectedAttributes- 5 + 1;

            if (projectedCharacterLevel < 1)
                projectedCharacterLevel = 1;


            return projectedCharacterLevel;
        }



        int totalAttributes = vitality+endurance+strength+dexterity+luck;
        int characterLevel = totalAttributes- 5 + 1;

        if(characterLevel < 1)
            characterLevel = 1;


        return characterLevel;


    }
}
