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
    public SerializableDictionary<int, bool> bossesAwakened;
    public SerializableDictionary<int, bool> bossesDefeated;

    [Header("Sites Of Grace")]
    public SerializableDictionary<int, bool> sitesOfGrace; //Int is SiteOfGrace ID, Bool Is Activation Status




    [Header("Items")]
    public SerializableDictionary<int, bool> worldItemsLooted; //int is the Item ID, Bool is The Looted Status


    public CharacterSaveData()
    {

        bossesAwakened = new SerializableDictionary<int, bool>();
        bossesDefeated = new SerializableDictionary<int, bool>();


        sitesOfGrace = new SerializableDictionary<int, bool>();
        worldItemsLooted = new SerializableDictionary<int, bool>();
    }

}
