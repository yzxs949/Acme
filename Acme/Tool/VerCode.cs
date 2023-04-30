using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Acme
{
    /// <summary>
    /// 验证码
    /// </summary>
    public static class VerCode
    {
        /// <summary>
        /// 产生随机0--9的随机数字
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns></returns>
        public static string GetCode(int len)
        {
            string str = "0,1,2,3,4,5,6,7,8,9";
            string[] strArry = str.Split(',');
            string code = "";
            Random rand = new(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++)
            {
                code += strArry[rand.Next(0, strArry.Length)];
            }
            return code;
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">要输出到的page对象</param>
        public static byte[] CreateValidateGraphic1(string validateCode)
        {
            Bitmap image = new((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            //生成随机生成器
            Random random = new();
            //清空图片背景色
            g.Clear(Color.White);
            //画图片的干扰线
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(image.Width);
                int x2 = random.Next(image.Width);
                int y1 = random.Next(image.Height);
                int y2 = random.Next(image.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            Font font = new("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
            LinearGradientBrush brush = new(new Rectangle(0, 0, image.Width, image.Height),
            Color.Blue, Color.DarkRed, 1.2f, true);
            g.DrawString(validateCode, font, brush, 3, 2);
            //画图片的前景干扰点
            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(image.Width);
                int y = random.Next(image.Height);
                image.SetPixel(x, y, Color.FromArgb(random.Next()));
            }
            //画图片的边框线
            g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
            //保存图片数据
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);

            g.Dispose();
            image.Dispose();
            //输出图片流
            return stream.ToArray();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="validateCode">要输出到的page对象</param>
        /// <returns></returns>
        public static byte[] CreateValidateGraphic2(string validateCode)
        {
            Color[] colors = {
                                 Color.Red, Color.OrangeRed,Color.SaddleBrown,
                                 Color.LimeGreen,Color.Green,Color.MediumAquamarine,
                                 Color.Blue,Color.MediumOrchid,Color.Black,
                                 Color.DarkBlue,Color.Orange,Color.Brown,
                                 Color.DarkCyan,Color.Purple
                             };
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random random = new();
            // 创建一个 Bitmap 图片类型对象
            Bitmap bitmap = new(validateCode.Length * 18, 32);
            // 创建一个图形画笔
            Graphics graphics = Graphics.FromImage(bitmap);
            // 将图片背景填充成白色
            graphics.Clear(Color.White);
            // 绘制验证码噪点
            for (int i = 0; i < random.Next(60, 80); i++)
            {
                int pointX = random.Next(bitmap.Width);
                int pointY = random.Next(bitmap.Height);
                graphics.DrawLine(new Pen(Color.LightGray, 1), pointX, pointY, pointX + 1, pointY);
            }
            // 绘制随机线条 1 条
            graphics.DrawLine(
                    new Pen(colors[random.Next(colors.Length)], random.Next(3)),
                    new Point(
                        random.Next(bitmap.Width),
                        random.Next(bitmap.Height)),
                    new Point(random.Next(bitmap.Width),
                    random.Next(bitmap.Height)));
            // 绘制验证码
            for (int i = 0; i < validateCode.Length; i++)
            {
                graphics.DrawString(
                    validateCode.Substring(i, 1),
                    new Font(fonts[random.Next(fonts.Length)], 15, FontStyle.Bold),
                    new SolidBrush(colors[random.Next(colors.Length)]),
                    16 * i + 1,
                    random.Next(0, 5)
                    );
            }
            // 绘制验证码噪点
            for (int i = 0; i < random.Next(30, 50); i++)
            {
                int pointX = random.Next(bitmap.Width);
                int pointY = random.Next(bitmap.Height);
                graphics.DrawLine(new Pen(colors[random.Next(colors.Length)], 1), pointX, pointY, pointX, pointY + 1);
            }
            // 绘制随机线条 1 条
            graphics.DrawLine(
                    new Pen(colors[random.Next(colors.Length)], random.Next(3)),
                    new Point(
                        random.Next(bitmap.Width),
                        random.Next(bitmap.Height)),
                    new Point(random.Next(bitmap.Width),
                    random.Next(bitmap.Height)));

            using MemoryStream memoryStream = new();
            bitmap.Save(memoryStream, ImageFormat.Jpeg);
            byte[] bytes = memoryStream.ToArray();
            return bytes;
        }
    }
}
