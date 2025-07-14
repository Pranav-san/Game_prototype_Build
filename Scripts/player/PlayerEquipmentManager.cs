using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    playerManager player;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandWeaponSlot;
    public WeaponModelInstantiationSlot leftHandShieldSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    [Header("Debug Delete Later")]
    [SerializeField] bool equipNewItem = false;

    [Header("Two Handing")]
    [SerializeField]  public bool isTwoHandingWeapon = false;
    [SerializeField] public int currentWeaponBeingTwoHanded;

    [Header("Notched Arrow")]
    public GameObject notchedArrow;


    [Header("General Equipments Models")]
    [HideInInspector]public GameObject HatsObject;
    [HideInInspector] public GameObject[] HalfHelmets;
    public GameObject HoodsObject;
    public GameObject[] hoods;


    [Header("Male Equipment Models")]
    public GameObject maleFullHelmetObject;
    [HideInInspector]public GameObject[] maleHeadFullHelmets;
    public GameObject maleFullBodyObject;
    [HideInInspector] public GameObject[] maleBodies;
    public GameObject maleFullLegObject;
    [HideInInspector]public GameObject[] maleFullLegArmor;
    public GameObject maleFullHandObject;
    [HideInInspector] public GameObject[] maleFullHandArmor;


    private void Update()
    {
        if(equipNewItem)
        {
            equipNewItem = false ;
            DebugEquipNewItems();
        }
    }

    public void DebugEquipNewItems()
    {
        Debug.Log("Equipping new Items");
        
        LoadHeadEquipment(player.playerInventoryManager.headEquipment);
        
        
        LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
       
        
        LoadLegEquipment(player.playerInventoryManager.legEquipment);

        LoadHandEquipment(player.playerInventoryManager.handEquipment);
        

    }

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<playerManager>();
        InitializeWeaponSlots();

        
        List<GameObject> Hoodslist = new List<GameObject>();

        foreach (Transform child in HoodsObject.transform)
        {
            Hoodslist.Add(child.gameObject);

        }
        hoods = Hoodslist.ToArray();







        //Helmet
        List<GameObject> maleFullHelmetlist = new List<GameObject>();

        foreach (Transform child in maleFullHelmetObject.transform)
        {
            maleFullHelmetlist.Add(child.gameObject);

        }
        maleHeadFullHelmets = maleFullHelmetlist.ToArray();


        //UpperBody
        List<GameObject> maleBodieslist = new List<GameObject>();

        foreach (Transform child in maleFullBodyObject.transform)
        {
            maleBodieslist.Add(child.gameObject);

        }
        maleBodies = maleBodieslist.ToArray();

        //Lower Body
        List<GameObject> maleFullLegArmorlist = new List<GameObject>();

        foreach (Transform child in maleFullLegObject.transform)
        {
            maleFullLegArmorlist.Add(child.gameObject);

        }
        maleFullLegArmor = maleFullLegArmorlist.ToArray();


        //Arm
        List<GameObject> maleFullHandArmorlist = new List<GameObject>();

        foreach (Transform child in maleFullHandObject.transform)
        {
            maleFullHandArmorlist.Add(child.gameObject);

        }
        maleFullHandArmor = maleFullHandArmorlist.ToArray();
    }

    protected override void Start()
    {
        base.Start();

        LoadWeaponOnBothHands();
        IgnoreMyOwnColliders();
    }

    private void InitializeWeaponSlots()
    {
        WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

        foreach (var weaponSlot in weaponSlots)
        {
            if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
            {
                leftHandWeaponSlot = weaponSlot;
            }
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
            {
                leftHandShieldSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnBothHands()
    {
        LoadLeftWeapon();
        LoadRightWeapon();
    }

    // Right
    public void LoadRightWeapon()
    {
        if (player.playerInventoryManager.currentRightHandWeapon != null)
        {
            // Remove/Destroy The Old Weapon
            rightHandSlot.UnLoadWeapon();

            // Bring In the New Weapon
            rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
            rightHandSlot.LoadWeaponModel(rightHandWeaponModel);
            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
        }
    }

    public void SwitchRightWeapon()
    {
        WeaponItem selectedWeapon = null;

        // Add 1 to index to switch to the next weapon
        player.playerInventoryManager.rightHandWeaponIndex += 1;

        if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
        {
            player.playerInventoryManager.rightHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlot.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlot[i].itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInRightHandSlot[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.rightHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unArmedWeapon;
                player.playerInventoryManager.currentRightHandWeaponID = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentRightHandWeaponID = firstWeapon.itemID;
            }

            player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(0, player.playerInventoryManager.currentRightHandWeaponID);
            return;
        }

        for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlot.Length; i++)
        {
            if (player.playerInventoryManager.weaponsInRightHandSlot[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlot[player.playerInventoryManager.rightHandWeaponIndex];
                player.playerInventoryManager.currentRightHandWeaponID = player.playerInventoryManager.weaponsInRightHandSlot[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                player.playerInventoryManager.OnCurrentRightHandWeaponIDChange(0, player.playerInventoryManager.currentRightHandWeaponID);
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
        {
            SwitchRightWeapon();
        }
    }

    // Left
    public void LoadLeftWeapon()
    {
        if (player.playerInventoryManager.currentLeftHandWeapon != null)
        {
            // Remove/Destroy Old Weapon
            if(leftHandWeaponSlot.currentWeaponModel!=null)
                leftHandWeaponSlot.UnLoadWeapon();
            if(leftHandShieldSlot.currentWeaponModel != null)
                leftHandShieldSlot.UnLoadWeapon();

            // Bring In The New Weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

            switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
            {
                case WeaponModelType.Weapon:
                    leftHandWeaponSlot.LoadWeaponModel(leftHandWeaponModel);
                    break;
                case WeaponModelType.Shield:
                    leftHandShieldSlot.LoadWeaponModel(leftHandWeaponModel);
                    break;
                default:
                    break;
            }


            
            
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
        }
    }

    public void SwitchLeftWeapon()
    {
        WeaponItem selectedWeapon = null;

        // Add 1 to index to switch to the next weapon
        player.playerInventoryManager.leftHandWeaponIndex += 1;

        if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
        {
            player.playerInventoryManager.leftHandWeaponIndex = 0;

            float weaponCount = 0;
            WeaponItem firstWeapon = null;
            int firstWeaponPosition = 0;

            for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlot.Length; i++)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlot[i].itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
                {
                    weaponCount += 1;
                    if (firstWeapon == null)
                    {
                        firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[i];
                        firstWeaponPosition = i;
                    }
                }
            }

            if (weaponCount <= 1)
            {
                player.playerInventoryManager.leftHandWeaponIndex = -1;
                selectedWeapon = WorldItemDatabase.instance.unArmedWeapon;
                player.playerInventoryManager.currentLeftHandWeaponID = selectedWeapon.itemID;
            }
            else
            {
                player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                player.playerInventoryManager.currentLeftHandWeaponID = firstWeapon.itemID;
            }

            player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(0, player.playerInventoryManager.currentLeftHandWeaponID);
            return;
        }

        for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlot.Length; i++)
        {
            if (player.playerInventoryManager.weaponsInLeftHandSlot[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unArmedWeapon.itemID)
            {
                selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlot[player.playerInventoryManager.leftHandWeaponIndex];
                player.playerInventoryManager.currentLeftHandWeaponID = player.playerInventoryManager.weaponsInLeftHandSlot[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                player.playerInventoryManager.OnCurrentLeftHandWeaponIDChange(0, player.playerInventoryManager.currentLeftHandWeaponID);
                return;
            }
        }

        if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
        {
            SwitchLeftWeapon();
        }

    }

   
    public void LoadHeadEquipment(HeadEquipmentItem equipment)
    {
        // 1. UNLOAD OLD EQUIPMENT MODELS (IF ANY)
        UnloadHeadEquipmentModel();

        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if(equipment == null)
        {
            player.playerInventoryManager.headEquipment = null;
            return;
        }

        // 3. IF YOU HAVE AN "ONITEMEQUIPPED" CALL ON YOUR EQUIPMENT, RUN IT NOW

        // 4. SET CURRENT EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.headEquipment = equipment;

        // 5. LOAD EQUIPMENT MODELS

        switch (equipment.headEquipmentType)
        {
            case HeadEquipmentType.FullHelmet:
                player.playerBodyManager.DisableHair();
                player.playerBodyManager.DisableHead();
                break;
            case HeadEquipmentType.Hat:
                player.playerBodyManager.DisableHair();
                break;
            case HeadEquipmentType.Hood:
                player.playerBodyManager.DisableHair();
                break;
            case HeadEquipmentType.FaceCover:
                player.playerBodyManager.DisableFacialHair();
                break;
            default:
                break;
        }
        foreach(var model in equipment.equipmentModels)
        {
            model.LoadModel(player, true);

        }
        // 6. CALCULATE TOTAL EQUIPMENT LOAD (WEIGHT OF ALL YOUR WORN EQUIPMENT. THIS IMPACTS ROLL SPEED AND AT EXTREME WEIGHTS, MOVEMENT SPEED)
        // 7. CALCULATE TOTAL ARMOR ABSORPTION
        player.playerStatsManager.calculateTotalArmorAbsorption();

        
    }

    private void UnloadHeadEquipmentModel()
    {
        foreach (var model in maleHeadFullHelmets)
        {
            model.SetActive(false);
        }
        foreach (var model in HalfHelmets)
        {
            model.SetActive(false);
        }
        foreach (var model in hoods)
        {
            model.SetActive(false);
        }

        //Re-Enable Head
        player.playerBodyManager.EnableHead();
        //Re-Enable Hair
        player.playerBodyManager.EnableHair();

    }



    public void LoadBodyEquipment(BodyEquipmentItem equipment)
    {
        // 1. UNLOAD OLD EQUIPMENT MODELS (IF ANY)
        UnLoadBodyEquipmentModels();
        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            player.playerInventoryManager.bodyEquipment = null;
            return;
        }

        // 3. IF YOU HAVE AN "ONITEMEQUIPPED" CALL ON YOUR EQUIPMENT, RUN IT NOW

        // 4. SET CURRENT EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.bodyEquipment = equipment;

        // 5. LOAD EQUIPMENT MODELS DISABLE BODY PARTS
        player.playerBodyManager.DisableBody();

        player.playerBodyManager.DisableDefaultUpperBodyClothes();

       
        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, true);

        }
        // 6. CALCULATE TOTAL EQUIPMENT LOAD (WEIGHT OF ALL YOUR WORN EQUIPMENT. THIS IMPACTS ROLL SPEED AND AT EXTREME WEIGHTS, MOVEMENT SPEED)
        // 7. CALCULATE TOTAL ARMOR ABSORPTION
        player.playerStatsManager.calculateTotalArmorAbsorption();
    }

    private void UnLoadBodyEquipmentModels()
    {

        foreach(var model in maleBodies)
        {
            model.SetActive(false);

        }

        //Re-Eable Body
        player.playerBodyManager.EnableBody();

        //Re-Enable Default Upper Clothes 
        player.playerBodyManager.EnableDefaultUpperBodyClothes();

    }

    public void LoadLegEquipment(LegEquipmentItem equipment)
    {
        // 1. UNLOAD OLD EQUIPMENT MODELS (IF ANY)
        UnLoadLegEquipmentModels();
        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            player.playerInventoryManager.legEquipment = null;
            return;
        }
        // 5. LOAD EQUIPMENT MODELS DISABLE BODY PARTS
        player.playerBodyManager.DisableLowerBody();

        player.playerBodyManager.DisableDefaultLowerBodyClothes();


        // 3. IF YOU HAVE AN "ONITEMEQUIPPED" CALL ON YOUR EQUIPMENT, RUN IT NOW



        // 4. SET CURRENT EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.legEquipment = equipment;

        // 5. LOAD EQUIPMENT MODELS


        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, true);

        }
        // 6. CALCULATE TOTAL EQUIPMENT LOAD (WEIGHT OF ALL YOUR WORN EQUIPMENT. THIS IMPACTS ROLL SPEED AND AT EXTREME WEIGHTS, MOVEMENT SPEED)
        // 7. CALCULATE TOTAL ARMOR ABSORPTION
        player.playerStatsManager.calculateTotalArmorAbsorption();
    }

    private void UnLoadLegEquipmentModels()
    {
        foreach (var model in maleFullLegArmor)
        {
            model.SetActive(false);

        }

        //Re-Enable Lower Body
        player.playerBodyManager.EnableLowerBody();

        //Re-Enable Default Lower Clothes
        player.playerBodyManager.EnableDefaultLowerBodyClothes();

    }

    public void LoadHandEquipment(HandEquipmentItem equipment)
    {
        // 1. UNLOAD OLD EQUIPMENT MODELS (IF ANY)
        UnLoadHandEquipmentModels();
        
        // 2. IF EQUIPMENT IS NULL SIMPLY SET EQUIPMENT IN INVENTORY TO NULL AND RETURN
        if (equipment == null)
        {
            player.playerInventoryManager.legEquipment = null;
            return;
        }

        // 3. IF YOU HAVE AN "ONITEMEQUIPPED" CALL ON YOUR EQUIPMENT, RUN IT NOW



        // 4. SET CURRENT EQUIPMENT IN PLAYER INVENTORY TO THE EQUIPMENT THAT IS PASSED TO THIS FUNCTION
        player.playerInventoryManager.handEquipment = equipment;

        // 5. LOAD EQUIPMENT MODELS


        foreach (var model in equipment.equipmentModels)
        {
            model.LoadModel(player, true);

        }
        // 6. CALCULATE TOTAL EQUIPMENT LOAD (WEIGHT OF ALL YOUR WORN EQUIPMENT. THIS IMPACTS ROLL SPEED AND AT EXTREME WEIGHTS, MOVEMENT SPEED)
        // 7. CALCULATE TOTAL ARMOR ABSORPTION
        player.playerStatsManager.calculateTotalArmorAbsorption();
    }

    private void UnLoadHandEquipmentModels()
    {
        foreach (var model in maleFullHandArmor)
        {
            model.SetActive(false);

        }

    }



    public void OnHeadEquipmentValueChanged(int oldValue, int newValue)
    {
        Debug.Log("Head equipment changed: Old=" + oldValue + ", New=" + newValue);

        UnloadHeadEquipmentModel();

        HeadEquipmentItem newEquipment = WorldItemDatabase.instance.GetHeadEquipmentByID(newValue);

        if (newEquipment != null)
        {
            LoadHeadEquipment(newEquipment);
        }
        else
        {
            LoadHeadEquipment(null);
        }

        
        
    }

    



    protected virtual void IgnoreMyOwnColliders()
    {
        Collider chararacterControllerCollider = GetComponent<Collider>();
        Collider[] damageableCharactersColliders = GetComponentsInChildren<Collider>();

        List <Collider> ignoreColliders = new List <Collider>();

        foreach(var collider in damageableCharactersColliders)
        {
            ignoreColliders.Add(collider);  
        }
        ignoreColliders.Add(chararacterControllerCollider);


        foreach(var collider in ignoreColliders)
        {
            foreach(var otherColliders in ignoreColliders)
            {
                Physics.IgnoreCollision(collider, otherColliders,true);
            }
        }
    }

    //Damage Colliders

    public void OpenDamageCollider()
    {
        if(player.isUsingRightHand)
        {
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();

            player.characterSoundFxManager.PlaySoundfx(WorldSoundFXManager.instance.ChooseRandomSoundFxFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));

        }
        else if(player.isUsingLeftHand)
        {
            leftWeaponManager.meleeDamageCollider.EnableDamageCollider();

        }
    }

    public void CloseDamageCollider()
    {
        if (player.isUsingRightHand)
        {
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();

        }
        else if (player.isUsingLeftHand)
        {
            leftWeaponManager.meleeDamageCollider.DisableDamageCollider();

        }
    }

    //Two Hand
    public void UnTwoHandWeapon()
    {
        WeaponItem weapon = player.playerInventoryManager.currentTwoHandWeapon;
        isTwoHandingWeapon = false;
        
        
        if (weapon.weaponClass == WeapomClass.Bow)
        {
            leftHandWeaponSlot.UnLoadWeapon();
            if (leftHandWeaponModel != null)
                Destroy(leftHandWeaponModel);

            rightHandWeaponModel = Instantiate(weapon.weaponModel);
            rightHandSlot.LoadWeaponModel(rightHandWeaponModel);

            rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
            rightWeaponManager.SetWeaponDamage(player, weapon);
        }

        //Update Animator Controller To Current Main Hand Weapon
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

        rightHandSlot.LoadWeaponModel(rightHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);

        player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);

    }
    public void TwoHandRightWeapon()
    {
        WeaponItem weapon = player.playerInventoryManager.currentRightHandWeapon;

        //Check For UnTwoHandable Item (Like Unarmed)
        if (player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.instance.unArmedWeapon)
        {

            isTwoHandingWeapon = false;
            return;

        }

        if (weapon.weaponClass == WeapomClass.Bow)
        {
            isTwoHandingWeapon = true;
            
            currentWeaponBeingTwoHanded = weapon.itemID;
            player.playerInventoryManager.currentTwoHandWeapon = weapon;

            // Remove from right hand
            rightHandSlot.UnLoadWeapon();
            if (rightHandWeaponModel != null)
                Destroy(rightHandWeaponModel);
            //if(leftHandWeaponModel != null) Destroy(leftHandWeaponModel);

            // Instantiate into LEFT hand
            leftHandWeaponModel = Instantiate(weapon.weaponModel);

            




            leftHandWeaponSlot.LoadWeaponModel(leftHandWeaponModel);

            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, weapon);

            player.playerAnimatorManager.UpdateAnimatorController(weapon.weaponAnimator);
            player.animator.SetBool("isTwoHandingWeapon", true);
        }

        //Update Animator Controller
        player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

        rightHandSlot.LoadWeaponModel(rightHandWeaponModel);

        rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);




    }

    public void DrawProjectile(int projectileID)
    {
        Animator bowAnimator;
        bowAnimator = player.playerInventoryManager.currentRightHandWeapon.weaponModel.GetComponentInChildren<Animator>();

        if (bowAnimator == null)
            return;





        //Animate The Bow
        bowAnimator.SetBool("isDrawn", true);
        bowAnimator.Play("Bow_Draw_01");

        //Instantiate Arrow

        notchedArrow = Instantiate(WorldItemDatabase.instance.GetProjectileByID(projectileID).drawProjectileModel, 
            player.playerEquipmentManager.rightHandSlot.transform);
       





    }
    public void OnIsTwoHandingRightWeapon(bool oldstatus, bool newstatus)
    {

       
        currentWeaponBeingTwoHanded = player.playerInventoryManager.currentRightHandWeaponID;
        isTwoHandingWeapon = true;
        player.playerInventoryManager.currentTwoHandWeapon = player.playerInventoryManager.currentRightHandWeapon;
        TwoHandRightWeapon();

        player.animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);

    }

    public void OnIsTwoHandingWeapon(bool oldstatus, bool newstatus)
    {
        if(!isTwoHandingWeapon) 
            UnTwoHandWeapon();

      
    }
    
}
