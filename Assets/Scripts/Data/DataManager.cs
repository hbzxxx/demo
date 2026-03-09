using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private Dictionary<string, PlayerData> playerDataDict = new Dictionary<string, PlayerData>();
    private Dictionary<string, EnemyData> enemyDataDict = new Dictionary<string, EnemyData>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadDataFromJSON(string jsonPath)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonPath);
        if (jsonFile == null)
        {
            Debug.LogError($"无法加载JSON文件: {jsonPath}");
            return;
        }

        CharacterDataSheet dataSheet = JsonUtility.FromJson<CharacterDataSheet>(jsonFile.text);

        playerDataDict.Clear();
        enemyDataDict.Clear();

        if (dataSheet.Players != null)
        {
            foreach (var player in dataSheet.Players)
            {
                playerDataDict[player.ID] = player;
            }
        }

        if (dataSheet.Enemies != null)
        {
            foreach (var enemy in dataSheet.Enemies)
            {
                enemyDataDict[enemy.ID] = enemy;
            }
        }

        Debug.Log($"数据加载完成: {playerDataDict.Count}个玩家数据, {enemyDataDict.Count}个敌人数据");
    }

    public PlayerData GetPlayerData(string id)
    {
        if (playerDataDict.TryGetValue(id, out PlayerData data))
        {
            return data;
        }
        Debug.LogWarning($"未找到玩家数据: {id}");
        return null;
    }

    public EnemyData GetEnemyData(string id)
    {
        if (enemyDataDict.TryGetValue(id, out EnemyData data))
        {
            return data;
        }
        Debug.LogWarning($"未找到敌人数据: {id}");
        return null;
    }

    public Dictionary<string, PlayerData> GetAllPlayerData()
    {
        return playerDataDict;
    }

    public Dictionary<string, EnemyData> GetAllEnemyData()
    {
        return enemyDataDict;
    }
}
