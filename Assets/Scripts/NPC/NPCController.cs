using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string NPCID;
    public NPCData Data;

    private void Start()
    {
        if (NPCManager.Instance != null)
        {
            Data = NPCManager.Instance.GetNPCData(NPCID);
        }
    }

    private void OnMouseDown()
    {
        InteractWithNPC();
    }

    public void InteractWithNPC()
    {
        if (Data == null) return;

        Debug.Log($"与NPC对话: {Data.Name}");

        if (Data.QuestIDs != null && Data.QuestIDs.Length > 0)
        {
            List<QuestData> availableQuests = new List<QuestData>();
            foreach (var questID in Data.QuestIDs)
            {
                QuestData quest = QuestManager.Instance.GetQuest(questID);
                if (quest != null && (quest.State == QuestState.Available || quest.State == QuestState.Completed))
                {
                    availableQuests.Add(quest);
                }
            }

            if (availableQuests.Count > 0)
            {
                EventCenter.Broadcast<string, List<QuestData>>(EventType.NPC_QUEST_DIALOG, Data.Name, availableQuests);
                return;
            }
        }

        EventCenter.Broadcast<string, string>(EventType.NPC_DIALOG, Data.Name, Data.DialogLines);
    }
}
