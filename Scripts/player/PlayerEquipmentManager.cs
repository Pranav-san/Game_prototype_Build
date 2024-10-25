using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : CharacterEquipmentManager
{
    playerManager player;
    public WeaponModelInstantiationSlot rightHandSlot;
    public WeaponModelInstantiationSlot leftHandSlot;

    [SerializeField] WeaponManager rightWeaponManager;
    [SerializeField] WeaponManager leftWeaponManager;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<playerManager>();
        InitializeWeaponSlots();
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
            else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
            {
                leftHandSlot = weaponSlot;
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
            leftHandSlot.UnLoadWeapon();

            // Bring In The New Weapon
            leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
            leftHandSlot.LoadWeaponModel(leftHandWeaponModel);
            leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
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
}
