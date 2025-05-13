using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;

namespace WebApp.Areas.cms.Controllers
{
    public class BannersController : Controller
    {
        //
        // GET: /cms/Banners/

        public ActionResult Banner300x350()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Banner300x350(HttpPostedFileBase FileSWF)
        {


            if (FileSWF != null && FileSWF.ContentLength > 0)
            {
                ///assets/Banners/300x350.swf
                string fileName = "300x350.swf";
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.banner300x350) + fileName;

                FileSWF.SaveAs(filePath);
            }
            return View();
        }
    }
}
