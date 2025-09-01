using UnityEngine;

[System.Serializable]
public class SerializableQuestObjectiveProgress
{
    
        public string objectiveID;
        public QuestObjective objectiveType;
        public string targetID;
        public int requiredAmount;
        public int currentAmount;
    
}
