using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest System/Quest")]
public class Quest : ScriptableObject
{
    [Header("Quest Info")]
    public string questID;
    public string questName;
    [TextArea] public string description;

    [Header("Objectives")]
    public List<SerializableQuestObjective> objectives = new List<SerializableQuestObjective>();

    [Header("Rewards")]
    public int experienceReward;
    public Item ItemReward;
}
