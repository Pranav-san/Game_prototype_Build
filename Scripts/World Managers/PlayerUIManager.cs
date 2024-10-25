using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    [HideInInspector] public PlayerUIPopUPManager playerUIPopUPManager;
    public PlayerUIHUDManager playerUIHUDManager;
    [HideInInspector]public PlayerUICharacterMenuManager playerUICharacterMenuManager;
    [HideInInspector]public PlayerUIEquipmentManager playerUIEquipmentManager;


    [SerializeField] UI_StatBar staminaBar;
    [SerializeField] UI_StatBar healthBar;

    [Header("Menu Window open")]
    public bool menuWindowOpen=false; //Inventory Screen Containing Equipment menu, etc
    public bool popUpWindowIsOpen=false;//Item Pick up, Dialogue Pop Up, etc

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make sure this GameObject is not destroyed when loading new scenes
        }
        else
        {
            Destroy(gameObject);
        }
        playerUIPopUPManager = GetComponentInChildren<PlayerUIPopUPManager>();
        playerUIHUDManager = GetComponentInChildren<PlayerUIHUDManager>();
        playerUICharacterMenuManager = GetComponentInChildren<PlayerUICharacterMenuManager>();
        playerUIEquipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetNewStaminaValue(int oldValue, float newValue)
    {
        staminaBar.SetStat((int)newValue);
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        staminaBar.SetStat(maxStamina);
    }

    public void UpdateStaminaBar(int currentStamina)
    {
        staminaBar.SetStat(currentStamina);
    }

    // Health Bar
    public void SetNewHealthValue(int oldValue, float newValue)
    {
        healthBar.SetStat((int)newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        healthBar.SetMaxStat(maxHealth);
    }

    public void UpdateHealthBar(int currentHealth)
    {
        healthBar.SetStat(currentHealth);
    }

    public void CloseAllMenu()
    {
        playerUICharacterMenuManager.CloseCharacterMenu();
        playerUIEquipmentManager.CloseEquipmentManagerMenu();


    }

   
}
