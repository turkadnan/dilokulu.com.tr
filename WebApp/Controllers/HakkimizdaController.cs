using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class HakkimizdaController : Controller
    {
        //
        // GET: /Hakkimizda/

        #region Degiskenler
        private IStatikSayfaRepository statikSayfaRepository = null;
        #endregion

        public ActionResult Index()
        {
            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("hakkimizda");
            return View(sayfa);
        }

    }
}
