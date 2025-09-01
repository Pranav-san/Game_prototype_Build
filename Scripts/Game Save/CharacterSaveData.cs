using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//Reference This Data For Every SAVE FILE, This Is NOT A MONOBEHAVIOUR

public class CharacterSaveData 
{
    [Header("Scene index")]
    public int sceneIndex;
    
    
    public string characterName="Character";
    public float timePlayed;

    [Header("Body Type")]
    public bool isMale = true;
    public int hairStyleID;

    [Header("World Coordinates")]
    public float xPosition;
    public float yPosition;
    public float zPosition;

    [Header("Stat Bar")]
    public int currentHealth;
    public float currentStamina;

    [Header("Stats")]
    public int endurance;
    public int vitality;

    [Header("Bosses")]
    public SerializableDictionary<int, bool> bossesAwakened;//Int is Boss ID, Bool is Awakened Status
    public SerializableDictionary<int, bool> bossesDefeated;//Int is The Boss ID, Bool is Defeated Status

    [Header("Sites Of Grace")]
    public SerializableDictionary<int, bool> sitesOfGrace; //Int is SiteOfGrace ID, Bool Is Activation Status




    [Header("Items")]
    public SerializableDictionary<int, bool> worldItemsLooted; //int is the Item ID, Bool is The Looted Status

    [Header("Equipment")]
    public int rightWeaponIndex;
    public SerializableWeapon rightWeapon_01;
    public SerializableWeapon rightWeapon_02;

    public int leftWeaponIndex;
    public SerializableWeapon leftWeapon_01;
    public SerializableWeapon leftWeapon_02;


   


    public CharacterSaveData()
    {

        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();


        sitesOfGrace = new SerializableDictionary<int, bool>();
        worldItemsLooted = new SerializableDictionary<int, bool>();
    }

}
