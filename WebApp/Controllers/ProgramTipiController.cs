using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    [ValidateInput(false)]
    public class ProgramTipiController : Controller
    {
        //
        // GET: /ProgramTipi/

        #region Degiskenler
        private OkulRepository okulRepository = null;
        private List<DilOkulu_Okullar> okullar = null;
        #endregion

        [ValidateInput(false)]
        public ActionResult Index(string baslik = "", int sayfa = 1)
        {
            baslik = Server.UrlDecode(baslik);

            ViewBag.Baslik = baslik;

            if (!string.IsNullOrEmpty(baslik))
            {

                okulRepository = new OkulRepository();
                int count = okulRepository.Liste()
                        .Where(o => o.SunulanProgramlar.Contains(baslik) && o.Durumu == (int)GeneralVariables.Durum.Aktif).Count();

                int totalPageCount = (int)Math.Ceiling((double)count / (double)15);

                okullar = okulRepository.Liste()
                    .Where(
                    o =>
                        o.SunulanProgramlar.Contains(baslik) && o.Durumu == (int)GeneralVariables.Durum.Aktif
                        ).OrderBy(x => x.Id).Skip((sayfa - 1) * 15).Take(15).ToList();

                ViewBag.Baslik = baslik;
                ViewBag.Sayfa = sayfa.ToString();
                ViewBag.TotalPageCount = totalPageCount.ToString();


            }
            return View(okullar);
        }

    }
}
