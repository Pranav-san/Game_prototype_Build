using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    
}
public enum CharacterSlot
{
    CharacterSlot_01,
    CharacterSlot_02,
    CharacterSlot_03,
    CharacterSlot_04,
    CharacterSlot_05,
    CharacterSlot_06,
    CharacterSlot_07,
    CharacterSlot_08,
    CharacterSlot_09,
    CharacterSlot_10,
    N0_SLOT
    
}
public enum WeaponModelSlot{

    RightHand,
    LeftHand,
    //RightHip
    //LeftHip
    //Back

}

public enum CharacterGroup
{
    Team01,
    Team02,
}

public enum AttackType
{
    LightAttack01,
    LightAttack02,
}

public enum EquipmentType
{
    RightWeapon01,//0
    RightWeapon02,//1
    RightWeapon03,//2

    leftWeapon01,//3
    leftWeapon02,//4
    leftWeapon03,//5
}


public enum ItemPickUpType
{
    WorldSpwan,
    CharacterDrop
}
