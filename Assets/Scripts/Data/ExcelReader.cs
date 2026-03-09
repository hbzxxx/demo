using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using UnityEngine;

public class ExcelReader
{
    public static DataSet ReadExcel(string filePath)
    {
        FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        excelReader.Close();
        stream.Close();
        return result;
    }

    public static List<Dictionary<string, string>> GetTableData(DataSet dataSet, string sheetName)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

        if (dataSet == null || dataSet.Tables.Count == 0)
        {
            Debug.LogError("Excel文件为空或没有工作表");
            return result;
        }

        DataTable table = null;
        foreach (DataTable dt in dataSet.Tables)
        {
            if (dt.TableName == sheetName)
            {
                table = dt;
                break;
            }
        }

        if (table == null)
        {
            table = dataSet.Tables[0];
        }

        if (table.Rows.Count < 2)
        {
            Debug.LogError("Excel表没有足够的数据行");
            return result;
        }

        List<string> headers = new List<string>();
        foreach (DataColumn column in table.Columns)
        {
            headers.Add(column.ColumnName);
        }

        for (int i = 1; i < table.Rows.Count; i++)
        {
            DataRow row = table.Rows[i];
            Dictionary<string, string> rowData = new Dictionary<string, string>();

            for (int j = 0; j < headers.Count; j++)
            {
                rowData[headers[j]] = row[j]?.ToString() ?? "";
            }

            result.Add(rowData);
        }

        return result;
    }
}
