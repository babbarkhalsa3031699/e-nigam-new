using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CMNirdesh.Controllers
{
    public class CaptchaRandomImage
    {

        private Bitmap image;

        private Random random = new Random();

        public Bitmap Image => image;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(disposing: true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                image.Dispose();
            }
        }

        public void GenerateImage(string text, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            HatchBrush brush = new HatchBrush(HatchStyle.Cross, Color.LightGray, Color.White);
            graphics.FillRectangle(brush, rectangle);
            float num = rectangle.Height + 1;
            Font font;
            do
            {
                num -= 1f;
                font = new Font(FontFamily.GenericSansSerif, num, FontStyle.Bold);
            }
            while (graphics.MeasureString(text, font).Width > (float)rectangle.Width);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(text, font.FontFamily, (int)font.Style, num, rectangle, stringFormat);
            float num2 = 4f;
            PointF[] destPoints = new PointF[4]
            {
                new PointF((float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            graphicsPath.Warp(destPoints, rectangle, matrix, WarpMode.Perspective, 0f);
            brush = new HatchBrush(HatchStyle.Percent10, Color.DarkGray, Color.Gray);
            graphics.FillPath(brush, graphicsPath);
            int num3 = Math.Max(rectangle.Width, rectangle.Height);
            for (int i = 0; i < (int)((float)(rectangle.Width * rectangle.Height) / 30f); i++)
            {
                int x = random.Next(rectangle.Width);
                int y = random.Next(rectangle.Height);
                int width2 = random.Next(num3 / 60);
                int height2 = random.Next(num3 / 60);
                graphics.FillEllipse(brush, x, y, width2, height2);
            }

            font.Dispose();
            brush.Dispose();
            graphics.Dispose();
            image = bitmap;
        }

        public void GenerateImage(string text, int width, int height, Color textColor)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            HatchBrush brush = new HatchBrush(HatchStyle.Cross, Color.LightGray, Color.White);
            graphics.FillRectangle(brush, rectangle);
            float num = rectangle.Height + 1;
            Font font;
            do
            {
                num -= 1f;
                font = new Font(FontFamily.GenericSansSerif, num, FontStyle.Bold);
            }
            while (graphics.MeasureString(text, font).Width > (float)rectangle.Width);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(text, font.FontFamily, (int)font.Style, num, rectangle, stringFormat);
            float num2 = 4f;
            PointF[] destPoints = new PointF[4]
            {
                new PointF((float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            graphicsPath.Warp(destPoints, rectangle, matrix, WarpMode.Perspective, 0f);
            brush = new HatchBrush(HatchStyle.Percent10, Color.Gray, textColor);
            graphics.FillPath(brush, graphicsPath);
            int num3 = Math.Max(rectangle.Width, rectangle.Height);
            for (int i = 0; i < (int)((float)(rectangle.Width * rectangle.Height) / 30f); i++)
            {
                int x = random.Next(rectangle.Width);
                int y = random.Next(rectangle.Height);
                int width2 = random.Next(num3 / 60);
                int height2 = random.Next(num3 / 60);
                graphics.FillEllipse(brush, x, y, width2, height2);
            }

            font.Dispose();
            brush.Dispose();
            graphics.Dispose();
            image = bitmap;
        }

        public void GenerateImage(string text, int width, int height, Color textColor, Color backColor)
        {
            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectangle = new Rectangle(0, 0, width, height);
            HatchBrush brush = new HatchBrush(HatchStyle.Cross, Color.LightGray, backColor);
            graphics.FillRectangle(brush, rectangle);
            float num = rectangle.Height + 1;
            Font font;
            do
            {
                num -= 1f;
                font = new Font(FontFamily.GenericSansSerif, num, FontStyle.Bold);
            }
            while (graphics.MeasureString(text, font).Width > (float)rectangle.Width);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddString(text, font.FontFamily, (int)font.Style, num, rectangle, stringFormat);
            float num2 = 4f;
            PointF[] destPoints = new PointF[4]
            {
                new PointF((float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)random.Next(rectangle.Height) / num2),
                new PointF((float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2),
                new PointF((float)rectangle.Width - (float)random.Next(rectangle.Width) / num2, (float)rectangle.Height - (float)random.Next(rectangle.Height) / num2)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0f, 0f);
            graphicsPath.Warp(destPoints, rectangle, matrix, WarpMode.Perspective, 0f);
            brush = new HatchBrush(HatchStyle.Percent10, Color.DarkGray, textColor);
            graphics.FillPath(brush, graphicsPath);
            int num3 = Math.Max(rectangle.Width, rectangle.Height);
            for (int i = 0; i < (int)((float)(rectangle.Width * rectangle.Height) / 30f); i++)
            {
                int x = random.Next(rectangle.Width);
                int y = random.Next(rectangle.Height);
                int width2 = random.Next(num3 / 60);
                int height2 = random.Next(num3 / 60);
                graphics.FillEllipse(brush, x, y, width2, height2);
            }

            font.Dispose();
            brush.Dispose();
            graphics.Dispose();
            image = bitmap;
        }

        public string GetRandomString(int size)
        {
            Random random = new Random();
            string text = "";
            for (int i = 0; i < size; i++)
            {
                switch (random.Next(3))
                {
                    case 1:
                        text += random.Next(0, 9);
                        break;
                    case 2:
                        {
                            int value = random.Next(65, 90);
                            text += Convert.ToChar(value);
                            break;
                        }
                    case 3:
                        {
                            int value = random.Next(97, 122);
                            text += Convert.ToChar(value);
                            break;
                        }
                    default:
                        {
                            int value = random.Next(97, 122);
                            text += Convert.ToChar(value);
                            break;
                        }
                }

                random.NextDouble();
                random.Next(100, 1999);
            }

            return text;
        }
    }
}