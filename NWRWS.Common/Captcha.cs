using NWRWS.Webs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Common
{
    public class Captcha
    {
        public static string GenerateCaptchaCode(HttpContext http)
        {
        lable: Random random = new Random();

            string randomStr = "";

            Random r = new Random();

            int a = r.Next(10, 99);
            int b = r.Next(10, 99);

            int c = a + b;

            randomStr = a.ToString() + " + " + b.ToString() + " = ";


            StringBuilder captcha = new StringBuilder();
            //for (int i = 0; i < 6; i++)
            {

                if (http.Session.GetString("pastcaptcha") != null)
                {
                    if (http.Session.GetString("pastcaptcha").ToString() == c.ToString())
                    {
                        goto lable;
                    }
                }

                http.Session.SetString("CaptchaCode", c.ToString());
                http.Session.SetString("pastcaptcha", c.ToString());

                //cvCaptcha.ValueToCompare= captcha.ToString();
            }


            return randomStr;
        }

        public static bool ValidateCaptchaCode(string userInputCaptcha, HttpContext context)
        {
            ErrorLogger.Trace("userInputCaptcha=> "+ userInputCaptcha+ " \n\r CaptchaCode=>" + context.Session.GetString("CaptchaCode"), "");
            var isValid = userInputCaptcha == context.Session.GetString("CaptchaCode");
            return isValid;
        }
        public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (Bitmap baseMap = new Bitmap(width, height))
            using (Graphics graph = Graphics.FromImage(baseMap))
            {
                Bitmap bmp = new Bitmap(width, height);

                RectangleF rectf = new RectangleF(10, 5, 0, 0);

                Graphics g = Graphics.FromImage(bmp);

                g.Clear(Color.White);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawString(captchaCode, new Font("Thaoma", 20, FontStyle.Bold), Brushes.Chocolate, rectf);

                g.DrawRectangle(new Pen(Color.Blue), 1, 1, width - 2, height - 2);
                MemoryStream ms = new MemoryStream();

                bmp.Save(ms, ImageFormat.Png);

                ErrorLogger.Trace("Generate CaptchaCode=> " + captchaCode , "");

                return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };
            }
        }

        //public static CaptchaResult GenerateCaptchaImage(int width, int height, string captchaCode)
        //{
        //    using (Bitmap baseMap = new Bitmap(width, height))
        //    using (Graphics graph = Graphics.FromImage(baseMap))
        //    {
        //        Random rand = new Random();

        //        graph.Clear(GetRandomLightColor());

        //        DrawCaptchaCode();
        //        DrawDisorderLine();
        //        AdjustRippleEffect();

        //        MemoryStream ms = new MemoryStream();

        //        baseMap.Save(ms, ImageFormat.Png);

        //        return new CaptchaResult { CaptchaCode = captchaCode, CaptchaByteData = ms.ToArray(), Timestamp = DateTime.Now };

        //        int GetFontSize(int imageWidth, int captchCodeCount)
        //        {
        //            var averageSize = imageWidth / captchCodeCount;

        //            return Convert.ToInt32(averageSize);
        //        }

        //        Color GetRandomDeepColor()
        //        {
        //            int redlow = 160, greenLow = 100, blueLow = 160;
        //            return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
        //        }

        //        Color GetRandomLightColor()
        //        {
        //            int low = 180, high = 255;

        //            int nRend = rand.Next(high) % (high - low) + low;
        //            int nGreen = rand.Next(high) % (high - low) + low;
        //            int nBlue = rand.Next(high) % (high - low) + low;

        //            return Color.FromArgb(nRend, nGreen, nBlue);
        //        }

        //        void DrawCaptchaCode()
        //        {
        //            SolidBrush fontBrush = new SolidBrush(Color.Black);
        //            int fontSize = GetFontSize(width, captchaCode.Length);
        //            Font font = new Font(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
        //            for (int i = 0; i < captchaCode.Length; i++)
        //            {
        //                fontBrush.Color = GetRandomDeepColor();

        //                int shiftPx = fontSize / 6;

        //                float x = i * fontSize + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
        //                int maxY = height - fontSize;
        //                if (maxY < 0) maxY = 0;
        //                float y = rand.Next(0, maxY);

        //                graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
        //            }
        //        }

        //        void DrawDisorderLine()
        //        {
        //            Pen linePen = new Pen(new SolidBrush(Color.Black), 3);
        //            for (int i = 0; i < rand.Next(3, 5); i++)
        //            {
        //                linePen.Color = GetRandomDeepColor();

        //                Point startPoint = new Point(rand.Next(0, width), rand.Next(0, height));
        //                Point endPoint = new Point(rand.Next(0, width), rand.Next(0, height));
        //                graph.DrawLine(linePen, startPoint, endPoint);

        //                //Point bezierPoint1 = new Point(rand.Next(0, width), rand.Next(0, height));
        //                //Point bezierPoint2 = new Point(rand.Next(0, width), rand.Next(0, height));

        //                //graph.DrawBezier(linePen, startPoint, bezierPoint1, bezierPoint2, endPoint);
        //            }
        //        }

        //        void AdjustRippleEffect()
        //        {
        //            short nWave = 6;
        //            int nWidth = baseMap.Width;
        //            int nHeight = baseMap.Height;

        //            Point[,] pt = new Point[nWidth, nHeight];

        //            for (int x = 0; x < nWidth; ++x)
        //            {
        //                for (int y = 0; y < nHeight; ++y)
        //                {
        //                    var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
        //                    var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

        //                    var newX = x + xo;
        //                    var newY = y + yo;

        //                    if (newX > 0 && newX < nWidth)
        //                    {
        //                        pt[x, y].X = (int)newX;
        //                    }
        //                    else
        //                    {
        //                        pt[x, y].X = 0;
        //                    }


        //                    if (newY > 0 && newY < nHeight)
        //                    {
        //                        pt[x, y].Y = (int)newY;
        //                    }
        //                    else
        //                    {
        //                        pt[x, y].Y = 0;
        //                    }
        //                }
        //            }

        //            Bitmap bSrc = (Bitmap)baseMap.Clone();

        //            BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        //            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

        //            int scanline = bitmapData.Stride;

        //            IntPtr scan0 = bitmapData.Scan0;
        //            IntPtr srcScan0 = bmSrc.Scan0;

        //            baseMap.UnlockBits(bitmapData);
        //            bSrc.UnlockBits(bmSrc);
        //            bSrc.Dispose();
        //        }
        //    }
        //}
    }
}
