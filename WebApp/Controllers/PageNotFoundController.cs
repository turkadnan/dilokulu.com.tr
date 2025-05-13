using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class PageNotFoundController : Controller
    {
        //
        // GET: /PageNotFound/

        public ActionResult Index(Exception exception)
        {
            return View();
        }

    }
}
