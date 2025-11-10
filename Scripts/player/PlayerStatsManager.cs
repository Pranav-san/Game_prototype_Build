using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    

    [Header("Stamina Costs")]
    public float sprintingStaminaCost = 5f;
    public float jumpStaminaCost = 10f;
    public float dodgeStaminaCost = 15f;




    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<playerManager>();


    }

    public void calculateTotalArmorAbsorption()
    {
        //Reset All Values to Zero
        ArmorPhysicalDamageAbsorption =0;
        ArmorLightningDamageAbsorption=0;
        ArmorMagicDamageAbsorption = 0;
        ArmorFireDamageAbsorption = 0;

        ArmorImmunity = 0;
        ArmorRobustness = 0;

        //Head Equipment
        if (player.playerInventoryManager.headEquipment!=null)
        {
            // DAMAGE RESISTANCE
            ArmorPhysicalDamageAbsorption += player.playerInventoryManager.headEquipment.physicalDamageAbsorption;
            ArmorMagicDamageAbsorption += player.playerInventoryManager.headEquipment.magicDamageAbsorption;
            ArmorFireDamageAbsorption += player.playerInventoryManager.headEquipment.fireDamageAbsorption;
            ArmorLightningDamageAbsorption += player.playerInventoryManager.headEquipment.lightningDamageAbsorption;
            
            // STATUS EFFECT RESISTANCE
            ArmorRobustness += player.playerInventoryManager.headEquipment.robustness;
            ArmorImmunity += player.playerInventoryManager.headEquipment.immunity;
           
        }
        //Body Equipment
        if (player.playerInventoryManager.bodyEquipment!=null)
        {

            // DAMAGE RESISTANCE
            ArmorPhysicalDamageAbsorption += player.playerInventoryManager.bodyEquipment.physicalDamageAbsorption;
            ArmorMagicDamageAbsorption += player.playerInventoryManager.bodyEquipment.magicDamageAbsorption;
            ArmorFireDamageAbsorption += player.playerInventoryManager.bodyEquipment.fireDamageAbsorption;
            ArmorLightningDamageAbsorption += player.playerInventoryManager.bodyEquipment.lightningDamageAbsorption;

            // STATUS EFFECT RESISTANCE
            ArmorRobustness += player.playerInventoryManager.bodyEquipment.robustness;
            ArmorImmunity += player.playerInventoryManager.bodyEquipment.immunity;

        }
        //Leg Equipment
        if (player.playerInventoryManager.legEquipment!=null)
        {

            // DAMAGE RESISTANCE
            ArmorPhysicalDamageAbsorption += player.playerInventoryManager.legEquipment.physicalDamageAbsorption;
            ArmorMagicDamageAbsorption += player.playerInventoryManager.legEquipment.magicDamageAbsorption;
            ArmorFireDamageAbsorption += player.playerInventoryManager.legEquipment.fireDamageAbsorption;
            ArmorLightningDamageAbsorption += player.playerInventoryManager.legEquipment.lightningDamageAbsorption;

            // STATUS EFFECT RESISTANCE
            ArmorRobustness += player.playerInventoryManager.legEquipment.robustness;
            ArmorImmunity += player.playerInventoryManager.legEquipment.immunity;

        }
    }

    public void AddRunes(int runesToAdd)
    {
        runes += runesToAdd;
        PlayerUIManager.instance.playerUIHUDManager.SetRunesCount(runesToAdd);
    }

    





}
