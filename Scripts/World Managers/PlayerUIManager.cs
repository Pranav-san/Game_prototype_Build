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
    [HideInInspector]public PlayerUISiteOfGraceManager playerUISiteOfGraceManager;
    [HideInInspector]public PlayerUITeleportLocationManager playerUITeleportLocationManager;


    //[SerializeField] UI_StatBar staminaBar;
    //[SerializeField] UI_StatBar healthBar;

    [Header("Menu Window open")]
    public bool menuWindowOpen=false; //Inventory Screen Containing Equipment menu, etc
    public bool popUpWindowIsOpen=false;//Item Pick up, Dialogue Pop Up, etc
    public bool survivalWheelOpen = false;
    [SerializeField] public GameObject inspectObjectTouchField;

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
        playerUISiteOfGraceManager = GetComponentInChildren<PlayerUISiteOfGraceManager>();
        playerUITeleportLocationManager = GetComponentInChildren<PlayerUITeleportLocationManager>();
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetNewStaminaValue(int oldValue, float newValue)
    {
        playerUIHUDManager.staminaBar.SetStat((int)newValue);
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        playerUIHUDManager.staminaBar.SetStat(maxStamina);
    }

    

    // Health Bar
    public void SetNewHealthValue(int oldValue, float newValue)
    {
        playerUIHUDManager.healthBar.SetStat((int)newValue);
    }

    public void SetMaxHealthValue(int maxHealth)
    {
        playerUIHUDManager.healthBar.SetMaxStat(maxHealth);
    }

    public void UpdateHealthBar(int currentHealth)
    {
        playerUIHUDManager.healthBar.SetStat(currentHealth);
    }

    public void UpdateStaminaBar(int currentStamina)
    {
        playerUIHUDManager.staminaBar.SetStat(currentStamina);
    }

    ////public void UpdateTemperatureBar(int currentTemperature)
    ////{
    ////    playerUIHUDManager.bodyTemperatureMeter.SetStat(currentTemperature);
    ////}

    public void CloseAllMenu()
    {
        playerUICharacterMenuManager.CloseCharacterMenu();
        playerUIEquipmentManager.CloseEquipmentManagerMenu();
        playerUICharacterMenuManager.CloseSurvivalWheel();
        playerUISiteOfGraceManager.CloseSiteOfGraceManagerMenu();
        playerUITeleportLocationManager.CloseTeleportLocationManagerMenu();


    }

   
}
