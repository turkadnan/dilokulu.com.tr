using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.cms.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /cms/Base/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["Login"] == null)
            {
                if (Session["login"] == null)
                {
                    Response.Redirect("/cms/login/");
                }
            }
        }

    }
}
