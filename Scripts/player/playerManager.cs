using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerManager : CharacterManager
{
    public static playerManager instance;

    [Header("Character name")]
    public string characterName = "Character";

    [Header("Default Player Position")]
    public Vector3 defaultPlayerposition;

    [Header("Sex, HairID")]
    public bool ismale = true;
    public int hairID;

    

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
    [HideInInspector] public PlayerStatsManager playerStatsManager;
    [HideInInspector]public PlayerBodyManager playerBodyManager;
    [HideInInspector] public PlayerRespawnManager playerRespawnManager;
    [HideInInspector] public PlayerEffectsManager playerEffectsManager;
   


    public PlayerUIHUDManager playerUIHUDManager;

    public float sprintingStaminaCost = 0.2f;
    public float jumpStaminaCost = 10f;

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
        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerBodyManager = GetComponent<PlayerBodyManager>();
        playerRespawnManager = GetComponent<PlayerRespawnManager>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
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
        
        isMoving = false;
        canMove = false;
        PlayerInputManager.Instance.player.playerCombatManager.isLockedOn = false;
        PlayerInputManager.Instance.player.playerCombatManager.currentTarget = null;
        PlayerUIManager.instance.SetLockOnTarget(null);
       

        playerAnimatorManager.PlayTargetActionAnimation("Dead_01", true);

        PlayerCamera.instance.SetLockCameraHeight();
        PlayerCamera.instance.ClearLockOnTargets();


        //Disable All Boss Fight And Remove Boss HP Bar From UI
        WorldAIManager.instance.DisableAllBossFights();
        if(PlayerUIManager.instance.playerUIHUDManager.currentBossHealthBar != null)
        {
            PlayerUIManager.instance.playerUIHUDManager.currentBossHealthBar.RemoveHPBar(1);
        }

        yield return new WaitForSeconds(4.5f); // Wait for animation or death screen

        playerRespawnManager.RespawnPlayer(this);

    }

    public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
    {
        currentCharacterData.characterName= characterName; // Assign character name
        currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        currentCharacterData.timePlayed = Time.time;

        //Save Sex, Name and hairId 

        currentCharacterData.isMale = ismale;
        currentCharacterData.hairStyleID = hairID;

        // Save character position
        currentCharacterData.xPosition = transform.position.x;
        currentCharacterData.yPosition = transform.position.y;
        currentCharacterData.zPosition = transform.position.z;

        //Save Player StatBars
        currentCharacterData.currentStamina = characterStatsManager.currentStamina;
        currentCharacterData.currentHealth = characterStatsManager.currentHealth;

        //Equipment
        currentCharacterData.rightWeaponIndex = playerInventoryManager.rightHandWeaponIndex;
        currentCharacterData.rightWeapon_01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlot[0]);
        currentCharacterData.rightWeapon_02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInRightHandSlot[1]);

        currentCharacterData.leftWeaponIndex = playerInventoryManager.leftHandWeaponIndex;
        currentCharacterData.leftWeapon_01 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlot[0]);
        currentCharacterData.leftWeapon_02 = WorldSaveGameManager.instance.GetSerializableWeaponFromWeaponItem(playerInventoryManager.weaponsInLeftHandSlot[1]);


    }
    public void LoadGameDataToCharacterData(ref CharacterSaveData currentCharacterSaveData)
    {
        //Load Character Sex, Name, HairID
        characterName = currentCharacterSaveData.characterName;
        playerBodyManager.ToggleBodyType(currentCharacterSaveData.isMale);
        playerBodyManager.ToggleHairType(currentCharacterSaveData.hairStyleID);


        // Example: Load and assign position
        Vector3 myPosition = new Vector3(currentCharacterSaveData.xPosition, currentCharacterSaveData.yPosition, currentCharacterSaveData.zPosition);
        transform.position = myPosition;

        //Load Player StatBars
        characterStatsManager.currentStamina= currentCharacterSaveData.currentStamina;
        characterStatsManager.currentHealth= currentCharacterSaveData.currentHealth;

        //Update UI
        PlayerUIManager.instance.UpdateStaminaBar(Mathf.RoundToInt(characterStatsManager.currentStamina));
        PlayerUIManager.instance.UpdateHealthBar(characterStatsManager.currentHealth);

        //Load Equipments
        playerInventoryManager.currentRightHandWeapon =  playerInventoryManager.weaponsInRightHandSlot[currentCharacterSaveData.rightWeaponIndex];
        playerInventoryManager.weaponsInRightHandSlot[0]= currentCharacterSaveData.rightWeapon_01.GetWeapon();
        playerInventoryManager.weaponsInRightHandSlot[1]= currentCharacterSaveData.rightWeapon_02.GetWeapon();
        playerInventoryManager.rightHandWeaponIndex = currentCharacterSaveData.rightWeaponIndex;
        playerEquipmentManager.LoadRightWeapon();

      
        playerInventoryManager.weaponsInLeftHandSlot[0]= currentCharacterSaveData.leftWeapon_01.GetWeapon();
        playerInventoryManager.weaponsInLeftHandSlot[1]= currentCharacterSaveData.leftWeapon_02.GetWeapon();
        playerInventoryManager.leftHandWeaponIndex = currentCharacterSaveData.leftWeaponIndex;
        playerInventoryManager.currentLeftHandWeapon =  playerInventoryManager.weaponsInLeftHandSlot[currentCharacterSaveData.leftWeaponIndex];
        playerEquipmentManager.LoadLeftWeapon();

        if(currentCharacterSaveData.rightWeaponIndex >= 0)
        {
            playerInventoryManager.currentRightHandWeapon =  playerInventoryManager.weaponsInRightHandSlot[currentCharacterSaveData.rightWeaponIndex];
            playerInventoryManager.rightHandWeaponIndex =  playerInventoryManager.weaponsInRightHandSlot[currentCharacterSaveData.rightWeaponIndex].itemID;
        }
        if (currentCharacterSaveData.leftWeaponIndex >= 0)
        {
            playerInventoryManager.currentLeftHandWeapon =  playerInventoryManager.weaponsInLeftHandSlot[currentCharacterSaveData.leftWeaponIndex];
            playerInventoryManager.leftHandWeaponIndex =  playerInventoryManager.weaponsInLeftHandSlot[currentCharacterSaveData.leftWeaponIndex].itemID;
        }


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

    public void HideWeaponmodel()
    {
        if(playerInventoryManager.currentRightHandWeapon != null)
           playerEquipmentManager.rightHandWeaponModel.SetActive(false);

        if (playerInventoryManager.currentLeftHandWeapon != null)
            playerEquipmentManager.leftHandWeaponModel.SetActive(false);


    }

    public void UnHideWeaponmodel()
    {
        if (playerInventoryManager.currentRightHandWeapon != null)
            playerEquipmentManager.rightHandWeaponModel.SetActive(true);

        if (playerInventoryManager.currentLeftHandWeapon != null)
            playerEquipmentManager.leftHandWeaponModel.SetActive(true);


    }



    public override void OnIsBlocking(bool Status)
    {
        base.OnIsBlocking(Status);

        playerStatsManager.blockingPhysicalAbsorption = playerCombatManager.currentWeaponBeingUsed.physicalBaseDamageAbsorption;
        playerStatsManager.blockingFireAbsorption = playerCombatManager.currentWeaponBeingUsed.fireBaseDamageAbsorption;
        playerStatsManager.blockingMagicAbsorption = playerCombatManager.currentWeaponBeingUsed.magicBaseDamageAbsorption;
        playerStatsManager.blockingLightningAbsorption = playerCombatManager.currentWeaponBeingUsed.lightningBaseDamageAbsorption;
    }
}
