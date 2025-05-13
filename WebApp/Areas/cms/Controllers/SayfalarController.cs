using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class SayfalarController : BaseController
    {
        //
        // GET: /cms/Sayfalar/

        #region Degiskenler
        private IStatikSayfaRepository statikSayfaRepository = null;
        private IStatikLinkRepository statikLinkRepository = null;
        private GenericRepository<DilOkulu_StatikSayfalar> statikSayfalarGenericRepository = null;
        private GenericRepository<DilOkulu_StatikLinkler> statikLinklerGenericRepository = null;
        #endregion

        public ActionResult hakkimizda()
        {
            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("hakkimizda");
            return View(sayfa);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult hakkimizda(FormCollection fColl)
        {
            #region FormCollection
            int id = Convert.ToInt32(fColl["Id"]);
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string icerik = fColl["Icerik"];
            #endregion

            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("hakkimizda");

            if (sayfa != null)
            {
                sayfa.Seo_Descriptions = seo_Descriptions;
                sayfa.Seo_Keywords = seo_Keywords;
                sayfa.Icerik = icerik;

                statikSayfalarGenericRepository = new GenericRepository<DilOkulu_StatikSayfalar>(statikSayfaRepository.DBContext);
                var retSayfa = statikSayfalarGenericRepository.Update(sayfa);
                if (retSayfa != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            return View(sayfa);
        }

        public ActionResult iletisim()
        {
            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("iletisim");
            return View(sayfa);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult iletisim(FormCollection fColl)
        {
            #region FormCollection
            int id = Convert.ToInt32(fColl["Id"]);
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string icerik = fColl["Icerik"];
            #endregion

            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("iletisim");

            if (sayfa != null)
            {
                sayfa.Seo_Descriptions = seo_Descriptions;
                sayfa.Seo_Keywords = seo_Keywords;
                sayfa.Icerik = icerik;

                statikSayfalarGenericRepository = new GenericRepository<DilOkulu_StatikSayfalar>(statikSayfaRepository.DBContext);
                var retSayfa = statikSayfalarGenericRepository.Update(sayfa);
                if (retSayfa != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            return View(sayfa);
        }

        public ActionResult acenta()
        {
            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("acenta");
            return View(sayfa);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult acenta(FormCollection fColl)
        {
            #region FormCollection
            int id = Convert.ToInt32(fColl["Id"]);
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string icerik = fColl["Icerik"];
            #endregion

            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("acenta");

            if (sayfa != null)
            {
                sayfa.Seo_Descriptions = seo_Descriptions;
                sayfa.Seo_Keywords = seo_Keywords;
                sayfa.Icerik = icerik;

                statikSayfalarGenericRepository = new GenericRepository<DilOkulu_StatikSayfalar>(statikSayfaRepository.DBContext);
                var retSayfa = statikSayfalarGenericRepository.Update(sayfa);
                if (retSayfa != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            return View(sayfa);
        }

        public ActionResult AnasayfaSeo()
        {
            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("anasayfa");
            return View(sayfa);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AnasayfaSeo(FormCollection fColl)
        {
            #region FormCollection
            int id = Convert.ToInt32(fColl["Id"]);
            string seo_Title = fColl["Seo_Title"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            #endregion

            statikSayfaRepository = new StatikSayfaRepository();
            var sayfa = statikSayfaRepository.Detay("anasayfa");

            if (sayfa != null)
            {
                sayfa.Seo_Title = seo_Title;
                sayfa.Seo_Descriptions = seo_Descriptions;
                sayfa.Seo_Keywords = seo_Keywords;

                statikSayfalarGenericRepository = new GenericRepository<DilOkulu_StatikSayfalar>(statikSayfaRepository.DBContext);
                var retSayfa = statikSayfalarGenericRepository.Update(sayfa);
                if (retSayfa != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            return View(sayfa);
        }

        public ActionResult StatikLinkler()
        {
            statikLinkRepository = new StatikLinkRepository();
            var linkler = statikLinkRepository.Liste().Where(l => l.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(l => l.LinkTipi).ThenBy(l => l.Oncelik).ToList();
            return View(linkler);
        }

        [HttpPost]
        public ActionResult StatikLinkler(FormCollection fColl)
        {
            #region Form Collections
            string baslik = fColl["Baslik"];
            string link = fColl["Link"];
            string tip = fColl["Tip"];
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);

            #endregion

            statikLinkRepository = new StatikLinkRepository();

            if (baslik.Length > 0 && link.Length > 0 && tip.Length > 0)
            {
                statikLinklerGenericRepository = new GenericRepository<DilOkulu_StatikLinkler>(statikLinkRepository.DBContext);
                DilOkulu_StatikLinkler statikLink = new DilOkulu_StatikLinkler()
                {
                    Baslik = baslik,
                    Link = link,
                    LinkTipi = tip,
                    Durumu = 1,
                    Oncelik = oncelik,
                    KayitTarihi = DateTime.Now
                };

                var retLink = statikLinklerGenericRepository.Insert(statikLink);

                if (retLink != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }

            var linkler = statikLinkRepository.Liste().Where(l => l.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(l => l.LinkTipi).ThenBy(l => l.Oncelik).ToList();

            return View(linkler);
        }

    }
}
