using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.cms.Controllers
{
    public class UploadController : Controller
    {
        //
        // GET: /cms/Upload/

        public ActionResult ImageUpload()
        {
            return View();
        }

    }
}
