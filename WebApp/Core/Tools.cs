using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;

namespace WebApp.Core
{
    public static class Tools
    {
        public static Bitmap ResizeImage(Stream streamImage, double maxWidth, double maxHeight)
        {
            try
            {
                Bitmap originalImage = new Bitmap(streamImage);

                if (originalImage.Width < maxWidth)
                {
                    maxWidth = originalImage.Width;
                    maxHeight = originalImage.Height;
                }

                double _maxWidth = maxWidth;
                double _maxHeight = maxHeight;

                double originalImageWidth = Convert.ToDouble(originalImage.Width);
                double originalImageHeight = Convert.ToDouble(originalImage.Height);

                double aspectRatio = originalImageWidth / originalImageHeight;
                double boxRatio = _maxWidth / _maxHeight;
                double scaleFactor = 0;


                if (maxWidth > maxHeight || maxWidth == maxHeight)
                {
                    scaleFactor = _maxWidth / originalImageWidth;
                }
                else
                {
                    scaleFactor = _maxHeight / originalImageHeight;
                }

                //if (boxRatio > aspectRatio) //Use height, since that is the most restrictive dimension of box.
                //    scaleFactor = _maxHeight / originalImageHeight;
                //else
                //    scaleFactor = _maxWidth / originalImageWidth;

                double newWidth = originalImageWidth * scaleFactor;
                double newHeight = originalImageHeight * scaleFactor;

                Bitmap newImage = new Bitmap(originalImage, Convert.ToInt32(Math.Floor(newWidth)), Convert.ToInt32(Math.Floor(newHeight)));

                Graphics g = Graphics.FromImage(newImage);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

                originalImage.Dispose();

                return newImage;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static Bitmap ResizeImage(string path, double maxWidth, double maxHeight)
        {
            Image originalImage = Bitmap.FromFile(path);

            double _maxWidth = maxWidth;
            double _maxHeight = maxHeight;

            double originalImageWidth = Convert.ToDouble(originalImage.Width);
            double originalImageHeight = Convert.ToDouble(originalImage.Height);

            double aspectRatio = originalImageWidth / originalImageHeight;
            double boxRatio = _maxWidth / _maxHeight;
            double scaleFactor = 0;

            if (boxRatio > aspectRatio) //Use height, since that is the most restrictive dimension of box.
                scaleFactor = _maxHeight / originalImageHeight;
            else
                scaleFactor = _maxWidth / originalImageWidth;

            double newWidth = originalImageWidth * scaleFactor;
            double newHeight = originalImageHeight * scaleFactor;

            Bitmap newImage = new Bitmap(originalImage, Convert.ToInt32(Math.Floor(newWidth)), Convert.ToInt32(Math.Floor(newHeight)));

            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

            originalImage.Dispose();

            return newImage;
        }

        public static byte[] Crop(string Img, int Width, int Height, int X, int Y)
        {
            using (Image OriginalImage = Image.FromFile(Img))
            {
                using (Bitmap bmp = new Bitmap(Width, Height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                    using (Graphics Graphic = Graphics.FromImage(bmp))
                    {
                        Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, OriginalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
        }

        public static byte[] Crop(Stream streamImage, int Width, int Height, int X, int Y)
        {
            using (Bitmap OriginalImage = new Bitmap(streamImage))
            {
                using (Bitmap bmp = new Bitmap(Width, Height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                    using (Graphics Graphic = Graphics.FromImage(bmp))
                    {
                        Graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        Graphic.DrawImage(OriginalImage, new Rectangle(0, 0, Width, Height), X, Y, Width, Height, GraphicsUnit.Pixel);
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, OriginalImage.RawFormat);
                        return ms.GetBuffer();
                    }
                }
            }
        }

        public static Bitmap CropByBitmap(Stream streamImage, int Width, int Height, int X, int Y)
        {
            try
            {
                Rectangle cropRect = new Rectangle(X, Y, Width, Height);
                Bitmap src = Image.FromStream(streamImage) as Bitmap;
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }

                return target;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static Bitmap CropByBitmap(string path, int Width, int Height, int X, int Y)
        {
            try
            {
                Rectangle cropRect = new Rectangle(X, Y, Width, Height);
                Bitmap src = Image.FromFile(path) as Bitmap;
                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }

                return target;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public static string AppendTimeStamp(this string fileName)
        {
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }

        ///URL ReWriter
        ///

        public static string ReWriter(string Title)
        {
            string temp = "";
            temp = ReplaceTitle(Title);
            return temp.ToLower();
        }

        public static string ReplaceTitle(string Title)
        {
            string temp = Title.ToLower();
            //temp = temp.Replace("-", "");
            temp = temp.Replace("_", "");
            temp = temp.Replace(".", "-");
            temp = temp.Replace("  ", " ");
            temp = temp.Replace(" / ", "-");
            temp = temp.Replace(" ", "-");
            temp = temp.Replace("ç", "c");
            temp = temp.Replace("ğ", "g");
            temp = temp.Replace("ı", "i");
            temp = temp.Replace("ö", "o");
            temp = temp.Replace("ş", "s");
            temp = temp.Replace("ü", "u");
            temp = temp.Replace("'", "");
            temp = temp.Replace("!", "");
            temp = temp.Replace("?", "");
            temp = temp.Replace(":", "");
            temp = temp.Replace(";", "");
            temp = temp.Replace("~", "");
            temp = temp.Replace(",", "");
            temp = temp.Replace("&", "and");
            temp = temp.Replace("%", "");
            temp = temp.Replace("(", "");
            temp = temp.Replace(")", "");
            temp = temp.Replace("[", "");
            temp = temp.Replace("]", "");
            temp = temp.Replace("=", "");
            temp = temp.Replace("<", "");
            temp = temp.Replace(">", "");
            temp = temp.Replace("^", "");
            temp = temp.Replace("+", "");
            temp = temp.Replace("{", "");
            temp = temp.Replace("}", "");
            temp = temp.Replace("$", "");
            temp = temp.Replace("#", "");
            temp = temp.Replace("/", "-");
            temp = temp.Replace("|", "");
            temp = temp.Replace("\"", "");
            temp = temp.Replace("‘", "");
            temp = temp.Replace("’", "");
            temp = temp.Replace("“", "");
            temp = temp.Replace("”", "");
            temp = temp.Replace("á", "a");
            temp = temp.Replace("ê", "e");
            temp = temp.Replace("--", "");
            temp = temp.Replace("---", "");
            temp = temp.Replace("..", "");
            temp = temp.Replace("...", "");
            temp = temp.Replace("*", "");
            temp = temp.Replace("®", "");
            temp = temp.Replace("--", "-");
            temp = temp.Replace("---", "-");
            temp = temp.Replace("™", "");
            temp = temp.Replace("<sup>&reg;</sup>", "");
            temp = temp.Replace("supandreg-sup", "");

            return temp;
        }

        ///SEO
        ///
        public static void SeoOlustur(HtmlHead PageHeader, string Description, string Keywords)
        {
            HtmlMeta hmDescription = new HtmlMeta();
            HtmlMeta hmKeywords = new HtmlMeta();
            HtmlHead hh = PageHeader;

            hmDescription.Name = "description";
            hmDescription.Content = HtmlTemizle(Description);

            hmKeywords.Name = "keywords";
            hmKeywords.Content = HtmlTemizle(Keywords);

            hh.Controls.Add(hmDescription);
            hh.Controls.Add(hmKeywords);
        }

        public static string HtmlTemizle150Length(string Html)
        {
            string temp = Html;
            if (temp != null)
            {
                temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
                temp = temp.Replace("\r\n", " ");
                temp = temp.Replace("\r", "").Replace("\n", "");
                temp = temp.Replace("&nbsp;&nbsp;", "");
                temp = temp.Replace("&quot;", "");
                temp = temp.Replace("&amp;", "");
                if (temp.Length > 150)
                {
                    temp = temp.Substring(0, 150) + "...";
                }
            }
            else
            {
                temp = string.Empty;
            }

            return temp;
        }

        public static string HtmlTemizle(string Html,int Uzunluk)
        {
            string temp = Html;
            if (temp != null)
            {
                temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
                temp = temp.Replace("\r\n", " ");
                temp = temp.Replace("\r", "").Replace("\n", "");
                temp = temp.Replace("&nbsp;&nbsp;", "");
                temp = temp.Replace("&quot;", "");
                temp = temp.Replace("&amp;", "");
                if (temp.Length > Uzunluk)
                {
                    temp = temp.Substring(0, Uzunluk) + "...";
                }
            }
            else
            {
                temp = string.Empty;
            }

            return temp;
        }

        public static string HtmlTemizle(string Html)
        {
            string temp = Html;
            temp = Regex.Replace(temp, @"<(.|\n)*?>", string.Empty);
            temp = temp.Replace("\r", "").Replace("\n", "");
            temp = temp.Replace("&nbsp;&nbsp;", "");
            temp = temp.Replace("&quot;", "");
            temp = temp.Replace("&amp;", "");
            return temp;
        }

        public enum eSizeBase
        {
            Width,
            Height
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static bool EmailValidation(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
              + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
              + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }

        public static bool IsInteger(string str)
        {
            int outInt;
            return int.TryParse(str, out outInt);
        }

        public static bool IsBool(string str)
        {
            bool outBool;
            return bool.TryParse(str, out outBool);
        }

        public static bool IsDate(string str)
        {
            DateTime outDate;
            return DateTime.TryParse(str, out outDate);
        }

        public static bool IsNumeric(string str)
        {
            double outDouble;
            return double.TryParse(str, out outDouble);
        }

        public static bool IsDecimal(string str)
        {
            decimal outDecimal;
            return decimal.TryParse(str, out outDecimal);
        }

        public static bool IsEmail(string str)
        {
            Regex rg = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return rg.IsMatch(str);
        }

        public static int? ParseNullableInt(this string value)
        {
            if (value == null || value.Trim() == string.Empty)
            {
                return null;
            }
            else
            {
                try
                {
                    return int.Parse(value);
                }
                catch
                {
                    return null;
                }
            }
        }

        public static string GetDataByForm(DateTime date)
        {
            return String.Format("{0:dd.MMMM.yyyy}", date.Date);
        }
    }
}