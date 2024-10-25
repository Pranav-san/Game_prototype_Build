using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerManager : CharacterManager
{
    public static playerManager instance;

    [Header("Debug Menu")]
    [SerializeField] bool ReSpwanCharacter = false;
    [SerializeField] bool switchRightWeapon = false;
    [SerializeField] public bool switchLeftWeapon = false;
    [SerializeField] public  bool isUsingRightHand = false;
    [SerializeField] public bool isUsingLeftHand = false;

    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [SerializeField] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
    [HideInInspector] public PlayerCombatManager playerCombatManager;
    [HideInInspector] public PlayerInteractionManager playerInteractionManager;
    [HideInInspector] public CharacterSoundFxManager characterSoundFxManager;
   
    public PlayerUIHUDManager playerUIHUDManager;

    public float sprintingStaminaCost = 0.2f;

    protected override void Awake()
    {
        base.Awake();
        
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        


        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerUIHUDManager = GetComponent<PlayerUIHUDManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerInteractionManager = GetComponent<PlayerInteractionManager>();  
        characterSoundFxManager = GetComponent<CharacterSoundFxManager>();
    }

    protected override void Update()
    {
        base.Update();
        playerLocomotionManager.HandleAllMovement();

        DebugMenu();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
        PlayerCamera.instance.HandleAllCameraActions();
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        PlayerUIManager.instance.playerUIPopUPManager.SendYouDiedPopUp();
        return base.ProcessDeathEvent(manuallySelectDeathAnimation);
    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        currentCharacterData.characterName = "Character"; // Assign player name
        currentCharacterData.timePlayed = Time.time;

        // Save player position
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        //Save Player StatBars
        currentCharacterData.currentStamina = characterStatsManager.currentStamina;
        currentCharacterData.currentHealth = characterStatsManager.currentHealth;

    }
    public void LoadGameDataToCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName = "Character";

        // Example: Load and assign position
        Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        transform.position = myPosition;

        //Load Player StatBars
        characterStatsManager.currentStamina= currentCharacterData.currentStamina;
        characterStatsManager.currentHealth= currentCharacterData.currentHealth;

        //Update UI
        PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(characterStatsManager.currentStamina));
        PlayerUIManager.instance.UpdateHealthBar(characterStatsManager.currentHealth);

    }

    public override void ReviveCharacter()
    {
        base.ReviveCharacter();

        characterStatsManager.currentHealth = characterStatsManager.maxHealth;
        characterStatsManager.currentStamina = characterStatsManager.maxStamina;

        playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
    }

    private void DebugMenu()
    {
        if (ReSpwanCharacter)
        {
            ReSpwanCharacter = false;
            ReviveCharacter();
        }
        if(switchRightWeapon)
        {
            switchRightWeapon = false;
            playerEquipmentManager.SwitchRightWeapon();
        }
        if(switchLeftWeapon)
        {
            switchLeftWeapon = false;
            playerEquipmentManager.SwitchLeftWeapon();
        }
        
    }
}
