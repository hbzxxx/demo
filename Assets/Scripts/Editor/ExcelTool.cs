using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ExcelTool : EditorWindow
{
    private string excelPath = "";
    private string jsonOutputPath = "Assets/Resources/Data/";
    private string playerSheetName = "Player";
    private string enemySheetName = "Enemy";

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
            excelPath = EditorUtility.OpenFilePanel("选择Excel文件", "", "xlsx;xls");
        }

        EditorGUILayout.Space();
        GUILayout.Label("工作表名称:", EditorStyles.boldLabel);
        playerSheetName = EditorGUILayout.TextField("玩家工作表:", playerSheetName);
        enemySheetName = EditorGUILayout.TextField("敌人工作表:", enemySheetName);

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
                Application.OpenURL("file://" + Path.GetFullPath(jsonOutputPath));
            }
            else
            {
                EditorUtility.RevealInFinder(jsonOutputPath);
            }
        }
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
            DataSet dataSet = ExcelReader.ReadExcel(excelPath);

            List<PlayerData> players = new List<PlayerData>();
            List<EnemyData> enemies = new List<EnemyData>();

            foreach (DataTable table in dataSet.Tables)
            {
                string sheetName = table.TableName;

                if (sheetName == playerSheetName || (string.IsNullOrEmpty(playerSheetName) && sheetName.Contains("Player")))
                {
                    List<Dictionary<string, string>> tableData = ExcelReader.GetTableData(dataSet, sheetName);
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

                if (sheetName == enemySheetName || (string.IsNullOrEmpty(enemySheetName) && sheetName.Contains("Enemy")))
                {
                    List<Dictionary<string, string>> tableData = ExcelReader.GetTableData(dataSet, sheetName);
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
            }

            CharacterDataSheet dataSheet = new CharacterDataSheet
            {
                Players = players.ToArray(),
                Enemies = enemies.ToArray()
            };

            string json = JsonUtility.ToJson(dataSheet, true);

            if (!Directory.Exists(jsonOutputPath))
            {
                Directory.CreateDirectory(jsonOutputPath);
            }

            string outputFile = Path.Combine(jsonOutputPath, "CharacterData.json");
            File.WriteAllText(outputFile, json);

            EditorUtility.DisplayDialog("成功", $"数据已导出到:\n{outputFile}", "确定");
            Debug.Log($"Excel数据已转换为JSON: {outputFile}");
        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayDialog("错误", $"转换失败: {e.Message}", "确定");
            Debug.LogError($"Excel转换错误: {e}");
        }
    }
}
