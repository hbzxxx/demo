using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, QuestData> allQuests = new Dictionary<string, QuestData>();
    private List<QuestData> acceptedQuests = new List<QuestData>();
    private List<QuestData> completedQuests = new List<QuestData>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadQuests(QuestData[] quests)
    {
        allQuests.Clear();
        acceptedQuests.Clear();
        completedQuests.Clear();

        foreach (var quest in quests)
        {
            allQuests[quest.ID] = quest;
            quest.State = QuestState.Available;
            if (quest.PreQuestIDs != null && quest.PreQuestIDs.Length > 0)
            {
                quest.State = QuestState.Locked;
            }
        }
        
        CheckAvailableQuests();
    }

    public void AcceptQuest(string questID)
    {
        if (allQuests.TryGetValue(questID, out QuestData quest))
        {
            if (quest.State == QuestState.Available)
            {
                quest.State = QuestState.Accepted;
                quest.CurrentCount = 0;
                acceptedQuests.Add(quest);
                EventCenter.TriggerEvent<QuestData>(EventType.QUEST_ACCEPTED, quest);
                Debug.Log($"接受任务: {quest.Title}");
            }
        }
    }

    public void UpdateQuestProgress(string targetID, int count = 1)
    {
        foreach (var quest in acceptedQuests)
        {
            if (quest.TargetID == targetID && quest.State == QuestState.Accepted)
            {
                quest.CurrentCount += count;
                EventCenter.TriggerEvent<QuestData>(EventType.QUEST_PROGRESS_UPDATED, quest);

                if (quest.CurrentCount >= quest.TargetCount)
                {
                    quest.State = QuestState.Completed;
                    EventCenter.TriggerEvent<QuestData>(EventType.QUEST_COMPLETED, quest);
                }
            }
        }
    }

    public void CompleteQuest(string questID)
    {
        if (allQuests.TryGetValue(questID, out QuestData quest))
        {
            if (quest.State == QuestState.Completed)
            {
                acceptedQuests.Remove(quest);
                completedQuests.Add(quest);
                quest.State = QuestState.Finished;

                InventoryManager.Instance.AddGold(quest.RewardGold);
                if (quest.RewardItems != null)
                {
                    foreach (var itemID in quest.RewardItems)
                    {
                        InventoryManager.Instance.AddItem(itemID, 1);
                    }
                }

                EventCenter.TriggerEvent<QuestData>(EventType.QUEST_FINISHED, quest);
                CheckAvailableQuests();
                Debug.Log($"完成任务: {quest.Title}, 奖励: {quest.RewardGold}金币");
            }
        }
    }

    private void CheckAvailableQuests()
    {
        foreach (var quest in allQuests.Values)
        {
            if (quest.State == QuestState.Locked)
            {
                bool allPreQuestsFinished = true;
                foreach (var preID in quest.PreQuestIDs)
                {
                    if (allQuests.TryGetValue(preID, out QuestData preQuest))
                    {
                        if (preQuest.State != QuestState.Finished)
                        {
                            allPreQuestsFinished = false;
                            break;
                        }
                    }
                }
                if (allPreQuestsFinished)
                {
                    quest.State = QuestState.Available;
                }
            }
        }
    }

    public List<QuestData> GetAvailableQuests()
    {
        List<QuestData> result = new List<QuestData>();
        foreach (var quest in allQuests.Values)
        {
            if (quest.State == QuestState.Available)
            {
                result.Add(quest);
            }
        }
        return result;
    }

    public List<QuestData> GetAcceptedQuests()
    {
        return acceptedQuests;
    }

    public List<QuestData> GetCompletedQuests()
    {
        return completedQuests;
    }

    public QuestData GetQuest(string questID)
    {
        return allQuests.TryGetValue(questID, out QuestData quest) ? quest : null;
    }
}
