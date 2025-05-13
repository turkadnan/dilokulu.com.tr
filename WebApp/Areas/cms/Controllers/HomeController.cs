using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.cms.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /cms/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
