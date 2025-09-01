using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableActiveQuest
{
    public Quest questData;
    public List<SerializableQuestObjective> objectives = new List<SerializableQuestObjective>();

    public SerializableActiveQuest(Quest quest)
    {
        questData = quest;

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new SerializableQuestObjective
            {
                objectiveID = obj.objectiveID,
                objectiveType = obj.objectiveType,
                targetID = obj.targetID,
                requiredAmount = obj.requiredAmount,
                currentAmount = 0
            });
        }
    }

    public bool IsComplete()
    {
        foreach (var obj in objectives)
        {
            if (obj.currentAmount < obj.requiredAmount)
                return false;
        }
        return true;
    }
}

