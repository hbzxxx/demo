# Excel依赖问题解决方案

## 问题描述

Unity中使用`using Excel`会导致报错，因为Unity默认不包含Excel相关的程序集。

## 解决方案

### 方案A：使用CSV文件代替Excel（推荐）

直接使用CSV格式的文件，Unity原生支持，无需额外DLL。

ExcelTool已经支持CSV文件读取，只需要：
1. 将Excel文件另存为CSV格式（每个工作表一个CSV文件）
2. 在工具中选择CSV文件进行转换

CSV格式示例：
```csv
ID,Name,MaxHealth,MoveSpeed
P001,Knight,120,5
P002,Archer,80,6
```

### 方案B：导入ExcelDataReader库

1. 打开Unity，在Package Manager中点击"Add package from git URL"
2. 输入：`https://github.com/ExcelDataReader/ExcelDataReader.git`
3. 等待安装完成

或者手动导入：
1. 下载ExcelDataReader：https://github.com/ExcelDataReader/ExcelDataReader/releases
2. 将DLL拖入Unity的Assets目录

### 方案C：修改代码使用CSV读取器

我将修改ExcelReader使其支持CSV文件读取：
<minimax:tool_call>
<invoke name="write">
<parameter name="content">using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader
{
    public static List<Dictionary<string, string>> ReadCSV(string filePath)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"CSV文件不存在: {filePath}");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            Debug.LogError("CSV文件没有足够的数据行");
            return result;
        }

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

            result.Add(rowData);
        }

        return result;
    }

    private static string[] ParseCSVLine(string line)
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

    public static List<Dictionary<string, string>> ReadCSVFromResources(string relativePath)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(relativePath);
        if (csvFile == null)
        {
            Debug.LogError($"无法加载CSV文件: {relativePath}");
            return new List<Dictionary<string, string>>();
        }

        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        string[] lines = csvFile.text.Split(new string[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2) return result;

        string[] headers = ParseCSVLine(lines[0]);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = ParseCSVLine(lines[i]);
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                rowData[headers[j]] = values[j];
            }

            result.Add(rowData);
        }

        return result;
    }
}
