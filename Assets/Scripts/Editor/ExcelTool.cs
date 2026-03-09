using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExcelTool : EditorWindow
{
    private string dataFolderPath = "Assets/Resources/Data/";
    private string jsonOutputPath = "Assets/Resources/Data/";

    [MenuItem("Tools/数据配置工具")]
    public static void ShowWindow()
    {
        GetWindow<ExcelTool>("数据配置工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("数据配置工具 (CSV转JSON)", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        dataFolderPath = EditorGUILayout.TextField("数据文件夹路径:", dataFolderPath);
        if (GUILayout.Button("选择数据文件夹"))
        {
            dataFolderPath = EditorUtility.OpenFolderPanel("选择数据文件夹", "", "");
            if (string.IsNullOrEmpty(dataFolderPath))
            {
                dataFolderPath = "Assets/Resources/Data/";
            }
        }

        EditorGUILayout.Space();
        jsonOutputPath = EditorGUILayout.TextField("JSON输出路径:", jsonOutputPath);

        EditorGUILayout.Space();
        if (GUILayout.Button("转换CSV为JSON"))
        {
            ConvertCSVToJson();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("打开输出目录"))
        {
            if (Directory.Exists(jsonOutputPath))
            {
                EditorUtility.RevealInFinder(jsonOutputPath);
            }
        }

        EditorGUILayout.Space();
        GUILayout.Label("CSV文件模板说明:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Player.csv:", "ID, Name, MaxHealth, MoveSpeed, AttackDamage, AttackSpeed, Defense");
        EditorGUILayout.LabelField("Enemy.csv:", "ID, Name, MaxHealth, MoveSpeed, ChaseSpeed, DetectionRadius, AttackRange, Damage, AttackCooldown, PatrolWaitTime");
        EditorGUILayout.LabelField("Item.csv:", "ID, Name, Description, Type(0-4), Price, MaxStack, Value");
        EditorGUILayout.LabelField("Quest.csv:", "ID, Title, Description, Type(0-3), TargetID, TargetCount, RewardGold, RewardItems(逗号分隔), PreQuestIDs(逗号分隔), NPCID");
        EditorGUILayout.LabelField("NPC.csv:", "ID, Name, Title, Description, QuestIDs(逗号分隔), ShopID, DialogLines");
        EditorGUILayout.LabelField("Shop.csv:", "ID, Name");
        EditorGUILayout.LabelField("ShopItem.csv:", "ShopID, ItemID, Price, Stock, IsUnlimited");
        EditorGUILayout.LabelField("Weapon.csv:", "ID, Name, FireRate, ReloadTime, ClipSize, MaxReserveAmmo, BulletName, BulletSpeed, BulletTime, Damage, MuzzleEffectsDisappear, BulletPerFire, SpreadAngle");
    }

    private void ConvertCSVToJson()
    {
        try
        {
            List<PlayerData> players = new List<PlayerData>();
            List<EnemyData> enemies = new List<EnemyData>();
            List<ItemData> items = new List<ItemData>();
            List<QuestData> quests = new List<QuestData>();
            List<NPCData> npcs = new List<NPCData>();
            List<ShopData> shops = new List<ShopData>();
            List<ShopItem> shopItems = new List<ShopItem>();
            List<WeaponData> weapons = new List<WeaponData>();

            string[] csvFiles = Directory.GetFiles(dataFolderPath, "*.csv");

            foreach (string file in csvFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file).ToLower();

                if (fileName.Contains("player"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        PlayerData player = new PlayerData();
                        if (row.ContainsKey("ID")) player.ID = row["ID"];
                        if (row.ContainsKey("Name")) player.Name = row["Name"];
                        if (row.ContainsKey("MaxHealth")) float.TryParse(row["MaxHealth"], out player.MaxHealth);
                        if (row.ContainsKey("MoveSpeed")) float.TryParse(row["MoveSpeed"], out player.MoveSpeed);
                        if (row.ContainsKey("AttackDamage")) float.TryParse(row["AttackDamage"], out player.AttackDamage);
                        if (row.ContainsKey("AttackSpeed")) float.TryParse(row["AttackSpeed"], out player.AttackSpeed);
                        if (row.ContainsKey("Defense")) float.TryParse(row["Defense"], out player.Defense);
                        players.Add(player);
                    }
                }

                if (fileName.Contains("enemy"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        EnemyData enemy = new EnemyData();
                        if (row.ContainsKey("ID")) enemy.ID = row["ID"];
                        if (row.ContainsKey("Name")) enemy.Name = row["Name"];
                        if (row.ContainsKey("MaxHealth")) float.TryParse(row["MaxHealth"], out enemy.MaxHealth);
                        if (row.ContainsKey("MoveSpeed")) float.TryParse(row["MoveSpeed"], out enemy.MoveSpeed);
                        if (row.ContainsKey("ChaseSpeed")) float.TryParse(row["ChaseSpeed"], out enemy.ChaseSpeed);
                        if (row.ContainsKey("DetectionRadius")) float.TryParse(row["DetectionRadius"], out enemy.DetectionRadius);
                        if (row.ContainsKey("AttackRange")) float.TryParse(row["AttackRange"], out enemy.AttackRange);
                        if (row.ContainsKey("Damage")) int.TryParse(row["Damage"], out enemy.Damage);
                        if (row.ContainsKey("AttackCooldown")) float.TryParse(row["AttackCooldown"], out enemy.AttackCooldown);
                        if (row.ContainsKey("PatrolWaitTime")) float.TryParse(row["PatrolWaitTime"], out enemy.PatrolWaitTime);
                        enemies.Add(enemy);
                    }
                }

                if (fileName.Contains("item"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        ItemData item = new ItemData();
                        if (row.ContainsKey("ID")) item.ID = row["ID"];
                        if (row.ContainsKey("Name")) item.Name = row["Name"];
                        if (row.ContainsKey("Description")) item.Description = row["Description"];
                        if (row.ContainsKey("Type")) int.TryParse(row["Type"], out int type);
                        item.Type = (ItemType)type;
                        if (row.ContainsKey("Price")) int.TryParse(row["Price"], out item.Price);
                        if (row.ContainsKey("MaxStack")) int.TryParse(row["MaxStack"], out item.MaxStack);
                        if (row.ContainsKey("Value")) int.TryParse(row["Value"], out item.Value);
                        items.Add(item);
                    }
                }

                if (fileName.Contains("quest"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        QuestData quest = new QuestData();
                        if (row.ContainsKey("ID")) quest.ID = row["ID"];
                        if (row.ContainsKey("Title")) quest.Title = row["Title"];
                        if (row.ContainsKey("Description")) quest.Description = row["Description"];
                        if (row.ContainsKey("Type")) int.TryParse(row["Type"], out int qtype);
                        quest.Type = (QuestType)qtype;
                        quest.State = QuestState.Available;
                        if (row.ContainsKey("TargetID")) quest.TargetID = row["TargetID"];
                        if (row.ContainsKey("TargetCount")) int.TryParse(row["TargetCount"], out quest.TargetCount);
                        quest.CurrentCount = 0;
                        if (row.ContainsKey("RewardGold")) int.TryParse(row["RewardGold"], out quest.RewardGold);
                        if (row.ContainsKey("RewardItems")) quest.RewardItems = SplitToArray(row["RewardItems"]);
                        if (row.ContainsKey("PreQuestIDs")) quest.PreQuestIDs = SplitToArray(row["PreQuestIDs"]);
                        if (row.ContainsKey("NPCID")) quest.NPCID = row["NPCID"];
                        quests.Add(quest);
                    }
                }

                if (fileName.Contains("npc"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        NPCData npc = new NPCData();
                        if (row.ContainsKey("ID")) npc.ID = row["ID"];
                        if (row.ContainsKey("Name")) npc.Name = row["Name"];
                        if (row.ContainsKey("Title")) npc.Title = row["Title"];
                        if (row.ContainsKey("Description")) npc.Description = row["Description"];
                        if (row.ContainsKey("QuestIDs")) npc.QuestIDs = SplitToArray(row["QuestIDs"]);
                        if (row.ContainsKey("ShopID")) npc.ShopID = row["ShopID"];
                        if (row.ContainsKey("DialogLines")) npc.DialogLines = row["DialogLines"];
                        npcs.Add(npc);
                    }
                }

                if (fileName.Contains("shop") && !fileName.Contains("shopitem"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        ShopData shop = new ShopData();
                        if (row.ContainsKey("ID")) shop.ID = row["ID"];
                        if (row.ContainsKey("Name")) shop.Name = row["Name"];
                        shops.Add(shop);
                    }
                }

                if (fileName.Contains("shopitem"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        ShopItem shopItem = new ShopItem();
                        if (row.ContainsKey("ShopID")) shopItem.ShopID = row["ShopID"];
                        if (row.ContainsKey("ItemID")) shopItem.ItemID = row["ItemID"];
                        if (row.ContainsKey("Price")) int.TryParse(row["Price"], out shopItem.Price);
                        if (row.ContainsKey("Stock")) int.TryParse(row["Stock"], out shopItem.Stock);
                        if (row.ContainsKey("IsUnlimited"))
                        {
                            bool.TryParse(row["IsUnlimited"], out shopItem.IsUnlimited);
                        }
                        shopItems.Add(shopItem);
                    }
                }

                if (fileName.Contains("weapon"))
                {
                    var data = CSVReader.ReadCSV(file);
                    foreach (var row in data)
                    {
                        WeaponData weapon = new WeaponData();
                        if (row.ContainsKey("ID")) weapon.ID = row["ID"];
                        if (row.ContainsKey("Name")) weapon.Name = row["Name"];
                        if (row.ContainsKey("FireRate")) float.TryParse(row["FireRate"], out weapon.FireRate);
                        if (row.ContainsKey("ReloadTime")) float.TryParse(row["ReloadTime"], out weapon.ReloadTime);
                        if (row.ContainsKey("ClipSize")) int.TryParse(row["ClipSize"], out weapon.ClipSize);
                        if (row.ContainsKey("MaxReserveAmmo")) int.TryParse(row["MaxReserveAmmo"], out weapon.MaxReserveAmmo);
                        if (row.ContainsKey("BulletName")) weapon.BulletName = row["BulletName"];
                        if (row.ContainsKey("BulletSpeed")) float.TryParse(row["BulletSpeed"], out weapon.BulletSpeed);
                        if (row.ContainsKey("BulletTime")) float.TryParse(row["BulletTime"], out weapon.BulletTime);
                        if (row.ContainsKey("Damage")) float.TryParse(row["Damage"], out weapon.Damage);
                        if (row.ContainsKey("MuzzleEffectsDisappear")) float.TryParse(row["MuzzleEffectsDisappear"], out weapon.MuzzleEffectsDisappear);
                        if (row.ContainsKey("BulletPerFire")) int.TryParse(row["BulletPerFire"], out weapon.BulletPerFire);
                        if (row.ContainsKey("SpreadAngle")) float.TryParse(row["SpreadAngle"], out weapon.SpreadAngle);
                        weapons.Add(weapon);
                    }
                }
            }

            if (shopItems.Count > 0 && shops.Count > 0)
            {
                foreach (var shop in shops)
                {
                    List<ShopItem> shopItemList = new List<ShopItem>();
                    foreach (var item in shopItems)
                    {
                        if (item.ShopID == shop.ID)
                        {
                            shopItemList.Add(item);
                        }
                    }
                    shop.Items = shopItemList.ToArray();
                }
            }

            GameData dataSheet = new GameData
            {
                Players = players.ToArray(),
                Enemies = enemies.ToArray(),
                Items = items.ToArray(),
                Quests = quests.ToArray(),
                NPCs = npcs.ToArray(),
                Shops = shops.ToArray(),
                Weapons = weapons.ToArray()
            };

            string json = JsonUtility.ToJson(dataSheet, true);

            if (!Directory.Exists(jsonOutputPath))
            {
                Directory.CreateDirectory(jsonOutputPath);
            }

            string outputFile = Path.Combine(jsonOutputPath, "GameData.json");
            File.WriteAllText(outputFile, json);

            EditorUtility.DisplayDialog("成功", $"数据已导出到:\n{outputFile}", "确定");
            Debug.Log($"CSV数据已转换为JSON: {outputFile}");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("错误", $"转换失败: {e.Message}", "确定");
            Debug.LogError($"CSV转换错误: {e}");
        }
    }

    private string[] SplitToArray(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new string[0];
        return str.Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
