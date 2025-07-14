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
    LeftHandWeaponSlot,
    LeftHandShieldSlot,
    leftHandBowSlot,

    //RightHip
    //LeftHip
    //Back

}

public enum WeaponModelType
{
    Weapon,
    Shield,
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

    HeavyAttack01,
    ChargedAttack01,
    ChargedAttack02,
}

public enum DamageIntensity
{
    Ping,
    Light,
    Medium,
    Heavy,
    Collasal
}

public enum EquipmentType
{
    RightWeapon01,//0
    RightWeapon02,//1
    RightWeapon03,//2

    leftWeapon01,//3
    leftWeapon02,//4
    leftWeapon03,//5

    Head,        //6
    Body,        //7
    Leg,         //8
    Hand,        //9
}

public enum EquipmentModelType
{
    FullHelmet, //Hide Face, Hair...ETC
    Hat,//Hide Hair
    Hood,////    Hide Hair,
    RightShoulder,
    RightUpperArm,
    RightElbow,
    RightLowerArm,
    LeftHand,
    LeftShoulder,
    LeftUpperArm,
    LeftElbow,
    LeftLowerArm,
    RightHand,

    Torso,
    Back,
    Hips,

    RightLeg,
    RightKnee,
    LeftLeg,
    LeftKnee,
}
public enum HeadEquipmentType
{
    FullHelmet, // HIDE ENTIRE HEAD + FEATURES
    Hat, // DOES NOT HIDE ANYTHING
    Hood, // HIDES HAIR
    FaceCover // HIDES BEARD
}


//Used to Determine Which Item is Needed to Cast Spell
public enum SpellClass
{
    Incantation,
    Sorcery

}

public enum WeapomClass
{
    Sword,
    spear,
    Bow,
    Fist,
    Shield
}

public enum ProjectileClass
{
    Arrow,
    Bolt

}

public enum ProjectileSlot
{
    Main,
    Secondary,

}


//Used To Determine Pick Up Item Type
public enum ItemPickUpType
{
    WorldSpwan,
    CharacterDrop
}

//AI States

public enum IdleStateMode
{
    Idle,
    Patrol
}




