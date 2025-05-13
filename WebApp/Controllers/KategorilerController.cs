using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class KategorilerController : Controller
    {
        //
        // GET: /Kategoriler/
        #region Degiskenler
        private IKategoriRepository kategoriRepository = null;
        #endregion

        public ActionResult Index()
        {
            
            return View();
        }

    }
}
