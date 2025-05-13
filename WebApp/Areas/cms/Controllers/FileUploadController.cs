using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.cms.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /cms/FileUpload/

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ImageUpload(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string strUploadPath = AppDomain.CurrentDomain.BaseDirectory + @"content\uploads\" + "Editor\\\\";

            string url = ConfigurationManager.AppSettings["SiteDomain"] + "Content/Uploads/Editor/";
            string message; // message to display (optional)

            // path of the image
            if (upload != null)
            {
                string NewFileName = DateTime.Now.Ticks.ToString() + System.IO.Path.GetExtension(upload.FileName).ToLower();

                switch (System.IO.Path.GetExtension(upload.FileName).ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".bmp":

                        Bitmap thumb = new Bitmap(upload.InputStream);
                        thumb.Save(strUploadPath + NewFileName);

                        break;
                    default:
                        upload.SaveAs(strUploadPath + NewFileName);
                        break;
                }


                url = url + NewFileName;
            }

            // passing message success/failure
            message = "Dosya yüklendi";

            // since it is an ajax request it requires this string
            string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + message + "\");</script></body></html>";
            return Content(output);
        }

    }
}
