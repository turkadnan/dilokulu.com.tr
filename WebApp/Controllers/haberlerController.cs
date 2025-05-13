using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class haberlerController : Controller
    {
        //
        // GET: /haberler/
        #region Degiskenler
        private IHaberRepository haberRepository = null;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detay(string title="")
        {
            if (!string.IsNullOrEmpty(title))
            {
                haberRepository = new HaberRepository();
                var haber = haberRepository.Detay(title, new[] { (int)GeneralVariables.Durum.Aktif });
                return View(haber);
            }
            else
            {
                return View();
            }
        }

    }
}
