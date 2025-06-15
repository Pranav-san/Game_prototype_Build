using UnityEngine;

[System.Serializable]
public class CharacterClass
{
    [Header("Class Information")]
    public string className;

    [Header("class Stats")]
    public int vitality = 10;
    public int stamina = 10;
    public int mind = 10;
    public int strength = 10;
    public int dexterity = 10;
    public int faith = 10;

    [Header("Class Weapons")]
    public WeaponItem[] mainHandWeapons = new WeaponItem[3];
    public WeaponItem[] offHandWeapons = new WeaponItem[3];

    [Header("Class Armor")]
    public HeadEquipmentItem headEquipment;
    public BodyEquipmentItem bodyEquipment;
    public LegEquipmentItem legEquipment;
    public HandEquipmentItem handEquipment;

    //[Header("Quick Slot items")]

    public void SetClass(playerManager player)
    {
        TitleScreenManager.Instance.SetCharacterClass(player, vitality, stamina, mind, strength, dexterity, faith, 
            mainHandWeapons,offHandWeapons, headEquipment, bodyEquipment, legEquipment, handEquipment);
    }
    
    
}
