using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class OgrenciYorumlariController : Controller
    {
        //
        // GET: /OgrenciYorumlari/

        #region Degiskenler
        private IMakaleRepository makaleRepository = null;
        #endregion

        public ActionResult Index()
        {
            makaleRepository = new MakaleRepository();
            var makaleler = makaleRepository.Liste()
                .Where(oy => oy.Durumu == (int)GeneralVariables.Durum.Aktif && oy.KategoriId == (int)GeneralVariables.MakaleKategorileri.Ogrenci)
                .OrderByDescending(oy => oy.KayitTarihi)
                .ToList();
            return View(makaleler);
        }

        public ActionResult Detay(string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                makaleRepository = new MakaleRepository();
                var makale = makaleRepository.Detay(title, new[] { (int)GeneralVariables.Durum.Aktif });
                return View(makale);
            }
            else
            {
                return View();
            }
        }

    }
}
