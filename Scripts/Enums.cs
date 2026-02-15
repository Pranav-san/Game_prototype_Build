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
    NO_SLOT
    
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
    

    leftWeapon01,//2
    leftWeapon02,//3

    TwoHandWeapon,//4

    Head,        //5
    Body,        //6
    Leg,         //7
    Hand,        //8

    QuickSlot01,//9
    QuickSlot02,//10
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
    Unarmed,
    Sword,
    spear,
    Dagger,
    Axe,
    Bow,
    Gun,
    Shield,
    
}

public enum WeaponWieldingType
{
    OneHand,
    TwoHand,
}

public enum ProjectileClass
{
    Arrow,
    Bolt, 
    HandgunAmmo,
    ShotgunShells,



}

public enum ProjectileSlot
{
    Main,
    Secondary,

}

public enum EnemyType
{
    melee,
    Ranged,
    exploder,
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
    Patrol,
    Sleep,

}

//Used To Give Characters Proper DialogueSets
public enum characterDialogueID
{
    NoDialogueID,
    NameLessKnightDialogueID,
}

//Inventory Items Category
public enum InventoryCategory
{
    All,
    Weapons,
    Armor,
    Consumables
}

//Quests

public enum QuestObjective
{
    Kill,
    collectItem,
    Talk,
}


//Puzzles

public enum PasscodeButtonType
{
    Digit0,
    Digit1,
    Digit2,
    Digit3,
    Digit4,
    Digit5,
    Digit6,
    Digit7,
    Digit8,
    Digit9,
}

//Used For Foot Step SFX
public enum SurfaceType
{
    Default,
    Snow,
    Grass,
    Floor,
    Wood,
    Metal,
}

//Used for Playing Atmosphere Ambient SFX

public enum LocationType
{
    Indoor,
    OutDoor,
}

//Used To Tag Sliders For LevelUp UI
public enum CharacterAttributes
{
    Vitality,
    Endurance,
    Strength,
    Dexterity,
    Luck
}






