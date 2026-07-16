using System;
using System.Data;
using System.DrawingCore;
using System.DrawingCore.Drawing2D;
using System.DrawingCore.Imaging;
using System.IO;
using System.Security.Cryptography;

namespace Acme.Tools;

/// <summary>
/// 验证码
/// </summary>
public static class VerCode
{
    /// <summary>
    /// 产生随机0--9的随机数字（使用密码学安全随机数）
    /// </summary>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string GetCode(int len)
    {
        return "0,1,2,3,4,5,6,7,8,9".ToRandomCode(len);
    }

    /// <summary>
    /// 产生随机1--99的数字
    /// </summary>
    /// <param name="len">长度</param>
    /// <returns></returns>
    public static string GetCodeBy100(int len)
    {
        // 使用 StringBuilder 避免循环字符串拼接
        var sb = new System.Text.StringBuilder();
        for (int i = 1; i < 100; i++)
        {
            if (sb.Length > 0)
                sb.Append(',');
            sb.Append(i);
        }
        return sb.ToString().ToRandomCode(len);
    }

    /// <summary>
    /// 获取运算符号
    /// </summary>
    /// <returns></returns>
    public static string GetOperator()
    {
        return "+,-,×,÷".ToRandomCode(1);
    }

    /// <summary>
    /// 创建验证码的图片（方法1：简单风格）
    /// </summary>
    /// <param name="validateCode">验证码字符串</param>
    public static byte[] CreateValidateGraphic1(string validateCode)
    {
        int width = (int)Math.Ceiling(validateCode.Length * 12.0);
        const int height = 22;

        using var bitmap = new Bitmap(width, height);
        using Graphics g = Graphics.FromImage(bitmap);
        // 清空图片背景色
        g.Clear(Color.White);

        // 画图片的干扰线
        for (int i = 0; i < 25; i++)
        {
            int x1 = RandomNumberGenerator.GetInt32(bitmap.Width);
            int x2 = RandomNumberGenerator.GetInt32(bitmap.Width);
            int y1 = RandomNumberGenerator.GetInt32(bitmap.Height);
            int y2 = RandomNumberGenerator.GetInt32(bitmap.Height);
            g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
        }

        using Font font = new("Arial", 12, FontStyle.Bold | FontStyle.Italic);
        using LinearGradientBrush brush = new(
            new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            Color.Blue, Color.DarkRed, 1.2f, true);
        g.DrawString(validateCode, font, brush, 3, 2);

        // 画图片的前景干扰点
        using var noiseBitmap = new Bitmap(width, height);
        for (int i = 0; i < 100; i++)
        {
            int x = RandomNumberGenerator.GetInt32(noiseBitmap.Width);
            int y = RandomNumberGenerator.GetInt32(noiseBitmap.Height);
            noiseBitmap.SetPixel(x, y, Color.FromArgb(RandomNumberGenerator.GetInt32(int.MaxValue) | unchecked((int)0xFF000000)));
        }
        g.DrawImage(noiseBitmap, 0, 0);

        // 画图片的边框线
        g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

        // 保存图片数据
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Jpeg);
        return stream.ToArray();
    }

    /// <summary>
    /// 创建验证码的图片（方法2：多彩风格）
    /// </summary>
    /// <param name="validateCode">验证码字符串</param>
    /// <returns></returns>
    public static byte[] CreateValidateGraphic2(string validateCode)
    {
        Color[] colors = [
            Color.Red, Color.OrangeRed,
            Color.SaddleBrown,
            Color.LimeGreen,
            Color.Green,
            Color.MediumAquamarine,
            Color.Blue,
            Color.MediumOrchid,
            Color.Black,
            Color.DarkBlue,
            Color.Orange,
            Color.Brown,
            Color.DarkCyan,
            Color.Purple];
        string[] fonts = ["Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体"];

        // 创建一个 Bitmap 图片类型对象
        using Bitmap bitmap = new(validateCode.Length * 18, 32);
        // 创建一个图形画笔
        using Graphics graphics = Graphics.FromImage(bitmap);
        // 将图片背景填充成白色
        graphics.Clear(Color.White);

        // 绘制验证码噪点
        int noiseCount = RandomNumberGenerator.GetInt32(60, 80);
        for (int i = 0; i < noiseCount; i++)
        {
            int pointX = RandomNumberGenerator.GetInt32(bitmap.Width);
            int pointY = RandomNumberGenerator.GetInt32(bitmap.Height);
            graphics.DrawLine(new Pen(Color.LightGray, 1), pointX, pointY, pointX + 1, pointY);
        }

        // 绘制随机线条 1 条
        graphics.DrawLine(
                new Pen(colors[RandomNumberGenerator.GetInt32(colors.Length)], RandomNumberGenerator.GetInt32(1, 4)),
                new Point(
                    RandomNumberGenerator.GetInt32(bitmap.Width),
                    RandomNumberGenerator.GetInt32(bitmap.Height)),
                new Point(RandomNumberGenerator.GetInt32(bitmap.Width),
                RandomNumberGenerator.GetInt32(bitmap.Height)));

        // 绘制验证码
        for (int i = 0; i < validateCode.Length; i++)
        {
            using Font font = new(fonts[RandomNumberGenerator.GetInt32(fonts.Length)], 15, FontStyle.Bold);
            graphics.DrawString(
                validateCode.AsSpan(i, 1).ToString(),
                font,
                new SolidBrush(colors[RandomNumberGenerator.GetInt32(colors.Length)]),
                16 * i + 1,
                RandomNumberGenerator.GetInt32(0, 6));
        }

        // 绘制验证码噪点2
        noiseCount = RandomNumberGenerator.GetInt32(30, 50);
        for (int i = 0; i < noiseCount; i++)
        {
            int pointX = RandomNumberGenerator.GetInt32(bitmap.Width);
            int pointY = RandomNumberGenerator.GetInt32(bitmap.Height);
            graphics.DrawLine(new Pen(colors[RandomNumberGenerator.GetInt32(colors.Length)], 1), pointX, pointY, pointX, pointY + 1);
        }

        // 绘制随机线条 2 条
        graphics.DrawLine(
                new Pen(colors[RandomNumberGenerator.GetInt32(colors.Length)], RandomNumberGenerator.GetInt32(1, 4)),
                new Point(
                    RandomNumberGenerator.GetInt32(bitmap.Width),
                    RandomNumberGenerator.GetInt32(bitmap.Height)),
                new Point(RandomNumberGenerator.GetInt32(bitmap.Width),
                RandomNumberGenerator.GetInt32(bitmap.Height)));

        using MemoryStream memoryStream = new();
        bitmap.Save(memoryStream, ImageFormat.Jpeg);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// 由DataTable计算公式（仅限数学运算，防止表达式注入）
    /// </summary>
    /// <param name="expression">表达式</param>
    public static int CalcByDataTable(string expression)
    {
        // 安全检查：只允许数字和运算符
        if (string.IsNullOrWhiteSpace(expression) ||
            !System.Text.RegularExpressions.Regex.IsMatch(expression, @"^[\d+\-*/.]+$"))
        {
            return 0;
        }

        object result = new DataTable().Compute(expression, "");
        _ = int.TryParse(result?.ToString(), out int intResult);
        return intResult;
    }

    /// <summary>
    /// 获取算式验证码
    /// </summary>
    /// <param name="equationString">生成图片的算式</param>
    /// <returns>计算结果</returns>
    public static int GetEquationCode(out string equationString)
    {
        string equation = $"{GetCodeBy100(1)}{GetOperator()}{GetCodeBy100(1)}";
        equationString = $"{equation}=?";
        return CalcByDataTable(equation.Replace("×", "*").Replace("÷", "/"));
    }
}
