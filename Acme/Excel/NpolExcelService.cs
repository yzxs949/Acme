using Acme.Tools;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Acme.Excel;

/// <summary>
/// 基于Npol的Excel操作类
/// </summary>
/// <typeparam name="TEntity">要操作的实体</typeparam>
public class NpolExcelService<TEntity>: INpolExcelService<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// 文件后缀集合
    /// </summary>
    private readonly List<string> _excelSuffixNames;

    /// <summary>
    /// 依赖注入文件后缀集合构造函数
    /// </summary>
    /// <param name="excelSuffixNames"></param>
    public NpolExcelService(List<string> excelSuffixNames)
    {
        _excelSuffixNames = excelSuffixNames;
    }

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public NpolExcelService()
    {
        _excelSuffixNames = new()
        {
            ".xls", ".xlsx", ".xlsm"
        };
    }

    /// <summary>
    /// 导入Excel内容读取到list集合中
    /// </summary>
    /// <param name="file">导入Execl文件</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    public List<TEntity> InputExcel(IFormFile file, string sheetName = null)
    {
        //获取文件后缀名
        string type = Path.GetExtension(file.FileName);
        //判断是否导入合法文件
        if (!_excelSuffixNames.Contains(type)) return null;
        //转成为文件流
        MemoryStream ms = new MemoryStream();
        file.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        //实例化T数组
        List<TEntity> list = new();
        //获取数据
        list = InputExcel(ms, sheetName);
        return list;
    }

    /// <summary>
    /// 根据指定路径读取文件将Excel文件内容读取到list集合中
    /// </summary>
    /// <param name="fileName">文件完整路径名</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    public List<TEntity> InputExcel(string fileName, string sheetName = null)
    {
        if (!File.Exists(fileName))
            return null;
        //根据指定路径读取文件
        using FileStream fs = new(fileName, FileMode.Open, FileAccess.Read);
        //获取数据
        List<TEntity> list = InputExcel(fs, sheetName);
        return list;
    }

    /// <summary>
    /// 将Excel文件内容读取到list集合中
    /// </summary>
    /// <param name="fileStream">文件流</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    private List<TEntity> InputExcel(Stream fileStream, string sheetName = null)
    {
        //创建Excel数据结构
        IWorkbook workbook = WorkbookFactory.Create(fileStream);
        //如果有指定工作表名称
        ISheet sheet;
        if (!string.IsNullOrEmpty(sheetName))
        {
            sheet = workbook.GetSheet(sheetName);
            //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
            sheet ??= workbook.GetSheetAt(0);
        }
        else
        {
            //如果没有指定的sheetName，则尝试获取第一个sheet
            sheet = workbook.GetSheetAt(0);
        }
        //实例化T数组
        List<TEntity> list = new();
        if (sheet != null)
        {
            //一行最后一个cell的编号 即总的列数

            //获取泛型对象T的所有属性
            var propertys = typeof(TEntity).GetProperties();
            //每行转换为单个T对象
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                var obj = new TEntity();
                for (int j = 0; j < propertys.Length; j++)
                {
                    //没有数据的单元格都默认是null
                    ICell cell = row.GetCell(j);
                    if (cell != null)
                    {
                        var value = row.GetCell(j).ToString();
                        string str = (propertys[j].PropertyType).FullName;
                        if (str == "System.String")
                        {
                            propertys[j].SetValue(obj, value, null);
                        }
                        else if (str == "System.DateTime")
                        {
                            DateTime pdt = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                            propertys[j].SetValue(obj, pdt, null);
                        }
                        else if (str == "System.Boolean")
                        {
                            bool pb = Convert.ToBoolean(value);
                            propertys[j].SetValue(obj, pb, null);
                        }
                        else if (str == "System.Int16")
                        {
                            short pi16 = Convert.ToInt16(value);
                            propertys[j].SetValue(obj, pi16, null);
                        }
                        else if (str == "System.Int32")
                        {
                            int pi32 = Convert.ToInt32(value);
                            propertys[j].SetValue(obj, pi32, null);
                        }
                        else if (str == "System.Int64")
                        {
                            long pi64 = Convert.ToInt64(value);
                            propertys[j].SetValue(obj, pi64, null);
                        }
                        else if (str == "System.Byte")
                        {
                            byte pb = Convert.ToByte(value);
                            propertys[j].SetValue(obj, pb, null);
                        }
                        else
                        {
                            propertys[j].SetValue(obj, null, null);
                        }
                    }
                }
                list.Add(obj);
            }
        }
        return list;
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
    public string ExportExcel(
        List<TEntity> listData,
        string templatePath,
        int startRow = 1,
        string sheetName = "Sheet1",
        string depositPath = "",
        string fileName = "")
    {
        if (listData.Count == 0)
            throw new Exception("没有可导出的数据");

        // 路径安全检查
        string safeFileName = SanitizeFileName(fileName);
        string safeDepositPath = SanitizePath(depositPath);

        XSSFWorkbook hssfworkbook;
        using FileStream file = new(templatePath, FileMode.Open, FileAccess.Read);
        hssfworkbook = new(file);
        ISheet sheet = hssfworkbook.GetSheet(sheetName);
        listData.ForEach(item =>
        {
            if (item != null)
            {
                PropertyInfo[] propertyInfo = item.GetType().GetProperties();
                XSSFRow rowdata = (XSSFRow)sheet.CreateRow(startRow);
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    var val = propertyInfo[i].GetValue(item);
                    rowdata.CreateCell(i).SetCellValue(val?.ToString() ?? "");
                }
                startRow++;
            }
        });
        string ExcelFileName = $"{safeFileName}{DateTime.Now.ToDataTimeString("", "", "")}.xlsx";
        string saveDir = Path.Combine(Directory.GetCurrentDirectory(), safeDepositPath);
        Directory.CreateDirectory(saveDir);
        string ExcelFilePath = Path.Combine(saveDir, ExcelFileName);
        using FileStream newfile = new(ExcelFilePath, FileMode.Create);
        hssfworkbook.Write(newfile);
        return ExcelFilePath;
    }

    /// <summary>
    /// 净化文件名，移除路径遍历字符
    /// </summary>
    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrEmpty(name)) return "";
        return name.Replace("\\", "").Replace("/", "").Replace("..", "");
    }

    /// <summary>
    /// 净化路径，移除路径遍历字符
    /// </summary>
    private static string SanitizePath(string path)
    {
        if (string.IsNullOrEmpty(path)) return "";
        var invalid = Path.GetInvalidPathChars();
        var sanitized = new string(path.Where(c => !invalid.Contains(c)).ToArray());
        return sanitized.Replace("..", "").Trim(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }
}
