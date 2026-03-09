using System;

[Serializable]
public class QuestData
{
    public string ID;
    public string Title;
    public string Description;
    public QuestType Type;
    public QuestState State;
    public string TargetID;
    public int TargetCount;
    public int CurrentCount;
    public int RewardGold;
    public string[] RewardItems;
    public string[] PreQuestIDs;
    public string NPCID;
}

public enum QuestType
{
    Kill,
    Collect,
    Talk,
    Reach,
}

public enum QuestState
{
    Locked,
    Available,
    Accepted,
    Completed,
    Finished,
}
