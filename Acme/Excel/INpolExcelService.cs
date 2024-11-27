using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Acme.Excel;

/// <summary>
/// Excel服务接口
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface INpolExcelService<TEntity> where TEntity : class, new()
{
    /// <summary>
    /// 导入Excel内容读取到list集合中
    /// </summary>
    /// <param name="file">导入Execl文件</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    List<TEntity> InputExcel(IFormFile file, string sheetName = null);

    /// <summary>
    /// 根据指定路径读取文件将Excel文件内容读取到list集合中
    /// </summary>
    /// <param name="fileName">文件完整路径名</param>
    /// <param name="sheetName">指定读取excel工作薄sheet的名称</param>
    /// <returns>list集合</returns>
    List<TEntity> InputExcel(string fileName, string sheetName = null);

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
    public string ExportExcel(List<TEntity> listData, string templatePath, int startRow = 1, string sheetName = "Sheet1", string depositPath = "", string fileName = "");
}
