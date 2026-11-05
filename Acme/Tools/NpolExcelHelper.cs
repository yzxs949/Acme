using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
namespace Acme.Tools;

/// <summary>
/// Excel帮助类
/// </summary>
public class NpolExcelHelper<T> where T : new()
{
    /// <summary>
    /// 合法的文件名后缀
    /// </summary>
    private static readonly List<string> extName = [".xls", ".xlsx", ".xlsm"];

    /// <summary>
    /// 导入Excel内容读取到list集合中
    /// </summary>
    /// <param name="file">导入Execl文件</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    public static List<T> InputExcel(IFormFile file, string sheetName = null)
    {
        if (file is null)
            return null;
        //获取文件后缀名（忽略大小写，兼容 .XLSX 等大写后缀）
        string type = Path.GetExtension(file.FileName).ToLowerInvariant();
        //判断是否导入合法文件
        if (!extName.Contains(type))
            return null;
        //转成为文件流（使用 using 确保释放）
        using MemoryStream ms = new();
        file.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        return InputExcel(ms, sheetName);
    }

    /// <summary>
    /// 根据指定路径读取文件将Excel文件内容读取到list集合中
    /// </summary>
    /// <param name="fileName">文件完整路径名</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    public static List<T> InputExcel(string fileName, string sheetName = null)
    {
        if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            return null;
        //根据指定路径读取文件（使用 using 确保释放）
        using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
        return InputExcel(fs, sheetName);
    }

    /// <summary>
    /// 将Excel文件内容读取到list集合中
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    private static List<T> InputExcel(Stream fileStream, string sheetName = null)
    {
        //创建Excel数据结构（使用 using 确保释放）
        using IWorkbook workbook = WorkbookFactory.Create(fileStream);
        //如果有指定工作表名称，则优先使用，找不到再回退到第一个sheet
        ISheet sheet = string.IsNullOrEmpty(sheetName)
            ? workbook.GetSheetAt(0)
            : workbook.GetSheet(sheetName) ?? workbook.GetSheetAt(0);

        List<T> list = [];
        if (sheet is null)
            return list;

        //获取泛型对象T的所有属性
        PropertyInfo[] properties = typeof(T).GetProperties();
        //每行转换为单个T对象（跳过空行）
        for (int i = 1; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);
            if (row is null)
                continue;
            T obj = new();
            for (int j = 0; j < properties.Length; j++)
            {
                //没有数据的单元格都默认是null
                ICell cell = row.GetCell(j);
                if (cell is null)
                    continue;
                SetPropertyValue(properties[j], obj, cell.ToString());
            }
            list.Add(obj);
        }
        return list;
    }

    /// <summary>
    /// 将字符串安全转换为属性类型并赋值，转换失败时保留默认值
    /// </summary>
    private static void SetPropertyValue(PropertyInfo property, T obj, string value)
    {
        Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        try
        {
            object converted = type switch
            {
                _ when type == typeof(string) => value,
                _ when type == typeof(DateTime) =>
                    DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt) ? dt : default,
                _ when type == typeof(bool) => bool.TryParse(value, out bool b) && b,
                _ when type == typeof(short) => short.TryParse(value, out short s) ? s : default,
                _ when type == typeof(int) => int.TryParse(value, out int n) ? n : default,
                _ when type == typeof(long) => long.TryParse(value, out long l) ? l : default,
                _ when type == typeof(byte) => byte.TryParse(value, out byte by) ? by : default,
                _ => null
            };
            property.SetValue(obj, converted);
        }
        catch
        {
            // 单个单元格转换失败时忽略，避免整表导入中断
        }
    }

    /// <summary>
    /// List导出到Excel
    /// </summary>
    /// <param name="listData">list数据</param>
    /// <param name="templatePath">模板路径</param>
    /// <param name="startRow">从第几行开始填充数据</param>
    /// <param name="sheetName">excel页码名称</param>
    /// <param name="depositPath">保存路径</param>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    public static string ExportExcel(
        List<T> listData,
        string templatePath,
        int startRow = 1,
        string sheetName = "Sheet1",
        string depositPath = "",
        string fileName = "")
    {
        if (listData is null || listData.Count == 0)
            throw new Exception("没有可导出的数据");

        using FileStream file = new(templatePath, FileMode.Open, FileAccess.Read);
        using XSSFWorkbook workbook = new(file);
        ISheet sheet = workbook.GetSheet(sheetName) ?? throw new Exception($"模板中未找到工作表：{sheetName}");

        foreach (T item in listData)
        {
            if (item is null)
                continue;
            PropertyInfo[] properties = item.GetType().GetProperties();
            IRow row = sheet.CreateRow(startRow);
            for (int i = 0; i < properties.Length; i++)
            {
                row.CreateCell(i).SetCellValue(properties[i].GetValue(item)?.ToString() ?? string.Empty);
            }
            startRow++;
        }

        string excelFileName = $"{fileName}{DateTime.Now.ToDataTimeString("", "", "")}.xlsx";
        string excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), depositPath, excelFileName);
        using FileStream newFile = new(excelFilePath, FileMode.Create);
        workbook.Write(newFile);
        return excelFilePath;
    }
}
