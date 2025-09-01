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
    [HideInInspector]public PlayerUIInventoryManager playerUIInventoryManager;
    [HideInInspector]public PlayerUISiteOfGraceManager playerUISiteOfGraceManager;
    [HideInInspector]public PlayerUITeleportLocationManager playerUITeleportLocationManager;
    [SerializeField] public PlayerStatsManager playerStatsManager;
    [HideInInspector] public PlayerUILoadingScreenManager playerUILoadingScreenManager;

    [Header("Puzzles")]
    [SerializeField]public  DoorLockManager doorLockManager;


    [Header("Lock-On UI")]
    public RectTransform lockOnDot; // Assign in inspector
    private Transform currentLockOnTarget;



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
        playerUILoadingScreenManager = GetComponentInChildren<PlayerUILoadingScreenManager>();
        playerUIInventoryManager = GetComponentInChildren<PlayerUIInventoryManager>();
        doorLockManager = GetComponentInChildren<DoorLockManager>();
       
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerStatsManager = playerManager.instance.playerStatsManager;
    }

    public void SetNewStaminaValue(int oldValue, float newValue)
    {
        playerUIHUDManager.staminaBar.SetStat((int)newValue);
    }

    public void SetMaxStaminaValue(int maxStamina)
    {
        playerUIHUDManager.staminaBar.SetStat(maxStamina);
    }

    public void SetLockOnTarget(Transform target)
    {
        currentLockOnTarget = target;

        if (target != null)
        {
            // Immediately place and enable the marker
            lockOnDot.position = Camera.main.WorldToScreenPoint(target.position);
            lockOnDot.gameObject.SetActive(true);
        }
        else
        {
            // Clear and hide
            lockOnDot.gameObject.SetActive(false);
        }
    }

    public void UpdateLockOnDotPosition()
    {
        if (currentLockOnTarget == null) return;

        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(currentLockOnTarget.position);

        if (targetScreenPos.z < 0)
        {
            lockOnDot.gameObject.SetActive(false);
            return;
        }

        lockOnDot.gameObject.SetActive(true);

        // Smooth but tight follow
        float smoothSpeed = 20f; // higher = snappier, lower = smoother
        lockOnDot.position = Vector3.Lerp(
            lockOnDot.position,
            targetScreenPos,
            Time.deltaTime * smoothSpeed
        );
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
        playerUIInventoryManager.CloseInventoryManagermenu();
        playerUISiteOfGraceManager.CloseSiteOfGraceManagerMenu();
        playerUITeleportLocationManager.CloseTeleportLocationManagerMenu();
        doorLockManager.CloseDoorLockUI();

      


    }

    public void RefreshAllUIAfterItemUse()
    {
        playerUIHUDManager.SetQuickSlotItemQuickSlotIcon(playerStatsManager.player.playerInventoryManager.currentQuickSlotItemID);
        playerUIEquipmentManager.RefreshWeaponSlotsIcons();
    }


}
