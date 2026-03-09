using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;

    private Dictionary<string, NPCData> npcDataDict = new Dictionary<string, NPCData>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNPCData(NPCData[] npcs)
    {
        npcDataDict.Clear();
        foreach (var npc in npcs)
        {
            npcDataDict[npc.ID] = npc;
        }
        Debug.Log($"加载了 {npcDataDict.Count} 个NPC数据");
    }

    public NPCData GetNPCData(string id)
    {
        return npcDataDict.TryGetValue(id, out NPCData data) ? data : null;
    }

    public Dictionary<string, NPCData> GetAllNPCData()
    {
        return npcDataDict;
    }
}
