using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestManager : MonoBehaviour
{
    public List<SerializableActiveQuest> activeQuests = new List<SerializableActiveQuest>();
    public List<string> completedQuestIDs = new List<string>();

    public void AddQuest(Quest newQuest)
    {
        if (HasQuest(newQuest.questID) || completedQuestIDs.Contains(newQuest.questID))
            return;

        //activeQuests.Add(new ActiveQuest(newQuest));
        Debug.Log("Quest added: " + newQuest.questName);
    }

    public bool HasQuest(string questID)
    {
        return activeQuests.Exists(q => q.questData.questID == questID);
    }

    //public void CompleteQuest(string questID)
    //{
    //    ActiveQuest quest = activeQuests.Find(q => q.questData.questID == questID);
    //    if (quest != null)
    //    {
    //        activeQuests.Remove(quest);
    //        completedQuestIDs.Add(questID);

    //        // Give rewards
    //        Debug.Log($"Completed Quest: {quest.questData.questName}");
    //        // TODO: Add experience and item rewards
    //    }
    //}
}
