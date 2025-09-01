using UnityEngine;

[System.Serializable]
public class SerializableQuestObjective
{
    public string objectiveID; // e.g., "KillRat"
    public QuestObjective objectiveType; // your enum
    public string targetID; // e.g., enemy ID or item ID
    public int requiredAmount;
    public int currentAmount;
}
