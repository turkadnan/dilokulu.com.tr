using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class FiyatListesiController : Controller
    {
        //
        // GET: /FiyatListesi/

        #region Değişkenler
        private IMerkezRepository merkezRepository = null;
        #endregion

        public ActionResult Index()
        {
            merkezRepository = new MerkezRepository();
            var merkezler = merkezRepository.Liste().Where(m => m.Durumu == (int)GeneralVariables.Durum.Aktif).OrderBy(m=>m.Baslik).ToList();
            return View(merkezler);
        }

    }
}
