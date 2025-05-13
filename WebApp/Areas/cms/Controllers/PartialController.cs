using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class PartialController : BaseController
    {
        //
        // GET: /cms/Partial/

        #region Degiskenler
        private DilRepository dilRepository = null;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DilUlkeSehir()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            return PartialView(diller);
        }

    }
}
