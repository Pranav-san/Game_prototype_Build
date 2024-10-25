using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmorItem : EquipmentItem
{
    [Header("Equipment Absorption")]
    public float physicalDamageAbsorption;
    public float magicDamageAbsorption;
    public float fireDamageAbsorption;
    public float lightiningDamageAbsorption;

    [Header("Equipment  REsistance")]
    public float imunnity;
    public float robustness;

}
