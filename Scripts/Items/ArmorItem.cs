using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmorItem : EquipmentItem
{
    


    [Header("Equipment Absorption")]
    public float physicalDamageAbsorption;
    public float magicDamageAbsorption;
    public float fireDamageAbsorption;
    public float lightningDamageAbsorption;

    [Header("Equipment  REsistance")]
    public float immunity;
    public float robustness;

    public EquipmentModel[] equipmentModels;

}
