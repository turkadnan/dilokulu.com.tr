using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class IletisimController : Controller
    {
        //
        // GET: /Iletisim/

        #region Degiskenler
        private IStatikSayfaRepository statikSayfaRepository = null;
        private ISubeRepository subeRepository = null;
        #endregion

        public ActionResult Index()
        {
            subeRepository = new SubeRepository();
            var subeler = subeRepository.Liste().Where(s => s.Durumu == 1).ToList();

            return View(subeler);

        }

    }
}
