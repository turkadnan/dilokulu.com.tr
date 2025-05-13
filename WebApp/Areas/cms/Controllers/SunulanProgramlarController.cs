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
    public class SunulanProgramlarController : BaseController
    {
        //
        // GET: /cms/SunulanProgramlar/
        #region Degiskenler
        private ISunulanProgramRepository sunulanProgramRepository = null;
        private IOkulRepository okulRepository = null;
        private GenericRepository<DilOkulu_SunulanProgramlar> sunulanProgramGenericRepository = null;
        private GenericRepository<DilOkulu_Okullar> okullarProgramGenericRepository = null;
        #endregion

        public ActionResult Index()
        {

            sunulanProgramRepository = new SunulanProgramRepository();
            var liste = sunulanProgramRepository.Liste().Where(sp => sp.UstProgramId == 0 && sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            ViewBag.Kategoriler = liste;

            return View();

        }

        [HttpPost]
        public ActionResult Index(FormCollection fColl)
        {
            #region Form Collections
            int id = Convert.ToInt32(fColl["SelectKategori"]);
            #endregion

            sunulanProgramRepository = new SunulanProgramRepository();
            var genelListe = sunulanProgramRepository.Liste().Where(sp => sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            var liste = sunulanProgramRepository.Liste().Where(sp => sp.UstProgramId == id && sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();

            ViewBag.Kategoriler = genelListe.Where(k => k.UstProgramId == 0).ToList();
            ViewBag.KategoriID = id;

            return View(liste);
        }

        public ActionResult yeni()
        {
            sunulanProgramRepository = new SunulanProgramRepository();
            var liste = sunulanProgramRepository.Liste().Where(sp => sp.UstProgramId == 0 && sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            ViewBag.Kategoriler = liste;
            return View();
        }

        [HttpPost]
        public ActionResult yeni(FormCollection fColl)
        {
            #region Form Collection
            int kategoriId = Convert.ToInt32("0" + fColl["SelectKategori"]);
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            sunulanProgramRepository = new SunulanProgramRepository();

            if (!string.IsNullOrEmpty(baslik) && kategoriId > 0)
            {
                DilOkulu_SunulanProgramlar kategori = new DilOkulu_SunulanProgramlar()
                {
                    Baslik = baslik.Trim(),
                    UstProgramId = kategoriId,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                sunulanProgramGenericRepository = new GenericRepository<DilOkulu_SunulanProgramlar>();
                var retKategori = sunulanProgramGenericRepository.Insert(kategori);
                if (retKategori != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            else
            {
                ViewBag.Status = "err";
            }

            var liste = sunulanProgramRepository.Liste().Where(sp => sp.UstProgramId == 0 && sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            ViewBag.Kategoriler = liste;
            ViewBag.KategoriID = kategoriId;
            return View();
        }

        public ActionResult detay(int id)
        {
            sunulanProgramRepository = new SunulanProgramRepository();

            var genelListe = sunulanProgramRepository.Liste().Where(sp => sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            var detay = sunulanProgramRepository.Liste().Where(sp => sp.Id == id && sp.Durumu != (int)GeneralVariables.Durum.Silindi).FirstOrDefault();

            ViewBag.Kategoriler = genelListe.Where(k => k.UstProgramId == 0).ToList();
            ViewBag.KategoriID = detay.UstProgramId;
            return View(detay);
        }

        [HttpPost]
        public ActionResult detay(FormCollection fColl)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            int kategoriId = Convert.ToInt32("0" + fColl["SelectKategori"]);
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            bool baslikDegistimi = false;
            string eskiBaslik = "";
            #endregion

            sunulanProgramRepository = new SunulanProgramRepository();
            var genelListe = sunulanProgramRepository.Liste().Where(sp => sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            var detay = sunulanProgramRepository.Liste().Where(sp => sp.Id == id && sp.Durumu != (int)GeneralVariables.Durum.Silindi).FirstOrDefault();

            if (!string.IsNullOrEmpty(baslik) && kategoriId > 0)
            {
                if (detay.Baslik != baslik.Trim())
                {
                    baslikDegistimi = true;
                    eskiBaslik = detay.Baslik;
                }
                detay.Baslik = baslik.Trim();
                detay.UstProgramId = kategoriId;
                detay.Durumu = durumu;

                sunulanProgramGenericRepository = new GenericRepository<DilOkulu_SunulanProgramlar>();
                var retKategori = sunulanProgramGenericRepository.Update(detay);
                if (retKategori != null)
                {

                    if (baslikDegistimi)
                    {
                        OkullardaProgramBasliklariniGuncelle(eskiBaslik, baslik, kategoriId);
                    }

                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            else
            {
                ViewBag.Status = "err";
            }

            ViewBag.Kategoriler = genelListe.Where(k => k.UstProgramId == 0).ToList();
            ViewBag.KategoriID = kategoriId;

            return View(detay);
        }



        public ActionResult kategoriler()
        {
            sunulanProgramRepository = new SunulanProgramRepository();
            var liste = sunulanProgramRepository.Liste().Where(sp => sp.UstProgramId == 0 && sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            return View(liste);
        }

        public ActionResult yenikategori()
        {
            return View();
        }

        [HttpPost]
        public ActionResult yenikategori(FormCollection fColl)
        {
            #region Form Collection
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_SunulanProgramlar kategori = new DilOkulu_SunulanProgramlar()
                {
                    Baslik = baslik.Trim(),
                    UstProgramId = 0,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                sunulanProgramGenericRepository = new GenericRepository<DilOkulu_SunulanProgramlar>();
                var retKategori = sunulanProgramGenericRepository.Insert(kategori);
                if (retKategori != null)
                {
                    return RedirectToAction("kategoriler");
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            else
            {
                ViewBag.Status = "err";
            }

            return View();
        }

        public ActionResult detaykategori(int id)
        {
            sunulanProgramRepository = new SunulanProgramRepository();
            var kategori = sunulanProgramRepository.Liste().Where(sp => sp.Id == id && sp.UstProgramId == 0 && sp.Durumu != (int)GeneralVariables.Durum.Silindi).FirstOrDefault();
            return View(kategori);
        }


        [HttpPost]
        public ActionResult detaykategori(FormCollection fColl)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["id"]);
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            sunulanProgramRepository = new SunulanProgramRepository();
            var kategori = sunulanProgramRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (id > 0 && !string.IsNullOrEmpty(baslik))
            {
                if (kategori != null)
                {
                    kategori.Baslik = baslik.Trim();
                    kategori.Durumu = durumu;

                    sunulanProgramGenericRepository = new GenericRepository<DilOkulu_SunulanProgramlar>();
                    var retSlider = sunulanProgramGenericRepository.Update(kategori);
                    if (retSlider != null)
                    {
                        return RedirectToAction("kategoriler");
                    }
                    else
                    {
                        ViewBag.Status = "err";
                    }
                }
            }
            else
            {
                ViewBag.Status = "err";
            }

            return View(kategori);
        }

        private void OkullardaProgramBasliklariniGuncelle(string eskiBaslik, string baslik, int kategoriId)
        {
            okulRepository = new OkulRepository();
            string kategoriIDveEskiBaslik = kategoriId.ToString() + "|" + eskiBaslik;
            string kategoriIDveYeniBaslik = kategoriId.ToString() + "|" + baslik;
            var okullar = okulRepository.Liste().Where(o => o.SunulanProgramlar.Contains(kategoriIDveEskiBaslik)).ToList();
            if (okullar != null && okullar.Count > 0)
            {
                okullarProgramGenericRepository = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);
                foreach (var okul in okullar)
                {
                    okul.SunulanProgramlar = okul.SunulanProgramlar.Replace(kategoriIDveEskiBaslik, kategoriIDveYeniBaslik);
                    okullarProgramGenericRepository.Update(okul);
                }
            }
        }
    }
}
