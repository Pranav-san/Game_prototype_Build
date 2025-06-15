using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : MonoBehaviour
{
    public static WorldItemDatabase instance;

    public WeaponItem unArmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

    [Header("Spells")]
    [SerializeField] List<SpellItem> spells = new List<SpellItem>();

    [Header("Head Equipment")]
    [SerializeField] List<HeadEquipmentItem> headEquipment = new List<HeadEquipmentItem>();

    [Header("Body Equipment")]
    [SerializeField] List<BodyEquipmentItem> bodyEquipment = new List<BodyEquipmentItem>();

    [Header("Leg Equipment")]
    [SerializeField] List<LegEquipmentItem> legEquipment = new List<LegEquipmentItem>();

    [Header("Hand Equipment")]
    [SerializeField] List<HandEquipmentItem> handEquipment = new List<HandEquipmentItem>();

    //List Of EveryItem We Have In The Game
    private List<Item>items = new List<Item>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }

        //Add Weapons In the List Of Items
        foreach (var Weapon in weapons)
        {
            items.Add(Weapon);

        }

        foreach (var item in spells)
        {
            items.Add(item);
        }

        foreach (var item in headEquipment)
        {
            items.Add(item);

        }
        foreach (var item in bodyEquipment)
        {
            items.Add(item);

        }
        foreach (var item in legEquipment)
        {
            items.Add(item);

        }
        foreach (var item in handEquipment)
        {
            items.Add(item);

        }

        for (int i=0; i<items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon=>weapon.itemID == ID);

    }

    public SpellItem GetSpellByID(int ID)
    {
        return spells.FirstOrDefault(weapon => weapon.itemID == ID);

    }
    public HeadEquipmentItem GetHeadEquipmentByID(int ID)
    {
        return headEquipment.FirstOrDefault(equipment => equipment.itemID == ID);

    }
    public BodyEquipmentItem GetBodyEquipmentByID(int ID)
    {
        return bodyEquipment.FirstOrDefault(equipment => equipment.itemID == ID);

    }
    public LegEquipmentItem GetLegEquipmentByID(int ID)
    {
        return legEquipment.FirstOrDefault(equipment => equipment.itemID == ID);

    }

    public HandEquipmentItem GetHandEquipmentByID(int ID)
    {
        return handEquipment.FirstOrDefault(equipment => equipment.itemID == ID);

    }



}
