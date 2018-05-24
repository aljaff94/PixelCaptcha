using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelCaptcha
{
    public static class Captcha
    {
        const int width = 190, height = 80;
        static int[] FontSizes = { 25, 30, 35 };
        static string[] FontNames = { "Trebuchet MS", "Arial", /*"Times New Roman",*/ "Georgia", "Verdana", "Geneva" };
        static FontStyle[] fontStyles = { FontStyle.Bold, FontStyle.Italic, FontStyle.Regular, FontStyle.Strikeout, FontStyle.Underline };
        static HatchStyle[] hatchStyles = { HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
                                            HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical, HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
                                            HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                                            HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
                                            HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal, HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
                                            HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal, HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
                                            HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard, HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
                                            HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis, HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
                                            HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag };


        public static string GenerateImage(string value)
        {
            using (var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    var rectangleF = new RectangleF(0, 0, width, height);
                    var random = new Random();
                    var brush = new HatchBrush(hatchStyles[random.Next (hatchStyles.Length - 1)],
                                               Color.FromArgb((random.Next(100, 255)), (random.Next(100, 255)),
                                               (random.Next(100, 255))), Color.White);
                    g.FillRectangle(brush, rectangleF);
                    var theMatrix = new Matrix();
                    for (var i = 0; i <= value.Length - 1; i++)
                    {
                        theMatrix.Reset();
                        var charLength = value.Length;
                        var x = width / (charLength + 1) * i;
                        var y = height / 2;
                        theMatrix.RotateAt(random.Next(-40, 40), new PointF(x, y));
                        g.Transform = theMatrix;
                        g.DrawString
                        (
                            value.Substring(i, 1),
                            new Font(FontNames[random.Next(FontNames.Length - 1)], 25, fontStyles[random.Next(fontStyles.Length - 1)]),
                            new SolidBrush(Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100))),
                            x,  random.Next(10, 40)
                        );
                        g.ResetTransform();
                    }
                    var buffer = new byte[16 * 1024];
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, ImageFormat.Png);
                        int read;
                        while ((read = ms.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        var base64String = Convert.ToBase64String(ms.ToArray());
                        return $"data:image/png;base64,{base64String}";
                    }
                }
            }
        }
        public static string GenerateRandomString(int length)
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
