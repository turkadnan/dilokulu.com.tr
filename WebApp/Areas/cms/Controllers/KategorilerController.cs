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
    public class KategorilerController : Controller
    {
        //
        // GET: /cms/Kategoriler/

        #region Variables
        private GenericRepository<DilOkulu_KursKategorileri> genericKursKategori = null;
        private KursKategoriRepository kursKategoriRepository = null;
        private DilRepository dilRepository = null;

        private GenericRepository<DilOkulu_KonaklamaKategorileri> genericKonaklamaKategori = null;
        private KonaklamaKategoriRepository konaklamaKategoriRepository = null;
        #endregion

        #region Kurs Kategori İşlemleri
        public ActionResult KursKategorileri()
        {
            kursKategoriRepository = new KursKategoriRepository();
            var kategoriler = kursKategoriRepository.Liste().Where(d => d.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(d => d.DilOkulu_Diller.Baslik).ThenBy(d => d.Baslik).ToList();

            return View(kategoriler);
        }

        public ActionResult YeniKursKategorisi()
        {
            ViewBag.Diller = DillerListesi();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult YeniKursKategorisi(FormCollection fColl)
        {

            #region Form Collection
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int dilId = Convert.ToInt32(fColl["selectDil"]);
            #endregion

            ViewBag.Diller = DillerListesi();

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_KursKategorileri kategori = new DilOkulu_KursKategorileri()
                {
                    Baslik = baslik,
                    DilId = dilId,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                genericKursKategori = new GenericRepository<DilOkulu_KursKategorileri>();
                var retKategori = genericKursKategori.Insert(kategori);
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

            return View();
        }

        public ActionResult KursKategorisiDetay(int id = 0)
        {
            ViewBag.Diller = DillerListesi();
            kursKategoriRepository = new KursKategoriRepository();
            var kategori = kursKategoriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(kategori);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KursKategorisiDetay(FormCollection fColl)
        {

            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int dilId = Convert.ToInt32(fColl["selectDil"]);
            #endregion

            ViewBag.Diller = DillerListesi();

            kursKategoriRepository = new KursKategoriRepository();
            var kategori = kursKategoriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(baslik))
            {
                kategori.Baslik = baslik;
                kategori.DilId = dilId;
                kategori.Durumu = durumu;

                genericKursKategori = new GenericRepository<DilOkulu_KursKategorileri>(kursKategoriRepository.DBContext);
                var retKategori = genericKursKategori.Update(kategori);
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
            return View(kategori);
        }
        #endregion

        #region Konaklama Kategori İşlemleri
        public ActionResult KonaklamaKategorileri()
        {
            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var kategoriler = konaklamaKategoriRepository.Liste().Where(d => d.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(d => d.Baslik).ToList();

            return View(kategoriler);
        }

        public ActionResult YeniKonaklamaKategorisi()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult YeniKonaklamaKategorisi(FormCollection fColl)
        {

            #region Form Collection
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);

            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_KonaklamaKategorileri kategori = new DilOkulu_KonaklamaKategorileri()
                {
                    Baslik = baslik,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                genericKonaklamaKategori = new GenericRepository<DilOkulu_KonaklamaKategorileri>();
                var retKategori = genericKonaklamaKategori.Insert(kategori);
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

            return View();
        }

        public ActionResult KonaklamaKategorisiDetay(int id = 0)
        {
            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var kategori = konaklamaKategoriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(kategori);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult KonaklamaKategorisiDetay(FormCollection fColl)
        {

            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var kategori = konaklamaKategoriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(baslik))
            {
                kategori.Baslik = baslik;
                kategori.Durumu = durumu;

                genericKonaklamaKategori = new GenericRepository<DilOkulu_KonaklamaKategorileri>(konaklamaKategoriRepository.DBContext);
                var retKategori = genericKonaklamaKategori.Update(kategori);
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
            return View(kategori);
        }
        #endregion

        public List<DilOkulu_Diller> DillerListesi()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            return diller;
        }

    }
}
