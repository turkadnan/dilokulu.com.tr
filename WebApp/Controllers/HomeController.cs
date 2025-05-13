using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Helpers;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        #region Degiskenler
        private IHaberRepository haberRepository = null;
        private IStatikSayfaRepository statikSayfaRepository = null;
        private string seoTitle= "";
        private string seoKeywords = "";
        private string seoDescription = "";
        #endregion

        public ActionResult Index()
        {
            haberRepository = new HaberRepository();
            var haberler = haberRepository.Liste().Where(h => h.Durumu == 1).OrderBy(h => h.Oncelik).ToList();

            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("anasayfa");
            if (sayfa != null)
            {
                seoTitle = sayfa.Seo_Title;
                seoKeywords = sayfa.Seo_Keywords;
                seoDescription = sayfa.Seo_Descriptions;
            }

            ViewBag.SeoTitle = seoTitle;
            ViewBag.SeoKeywords = seoKeywords;
            ViewBag.Description = seoDescription;
            return View(haberler);
        }

    }
}
