using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class YazarController : BaseController
    {
        //
        // GET: /cms/Yazar/

        #region Degiskenler
        private IYazarRepository yazarRepository = null;
        private GenericRepository<DilOkulu_Yazarlar> yazarGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fCol)
        {
            string yazarTipi = fCol["YazarTipi"].ToString();

            yazarRepository = new YazarRepository();

            var yazarlar = yazarRepository.Liste().Where(y => y.YazarTipi == yazarTipi).OrderBy(y => y.AdSoyad).ToList();
            if (yazarlar.Count == 0)
            {
                ViewBag.Status = "Kayıt bulunamadı...";
            }
            else
            {
                ViewBag.Status = "";
            }

            return View(yazarlar);
        }

        public ActionResult yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fCol, HttpPostedFileBase file)
        {

            #region Form Collection
            string adSoyad = fCol["AdSoyad"];
            string pozisyon = fCol["Pozisyon"];
            string ePosta = fCol["EPosta"];
            string telefon = fCol["Telefon"];
            string yazarTipi = fCol["YazarTipi"];
            string aciklama = fCol["Aciklama"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);

            #endregion

            if (!string.IsNullOrEmpty(adSoyad))
            {
                DilOkulu_Yazarlar yazar = new DilOkulu_Yazarlar()
                {
                    AdSoyad = adSoyad,
                    Pozisyon = pozisyon,
                    Telefon = telefon,
                    EPosta = ePosta,
                    YazarTipi = yazarTipi,
                    Aciklama = aciklama,
                    Durumu = Durumu,
                    KayitTarihi = DateTime.Now
                };

                if (file != null && file.ContentLength > 0)
                {
                    yazar.ResimUrl = GetImagePath(file.InputStream, adSoyad); ;
                }

                yazarGenericRepository = new GenericRepository<DilOkulu_Yazarlar>();
                var RetYazar = yazarGenericRepository.Insert(yazar);
                if (RetYazar != null)
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

        public ActionResult detay(int Id = 0)
        {
            yazarRepository = new YazarRepository();
            var yazar = yazarRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(yazar);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult detay(FormCollection fCol, HttpPostedFileBase file)
        {

            #region Form Collection
            int Id = Convert.ToInt32(fCol["Id"]);
            string adSoyad = fCol["AdSoyad"];
            string pozisyon = fCol["Pozisyon"];
            string ePosta = fCol["EPosta"];
            string telefon = fCol["Telefon"];
            string yazarTipi = fCol["YazarTipi"];
            string aciklama = fCol["Aciklama"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);
            #endregion

            yazarRepository = new YazarRepository();
            var yazar = yazarRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(adSoyad))
            {
                yazar.AdSoyad = adSoyad;
                yazar.Pozisyon = pozisyon;
                yazar.Telefon = telefon;
                yazar.EPosta = ePosta;
                yazar.YazarTipi = yazarTipi;
                yazar.Aciklama = aciklama;
                yazar.Durumu = Durumu;

                if (file != null && file.ContentLength > 0)
                {
                    yazar.ResimUrl = GetImagePath(file.InputStream, adSoyad); ;
                }

                yazarGenericRepository = new GenericRepository<DilOkulu_Yazarlar>(yazarRepository.DBContext);
                var RetYazar = yazarGenericRepository.Update(yazar);
                if (RetYazar != null)
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


            return View(yazar);
        }

        private string GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension)
        {
            string tmpPath = "";
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Yazar) + fileName;
                Bitmap img = Tools.ResizeImage(streamImage, 100, 100);
                img.Save(filePath);
                tmpPath = "~/content/images/yazarlar/" + fileName;
            }
            catch
            {
            }

            return tmpPath;
        }
    }
}
