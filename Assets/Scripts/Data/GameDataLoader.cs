using System.Collections.Generic;
using UnityEngine;

public class GameDataLoader : MonoBehaviour
{
    public static GameDataLoader Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        LoadAllData();
    }

    public void LoadAllData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/GameData");
        if (jsonFile == null)
        {
            Debug.LogError("无法加载GameData.json");
            return;
        }

        GameData data = JsonUtility.FromJson<GameData>(jsonFile.text);

        if (data.Players != null)
        {
            Debug.Log($"加载了 {data.Players.Length} 个玩家数据");
        }

        if (data.Enemies != null)
        {
            Debug.Log($"加载了 {data.Enemies.Length} 个敌人数据");
        }

        if (data.Items != null)
        {
            InventoryManager.Instance.LoadItemData(data.Items);
        }

        if (data.Quests != null)
        {
            QuestManager.Instance.LoadQuests(data.Quests);
        }

        if (data.NPCs != null)
        {
            NPCManager.Instance.LoadNPCData(data.NPCs);
        }

        if (data.Shops != null)
        {
            ShopManager.Instance.LoadShopData(data.Shops);
        }

        Debug.Log("所有数据加载完成！");
    }
}

[System.Serializable]
public class GameData
{
    public PlayerData[] Players;
    public EnemyData[] Enemies;
    public ItemData[] Items;
    public QuestData[] Quests;
    public NPCData[] NPCs;
    public ShopData[] Shops;
}
