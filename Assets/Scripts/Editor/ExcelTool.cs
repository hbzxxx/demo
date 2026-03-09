using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class ExcelTool : EditorWindow
{
    private string excelPath = "";
    private string jsonOutputPath = "Assets/Resources/Data/";
    private string templateOutputPath = "Assets/Resources/Data/Templates/";
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
        GUILayout.Label("Excel数据配置工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 生成模板部分
        GUILayout.Label("=== 生成模板文件 ===", EditorStyles.boldLabel);
        templateOutputPath = EditorGUILayout.TextField("模板输出路径:", templateOutputPath);
        
        if (GUILayout.Button("生成CSV模板文件"))
        {
            GenerateCSVTemplates();
        }
        
        if (GUILayout.Button("生成示例数据"))
        {
            GenerateSampleData();
        }

        EditorGUILayout.Space();
        GUILayout.Label("=== 转换Excel为JSON ===", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        excelPath = EditorGUILayout.TextField("Excel文件路径:", excelPath);
        if (GUILayout.Button("选择Excel文件"))
        {
            excelPath = EditorUtility.OpenFilePanel("选择Excel文件", "", "xlsx;csv");
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
        if (GUILayout.Button("转换Excel/CSV为JSON"))
        {
            ConvertToJson();
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
        EditorGUILayout.LabelField("1. 点击「生成CSV模板文件」获取空白模板");
        EditorGUILayout.LabelField("2. 在Excel/WPS中打开CSV文件并填写数据");
        EditorGUILayout.LabelField("3. 保存后使用本工具转换为JSON");
    }

    /// <summary>
    /// 生成CSV模板文件
    /// </summary>
    private void GenerateCSVTemplates()
    {
        if (!Directory.Exists(templateOutputPath))
        {
            Directory.CreateDirectory(templateOutputPath);
        }

        // Player模板
        string playerContent = "ID,Name,MaxHealth,MoveSpeed,AttackDamage,AttackSpeed,Defense\nP001,Knight,120,5,25,1.2,10";
        File.WriteAllText(Path.Combine(templateOutputPath, "Player模板.csv"), playerContent, Encoding.UTF8);

        // Enemy模板
        string enemyContent = "ID,Name,MaxHealth,MoveSpeed,ChaseSpeed,DetectionRadius,AttackRange,Damage,AttackCooldown,PatrolWaitTime\nE001,Slime,30,2,3,4,1,5,2,2";
        File.WriteAllText(Path.Combine(templateOutputPath, "Enemy模板.csv"), enemyContent, Encoding.UTF8);

        // Item模板
        string itemContent = "ID,Name,Description,Type,Price,MaxStack,Value\nI001,生命药水,恢复50点生命值,2,50,99,50";
        File.WriteAllText(Path.Combine(templateOutputPath, "Item模板.csv"), itemContent, Encoding.UTF8);

        // Quest模板
        string questContent = "ID,Title,Description,Type,TargetID,TargetCount,RewardGold,RewardItems,PreQuestIDs,NPCID\nQ001,讨伐史莱姆,击败5只史莱姆,0,E001,5,100,I002,,N001";
        File.WriteAllText(Path.Combine(templateOutputPath, "Quest模板.csv"), questContent, Encoding.UTF8);

        // NPC模板
        string npcContent = "ID,Name,Title,Description,QuestIDs,ShopID,DialogLines\nN001,村长,村长,村庄的村长,Q001;Q002,,欢迎来到小村庄!";
        File.WriteAllText(Path.Combine(templateOutputPath, "NPC模板.csv"), npcContent, Encoding.UTF8);

        // Shop模板
        string shopContent = "ID,Name\nS001,杂货店";
        File.WriteAllText(Path.Combine(templateOutputPath, "Shop模板.csv"), shopContent, Encoding.UTF8);

        // ShopItem模板
        string shopItemContent = "ShopID,ItemID,Price,Stock,IsUnlimited\nS001,I001,50,99,True";
        File.WriteAllText(Path.Combine(templateOutputPath, "ShopItem模板.csv"), shopItemContent, Encoding.UTF8);

        // Weapon模板
        string weaponContent = "ID,Name,FireRate,ReloadTime,ClipSize,MaxReserveAmmo,BulletName,BulletSpeed,BulletTime,Damage,MuzzleEffectsDisappear,BulletPerFire,SpreadAngle\nW001,手枪,0.2,1.5,12,60,Bullet,20,2,10,0.1,1,5";
        File.WriteAllText(Path.Combine(templateOutputPath, "Weapon模板.csv"), weaponContent, Encoding.UTF8);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", $"模板文件已生成到:\n{templateOutputPath}", "确定");
        Debug.Log($"CSV模板文件已生成: {templateOutputPath}");
    }

    /// <summary>
    /// 生成示例数据
    /// </summary>
    private void GenerateSampleData()
    {
        if (!Directory.Exists(jsonOutputPath))
        {
            Directory.CreateDirectory(jsonOutputPath);
        }

        // 生成完整的示例数据JSON
        GameData data = new GameData
        {
            Players = new PlayerData[]
            {
                new PlayerData { ID = "P001", Name = "骑士", MaxHealth = 120, MoveSpeed = 5, AttackDamage = 25, AttackSpeed = 1.2f, Defense = 10 },
                new PlayerData { ID = "P002", Name = "弓箭手", MaxHealth = 80, MoveSpeed = 6, AttackDamage = 20, AttackSpeed = 2.0f, Defense = 5 },
                new PlayerData { ID = "P003", Name = "法师", MaxHealth = 60, MoveSpeed = 4, AttackDamage = 35, AttackSpeed = 0.8f, Defense = 3 },
            },
            Enemies = new EnemyData[]
            {
                new EnemyData { ID = "E001", Name = "史莱姆", MaxHealth = 30, MoveSpeed = 2, ChaseSpeed = 3, DetectionRadius = 4, AttackRange = 1, Damage = 5, AttackCooldown = 2, PatrolWaitTime = 2 },
                new EnemyData { ID = "E002", Name = "哥布林", MaxHealth = 50, MoveSpeed = 3, ChaseSpeed = 4, DetectionRadius = 5, AttackRange = 1.5f, Damage = 10, AttackCooldown = 1.5f, PatrolWaitTime = 1.5f },
                new EnemyData { ID = "E003", Name = "兽人", MaxHealth = 100, MoveSpeed = 2, ChaseSpeed = 3.5f, DetectionRadius = 6, AttackRange = 2, Damage = 20, AttackCooldown = 3, PatrolWaitTime = 2 },
            },
            Items = new ItemData[]
            {
                new ItemData { ID = "I001", Name = "生命药水", Description = "恢复50点生命值", Type = ItemType.Consumable, Price = 50, MaxStack = 99, Value = 50 },
                new ItemData { ID = "I002", Name = "铁剑", Description = "基础武器，伤害+10", Type = ItemType.Weapon, Price = 100, MaxStack = 1, Value = 10 },
                new ItemData { ID = "I003", Name = "皮甲", Description = "基础防具，防御+5", Type = ItemType.Armor, Price = 80, MaxStack = 1, Value = 5 },
                new ItemData { ID = "I004", Name = "史莱姆凝胶", Description = "任务材料", Type = ItemType.Material, Price = 5, MaxStack = 99, Value = 0 },
                new ItemData { ID = "I005", Name = "金币袋", Description = "包含100金币", Type = ItemType.Consumable, Price = 0, MaxStack = 99, Value = 100 },
                new ItemData { ID = "I006", Name = "魔法杖", Description = "法师武器，伤害+15", Type = ItemType.Weapon, Price = 150, MaxStack = 1, Value = 15 },
            },
            Quests = new QuestData[]
            {
                new QuestData { ID = "Q001", Title = "讨伐史莱姆", Description = "击败5只史莱姆", Type = QuestType.Kill, TargetID = "E001", TargetCount = 5, RewardGold = 100, RewardItems = new string[] { "I002" }, PreQuestIDs = new string[] { }, NPCID = "N001", State = QuestState.Available },
                new QuestData { ID = "Q002", Title = "收集材料", Description = "收集10个史莱姆凝胶", Type = QuestType.Collect, TargetID = "I004", TargetCount = 10, RewardGold = 50, RewardItems = new string[] { }, PreQuestIDs = new string[] { "Q001" }, NPCID = "N001", State = QuestState.Available },
            },
            NPCs = new NPCData[]
            {
                new NPCData { ID = "N001", Name = "村长", Title = "村长", Description = "村庄的村长，有重要任务委托给冒险者", QuestIDs = new string[] { "Q001", "Q002" }, ShopID = "", DialogLines = "欢迎来到小村庄，冒险者！最近村庄附近出现了很多史莱姆..." },
                new NPCData { ID = "N002", Name = "商人", Title = "杂货商", Description = "出售各种道具", QuestIDs = new string[] { }, ShopID = "S001", DialogLines = "看看我的商品吧！" },
            },
            Shops = new ShopData[]
            {
                new ShopData { ID = "S001", Name = "杂货店", Items = new ShopItem[] 
                { 
                    new ShopItem { ShopID = "S001", ItemID = "I001", Price = 50, Stock = 99, IsUnlimited = true },
                    new ShopItem { ShopID = "S001", ItemID = "I002", Price = 100, Stock = 5, IsUnlimited = false },
                    new ShopItem { ShopID = "S001", ItemID = "I003", Price = 80, Stock = 3, IsUnlimited = false },
                }},
            },
            Weapons = new WeaponData[]
            {
                new WeaponData { ID = "W001", Name = "手枪", FireRate = 0.2f, ReloadTime = 1.5f, ClipSize = 12, MaxReserveAmmo = 60, BulletName = "Bullet", BulletSpeed = 20, BulletTime = 2, Damage = 10, MuzzleEffectsDisappear = 0.1f, BulletPerFire = 1, SpreadAngle = 5 },
                new WeaponData { ID = "W002", Name = "步枪", FireRate = 0.1f, ReloadTime = 2, ClipSize = 30, MaxReserveAmmo = 120, BulletName = "Bullet", BulletSpeed = 30, BulletTime = 3, Damage = 15, MuzzleEffectsDisappear = 0.1f, BulletPerFire = 1, SpreadAngle = 3 },
                new WeaponData { ID = "W003", Name = "霰弹枪", FireRate = 1, ReloadTime = 3, ClipSize = 6, MaxReserveAmmo = 24, BulletName = "ShotgunBullet", BulletSpeed = 15, BulletTime = 1, Damage = 25, MuzzleEffectsDisappear = 0.15f, BulletPerFire = 5, SpreadAngle = 15 },
            }
        };

        string json = JsonUtility.ToJson(data, true);
        string outputFile = Path.Combine(jsonOutputPath, "GameData.json");
        File.WriteAllText(outputFile, json);

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", $"示例数据已生成到:\n{outputFile}", "确定");
        Debug.Log($"示例数据已生成: {outputFile}");
    }

    /// <summary>
    /// 转换Excel或CSV为JSON
    /// </summary>
    private void ConvertToJson()
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            EditorUtility.DisplayDialog("错误", "请选择Excel或CSV文件!", "确定");
            return;
        }

        if (!File.Exists(excelPath))
        {
            EditorUtility.DisplayDialog("错误", "文件不存在!", "确定");
            return;
        }

        try
        {
            string extension = Path.GetExtension(excelPath).ToLower();
            Dictionary<string, List<Dictionary<string, string>>> allSheets;

            if (extension == ".csv")
            {
                allSheets = ReadCSVFile(excelPath);
            }
            else if (extension == ".xlsx")
            {
                allSheets = ReadExcelFile(excelPath);
            }
            else
            {
                EditorUtility.DisplayDialog("错误", "仅支持.xlsx和.csv格式!", "确定");
                return;
            }

            List<PlayerData> players = new List<PlayerData>();
            List<EnemyData> enemies = new List<EnemyData>();
            List<ItemData> items = new List<ItemData>();
            List<QuestData> quests = new List<QuestData>();
            List<NPCData> npcs = new List<NPCData>();
            List<ShopData> shops = new List<ShopData>();
            List<ShopItem> shopItems = new List<ShopItem>();
            List<WeaponData> weapons = new List<WeaponData>();

            foreach (var sheet in allSheets)
            {
                string sheetName = sheet.Key;
                List<Dictionary<string, string>> rows = sheet.Value;

                if (MatchSheetName(sheetName, playerSheetName, "Player"))
                {
                    foreach (var row in rows)
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
                    foreach (var row in rows)
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
                    foreach (var row in rows)
                    {
                        ItemData item = new ItemData();
                        if (row.ContainsKey("ID")) item.ID = row["ID"];
                        if (row.ContainsKey("Name")) item.Name = row["Name"];
                        if (row.ContainsKey("Description")) item.Description = row["Description"];
                        int type = 0;
                        if (row.ContainsKey("Type")) int.TryParse(row["Type"], out type);
                        item.Type = (ItemType)type;
                        if (row.ContainsKey("Price")) int.TryParse(row["Price"], out item.Price);
                        if (row.ContainsKey("MaxStack")) int.TryParse(row["MaxStack"], out item.MaxStack);
                        if (row.ContainsKey("Value")) int.TryParse(row["Value"], out item.Value);
                        items.Add(item);
                    }
                }

                if (MatchSheetName(sheetName, questSheetName, "Quest"))
                {
                    foreach (var row in rows)
                    {
                        QuestData quest = new QuestData();
                        if (row.ContainsKey("ID")) quest.ID = row["ID"];
                        if (row.ContainsKey("Title")) quest.Title = row["Title"];
                        if (row.ContainsKey("Description")) quest.Description = row["Description"];
                        int qtype = 0;
                        if (row.ContainsKey("Type")) int.TryParse(row["Type"], out qtype);
                        if("Type")) int.TryParse(row["Type"], out qtype);
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
                    foreach (var row in rows)
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
                    foreach (var row in rows)
                    {
                        ShopData shop = new ShopData();
                        if (row.ContainsKey("ID")) shop.ID = row["ID"];
                        if (row.ContainsKey("Name")) shop.Name = row["Name"];
                        shops.Add(shop);
                    }
                }

                if (MatchSheetName(sheetName, shopItemSheetName, "ShopItem"))
                {
                    foreach (var row in rows)
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
                    foreach (var row in rows)
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

            AssetDatabase.Refresh();

            EditorUtility.DisplayDialog("成功", $"数据已导出到:\n{outputFile}", "确定");
            Debug.Log($"数据已转换为JSON: {outputFile}");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("错误", $"转换失败: {e.Message}", "确定");
            Debug.LogError($"转换错误: {e}");
        }
    }

    /// <summary>
    /// 读取CSV文件
    /// </summary>
    private Dictionary<string, List<Dictionary<string, string>>> ReadCSVFile(string filePath)
    {
        Dictionary<string, List<Dictionary<string, string>>> result = new Dictionary<string, List<Dictionary<string, string>>>();

        string fileName = Path.GetFileNameWithoutExtension(filePath);
        List<Dictionary<string, string>> rows = ParseCSV(filePath);

        if (rows.Count > 0)
        {
            result[fileName] = rows;
        }

        return result;
    }

    /// <summary>
    /// 解析CSV文件
    /// </summary>
    private List<Dictionary<string, string>> ParseCSV(string filePath)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        if (lines.Length < 1) return result;

        string[] headers = ParseCSVLine(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] values = ParseCSVLine(lines[i]);
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                rowData[headers[j]] = values[j];
            }

            if (rowData.Count > 0)
            {
                result.Add(rowData);
            }
        }

        return result;
    }

    /// <summary>
    /// 解析CSV单行
    /// </summary>
    private string[] ParseCSVLine(string line)
    {
        List<string> result = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(current.Trim());
                current = "";
            }
            else
            {
                current += c;
            }
        }

        result.Add(current.Trim());
        return result.ToArray();
    }

    /// <summary>
    /// 读取Excel文件（xlsx格式）
    /// </summary>
    private Dictionary<string, List<Dictionary<string, string>>> ReadExcelFile(string filePath)
    {
        Dictionary<string, List<Dictionary<string, string>>> result = new Dictionary<string, List<Dictionary<string, string>>>();

        using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Read))
        {
            foreach (var entry in archive.Entries)
            {
                if (entry.FullName.StartsWith("xl/worksheets/sheet") && entry.FullName.EndsWith(".xml"))
                {
                    string sheetName = GetSheetName(entry.FullName, archive);
                    if (!string.IsNullOrEmpty(sheetName))
                    {
                        List<Dictionary<string, string>> rows = ParseSheet(entry.Open());
                        if (rows.Count > 0)
                        {
                            result[sheetName] = rows;
                        }
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 获取工作表名称
    /// </summary>
    private string GetSheetName(string sheetPath, ZipArchive archive)
    {
        string num = Path.GetFileNameWithoutExtension(sheetPath).Replace("sheet", "");
        var workBook = archive.GetEntry("xl/workbook.xml");
        if (workBook == null) return "Sheet" + num;

        XmlDocument doc = new XmlDocument();
        doc.Load(workBook.Open());
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("main", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");

        XmlNodeList sheets = doc.SelectNodes("//main:sheets/main:sheet", nsmgr);
        int index = int.Parse(num) - 1;
        if (sheets != null && index < sheets.Count)
        {
            XmlNode sheet = sheets[index];
            if (sheet.Attributes != null)
            {
                return sheet.Attributes["name"].Value;
            }
        }

        return "Sheet" + num;
    }

    /// <summary>
    /// 解析工作表
    /// </summary>
    private List<Dictionary<string, string>> ParseSheet(Stream stream)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        XmlDocument doc = new XmlDocument();
        doc.Load(stream);
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("main", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
        nsmgr.AddNamespace("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");

        XmlNodeList rows = doc.SelectNodes("//main:row", nsmgr);
        if (rows == null || rows.Count < 2) return result;

        List<string> headers = new List<string>();
        XmlNode firstRow = rows[0];
        foreach (XmlNode cell in firstRow.ChildNodes)
        {
            string value = GetCellValue(cell, doc, nsmgr);
            headers.Add(value);
        }

        for (int i = 1; i < rows.Count; i++)
        {
            XmlNode row = rows[i];
            Dictionary<string, string> rowData = new Dictionary<string, string>();
            int colIndex = 0;

            foreach (XmlNode cell in row.ChildNodes)
            {
                string value = GetCellValue(cell, doc, nsmgr);
                if (colIndex < headers.Count)
                {
                    rowData[headers[colIndex]] = value;
                }
                colIndex++;
            }

            if (rowData.Count > 0)
            {
                result.Add(rowData);
            }
        }

        return result;
    }

    /// <summary>
    /// 获取单元格值
    /// </summary>
    private string GetCellValue(XmlNode cell, XmlDocument doc, XmlNamespaceManager nsmgr)
    {
        XmlNode valueNode = cell.SelectSingleNode("main:v", nsmgr);
        if (valueNode != null)
        {
            return valueNode.InnerText;
        }

        XmlNode inlineStrNode = cell.SelectSingleNode("main:is/main:t", nsmgr);
        if (inlineStrNode != null)
        {
            return inlineStrNode.InnerText;
        }

        return "";
    }

    /// <summary>
    /// 匹配工作表名称
    /// </summary>
    private bool MatchSheetName(string sheetName, string configName, string defaultName)
    {
        if (!string.IsNullOrEmpty(configName) && sheetName == configName)
            return true;
        if (string.IsNullOrEmpty(configName) && sheetName.Contains(defaultName))
            return true;
        return false;
    }

    /// <summary>
    /// 分割字符串为数组
    /// </summary>
    private string[] SplitToArray(string str)
    {
        if (string.IsNullOrEmpty(str))
            return new string[0];
        return str.Split(new char[] { ',', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
    }
}
