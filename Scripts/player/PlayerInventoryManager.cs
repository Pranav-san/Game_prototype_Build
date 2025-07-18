using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerInventoryManager : CharacterInventoryManager
{
    [SerializeField] playerManager player;
    public WeaponItem currentRightHandWeapon;
    public WeaponItem currentLeftHandWeapon;
    public WeaponItem currentTwoHandWeapon;

    [Header("Quick Slots")]
    public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[3];
    public int rightHandWeaponIndex = 0;
    public int leftHandWeaponIndex = 0;

    public SpellItem currentSpell;

    [Header("Equipment")]
    public int currentRightHandWeaponID = 0;
    public int currentLeftHandWeaponID = 0;
    public int currentSpellID=0;
    public int currentMainProjectileID=0;
  

    [Header("Aromo/OutFit Equipment")]
    public HeadEquipmentItem headEquipment;
    public BodyEquipmentItem bodyEquipment;
    public LegEquipmentItem legEquipment;
    public HandEquipmentItem handEquipment;

    [Header("Projectiles")]
    public RangedProjectileItem mainProjectile;
    public RangedProjectileItem secondaryProjectile;
    public bool hasArrowNotched = false;
    public bool isHoldingArrow = false;

    [Header("Inventory")]
    public List<Item> itemsInInventory;
    


    public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[3];
    public int rightLeftWeaponIndex = 0;

    private void Start()
    {
        OnCurrentRightHandWeaponIDChange(0, currentRightHandWeaponID);
        OnCurrentLeftHandWeaponIDChange(0, currentLeftHandWeaponID);
        OnCurrentHandWeaponBeingUsedIDChange(0, currentRightHandWeaponID);
        OnCurrentSpellIDChange(0, currentSpellID);
        OnMainProjectileIDChange(0, currentMainProjectileID);

        player= GetComponent<playerManager>();
    }

    public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        currentRightHandWeapon = newWeapon;


        player.playerEquipmentManager.LoadRightWeapon();

        PlayerUIManager.instance.playerUIHUDManager.SetRightweaponQuickSlotIcon(newID);
    }

    public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        currentLeftHandWeapon = newWeapon;
        player.playerEquipmentManager.LoadLeftWeapon();


        PlayerUIManager.instance.playerUIHUDManager.SetLeftweaponQuickSlotIcon(newID);
    }

    public void OnCurrentSpellIDChange(int oldID, int newID)
    {
        SpellItem newSpell = Instantiate(WorldItemDatabase.instance.GetSpellByID(newID));
        currentSpell= newSpell;
       
        

        
    }

    public void OnMainProjectileIDChange(int oldID, int newID)
    {

        RangedProjectileItem newProjectile = null;

        if (WorldItemDatabase.instance.GetProjectileByID(newID))
            newProjectile = Instantiate(WorldItemDatabase.instance.GetProjectileByID(newID));

        if (newProjectile != null)
            player.playerInventoryManager.mainProjectile = newProjectile;
       



    }

    public void OnSecondaryProjectileIDChange(int oldID, int newID)
    {

        RangedProjectileItem newProjectile = null;

        if (WorldItemDatabase.instance.GetProjectileByID(newID))
            newProjectile = Instantiate(WorldItemDatabase.instance.GetProjectileByID(newID));

        if (newProjectile != null)
            player.playerInventoryManager.mainProjectile = newProjectile;




    }

    public void OnHeadEquipmentModelChanged(int oldValue, int newValue)
    {
       HeadEquipmentItem equipment = WorldItemDatabase.instance.GetHeadEquipmentByID(newValue);

        if(headEquipment != null)
        {
            player.playerEquipmentManager.LoadHeadEquipment(Instantiate(equipment));

        }
        else
        {
            player.playerEquipmentManager.LoadHeadEquipment(null);
        }

        player.playerEquipmentManager.OnHeadEquipmentValueChanged(oldValue, newValue);

    }
    public void OnCurrentHandWeaponBeingUsedIDChange(int oldID, int newID)
    {
        WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
        player.playerCombatManager.currentWeaponBeingUsed = newWeapon;
        
    }

    

    public void PerformWeaponBasedAction(int actionID, int weaponID)
    {
        WeaponItemBasedAction weaponAction = WorldActionManager.Instance.GetWeaponItemActionByID(actionID);

        if (weaponAction != null)
        {
            weaponAction.AttemptToPerformAction(player, WorldItemDatabase.instance.GetWeaponByID(weaponID));

        }
        else
        {
            Debug.LogError("Action is null");
        }
    }

    public void AddItemToInventory(Item item)
    {
        itemsInInventory.Add(item);

    }
    public void RemoveItemToInventory(Item item)
    {
        //itemsInInventory.Remove(item);

        for (int i = 0; i < itemsInInventory.Count; i++)
        {
            if (itemsInInventory[i].itemID == item.itemID) // Compare item ID
            {
                itemsInInventory.RemoveAt(i);
                break; // Exit the loop after removing one instance
            }
        }


    }

}
