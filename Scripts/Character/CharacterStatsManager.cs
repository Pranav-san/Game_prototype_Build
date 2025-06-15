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
    public UI_Character_HP_Bar uI_Character_HP_Bar;

    [Header("Blocking Absorption")]
    public float blockingPhysicalAbsorption;
    public float blockingFireAbsorption;
    public float blockingLightningAbsorption;
    public float blockingMagicAbsorption;


    [Header("Armor Absorption")]
    public float ArmorPhysicalDamageAbsorption;
    public float ArmorMagicDamageAbsorption;
    public float ArmorFireDamageAbsorption;
    public float ArmorLightningDamageAbsorption;

    [Header("Resistance")]
    public float ArmorImmunity;
    public float ArmorRobustness;


    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        uI_Character_HP_Bar = GetComponentInChildren<UI_Character_HP_Bar>();
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
    
    
    void Update()
    {
        if (currentStamina < maxStamina)
        {
            RegenerateStamina(10);
        }

        

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
        if (uI_Character_HP_Bar != null)
        {
            if (!isDead)
            {
                uI_Character_HP_Bar.SetStat(currentHealth);

            }
           
        }

        //Check whether its player or not before updating Player Hp BaR

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
    }

    

    public void RegenerateStamina(float amountPerSecond)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += amountPerSecond * Time.deltaTime;
            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
            PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(currentStamina));
            PlayerInputManager.Instance.ForceStateUpdate();
        }
    }
}
