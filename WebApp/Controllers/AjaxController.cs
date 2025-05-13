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
    public class AjaxController : Controller
    {
        //
        // GET: /Ajax/
        #region Degiskenler
        private UlkeRepository ulkeRepository = null;
        private SehirRepository sehirRepository = null;
        #endregion

        public ActionResult UlkeListesi(int dilId)
        {
            ulkeRepository = new UlkeRepository();
            var ulkeler = ulkeRepository.Liste().Where(u => u.Durumu == 1 && u.DilId == dilId)
                .Select(u =>
                    new
                    {
                        u.Id,
                        u.Baslik,
                        u.Durumu
                    }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik));

            return Json(new { ulkeler = ulkeler }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SehirlerListesi(int ulkeId = 0)
        {
            sehirRepository = new SehirRepository();
            IEnumerable<KeyValuePair<int, string>> sehirler =
                sehirRepository.Liste().Where(u => u.UlkeId == ulkeId && u.Durumu == 1).Select(u => new { u.Baslik, u.Id, u.Durumu }).OrderBy(u => u.Baslik).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik));

            return Json(new { sehirler = sehirler }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetUrlString(string dil, string ulke, string sehir)
        {
            dil = Tools.ReplaceTitle(dil);
            ulke = Tools.ReplaceTitle(ulke);
            sehir = Tools.ReplaceTitle(sehir);

            return Json(new { dil = dil, ulke = ulke, sehir = sehir }, JsonRequestBehavior.AllowGet);
        }

    }
}
