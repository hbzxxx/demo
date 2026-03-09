using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExcelTool : EditorWindow
{
    private string excelPath = "";
    private string jsonOutputPath = "Assets/Resources/Data/";
    private string playerSheetName = "Player";
    private string enemySheetName = "Enemy";
    private string itemSheetName = "Item";
    private string questSheetName = "Quest";
    private string npcSheetName = "NPC";
    private string shopSheetName = "Shop";
    private string shopItemSheetName = "ShopItem";
    private string weaponSheetName = "Weapon";

    [MenuItem("Tools/Excel转JSON工具")]
    public static void ShowWindow()
    {
        GetWindow<ExcelTool>("Excel转JSON工具");
    }

    private void OnGUI()
    {
        GUILayout.Label("Excel转JSON工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        excelPath = EditorGUILayout.TextField("Excel文件路径:", excelPath);
        if (GUILayout.Button("选择Excel文件"))
        {
            excelPath = EditorUtility.OpenFilePanel("选择Excel文件", "", "xlsx");
        }

        EditorGUILayout.Space();
        GUILayout.Label("工作表名称:", EditorStyles.boldLabel);
        playerSheetName = EditorGUILayout.TextField("玩家工作表:", playerSheetName);
        enemySheetName = EditorGUILayout.TextField("敌人工作表:", enemySheetName);
        itemSheetName = EditorGUILayout.TextField("物品工作表:", itemSheetName);
        questSheetName = EditorGUILayout.TextField("任务工作表:", questSheetName);
        npcSheetName = EditorGUILayout.TextField("NPC工作表:", npcSheetName);
        shopSheetName = EditorGUILayout.TextField("商店工作表:", shopSheetName);
        shopItemSheetName = EditorGUILayout.TextField("商店物品工作表:", shopItemSheetName);
        weaponSheetName = EditorGUILayout.TextField("武器工作表:", weaponSheetName);

        EditorGUILayout.Space();
        jsonOutputPath = EditorGUILayout.TextField("JSON输出路径:", jsonOutputPath);

        EditorGUILayout.Space();
        if (GUILayout.Button("转换Excel为JSON"))
        {
            ConvertExcelToJson();
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
        GUILayout.Label("使用说明:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("1. 在Unity中安装ExcelDataReader包");
        EditorGUILayout.LabelField("2. 准备Excel文件，每个数据类型一个工作表");
        EditorGUILayout.LabelField("3. 点击转换按钮生成JSON文件");
        
        EditorGUILayout.Space();
        GUILayout.Label("工作表字段说明:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Player:", "ID, Name, MaxHealth, MoveSpeed, AttackDamage, AttackSpeed, Defense");
        EditorGUILayout.LabelField("Enemy:", "ID, Name, MaxHealth, MoveSpeed, ChaseSpeed, DetectionRadius, AttackRange, Damage, AttackCooldown, PatrolWaitTime");
        EditorGUILayout.LabelField("Item:", "ID, Name, Description, Type(0-4), Price, MaxStack, Value");
        EditorGUILayout.LabelField("Quest:", "ID, Title, Description, Type(0-3), TargetID, TargetCount, RewardGold, RewardItems, PreQuestIDs, NPCID");
        EditorGUILayout.LabelField("NPC:", "ID, Name, Title, Description, QuestIDs, ShopID, DialogLines");
        EditorGUILayout.LabelField("Shop:", "ID, Name");
        EditorGUILayout.LabelField("ShopItem:", "ShopID, ItemID, Price, Stock, IsUnlimited");
        EditorGUILayout.LabelField("Weapon:", "ID, Name, FireRate, ReloadTime, ClipSize, MaxReserveAmmo, BulletName, BulletSpeed, BulletTime, Damage, MuzzleEffectsDisappear, BulletPerFire, SpreadAngle");
    }

    private void ConvertExcelToJson()
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            EditorUtility.DisplayDialog("错误", "请选择Excel文件!", "确定");
            return;
        }

        if (!File.Exists(excelPath))
        {
            EditorUtility.DisplayDialog("错误", "Excel文件不存在!", "确定");
            return;
        }

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

            using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet();

                    foreach (System.Data.DataTable table in dataSet.Tables)
                    {
                        string sheetName = table.TableName;
                        List<Dictionary<string, string>> tableData = GetTableData(table);

                        if (MatchSheetName(sheetName, playerSheetName, "Player"))
                        {
                            foreach (var row in tableData)
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

                        if (MatchSheetName(sheetName, enemySheetName, "Enemy"))
                        {
                            foreach (var row in tableData)
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

                        if (MatchSheetName(sheetName, itemSheetName, "Item"))
                        {
                            foreach (var row in tableData)
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

                        if (MatchSheetName(sheetName, questSheetName, "Quest"))
                        {
                            foreach (var row in tableData)
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

                        if (MatchSheetName(sheetName, npcSheetName, "NPC"))
                        {
                            foreach (var row in tableData)
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

                        if (MatchSheetName(sheetName, shopSheetName, "Shop"))
                        {
                            foreach (var row in tableData)
                            {
                                ShopData shop = new ShopData();
                                if (row.ContainsKey("ID")) shop.ID = row["ID"];
                                if (row.ContainsKey("Name")) shop.Name = row["Name"];
                                shops.Add(shop);
                            }
                        }

                        if (MatchSheetName(sheetName, shopItemSheetName, "ShopItem"))
                        {
                            foreach (var row in tableData)
                            {
                                ShopItem shopItem = new ShopItem();
                                if (row.ContainsKey("ShopID")) shopItem.ShopID = row["ShopID"];
                                if (row.ContainsKey("ItemID")) shopItem.ItemID = row["ItemID"];
                                if (row.ContainsKey("Price")) int.TryParse(row["Price"], out shopItem.Price);
                                if (row.ContainsKey("Stock")) int.TryParse(row["Stock"], out shopItem.Stock);
                                if (row.ContainsKey("IsUnlimited")) bool.TryParse(row["IsUnlimited"], out shopItem.IsUnlimited);
                                shopItems.Add(shopItem);
                            }
                        }

                        if (MatchSheetName(sheetName, weaponSheetName, "Weapon"))
                        {
                            foreach (var row in tableData)
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

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("成功", $"数据已导出到:\n{outputFile}", "确定");
            Debug.Log($"Excel数据已转换为JSON: {outputFile}");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("错误", $"转换失败: {e.Message}\n\n请确保已安装ExcelDataReader包!", "确定");
            Debug.LogError($"Excel转换错误: {e}");
        }
    }

    private List<Dictionary<string, string>> GetTableData(System.Data.DataTable table)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        if (table.Rows.Count < 1)
            return result;

        List<string> headers = new List<string>();
        foreach (System.Data.DataColumn column in table.Columns)
        {
            headers.Add(column.ColumnName);
        }

        for (int i = 0; i < table.Rows.Count; i++)
        {
            System.Data.DataRow row = table.Rows[i];
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            for (int j = 0; j < headers.Count; j++)
            {
                rowData[headers[j]] = row[j]?.ToString() ?? "";
            }

            result.Add(rowData);
        }

        return result;
    }

    private bool MatchSheetName(string sheetName, string configName, string defaultName)
    {
        if (!string.IsNullOrEmpty(configName) && sheetName == configName)
            return true;
        if (string.IsNullOrEmpty(configName) && sheetName.Contains(defaultName))
            return true;
        return false;
    }

    private string[] SplitToArray(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new string[0];
        return str.Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
