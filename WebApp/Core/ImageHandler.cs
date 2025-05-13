using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace WebApp.Core
{
    public class ImageHandler : IHttpHandler
    {

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string FolderName = "/content/galeri/";
            int thumbWidth = Convert.ToInt32(context.Request.QueryString["thumbWidth"]);
            int thumbHeight = Convert.ToInt32(context.Request.QueryString["thumbHeight"]);
            string strPath = context.Server.MapPath("~/" + FolderName);
            string uploadedFilName = DateTime.Now.Ticks.ToString() + ".jpg";
            string FlashImage = string.Format("{0}{1}", strPath, uploadedFilName);

            Byte[] fileData = context.Request.BinaryRead(context.Request.TotalBytes);

            FileStream oFile;
            oFile = File.Create(FlashImage);
            oFile.Write(fileData, 0, context.Request.TotalBytes);
            oFile.Close();

            CreateThumbImage(FlashImage, thumbWidth, thumbHeight);

            HttpContext.Current.Response.Write(uploadedFilName);
        }

        private void CreateThumbImage(string FlashImage, int w, int h)
        {
            FileStream fs = new FileStream(FlashImage, FileMode.Open, FileAccess.Read);
            Bitmap thumb = Tools.ResizeImage(fs, w, h);
            thumb.Save(FlashImage.Replace(Path.GetFileName(FlashImage), "tn_" + Path.GetFileName(FlashImage)));
            fs.Close();

        }
    }
}