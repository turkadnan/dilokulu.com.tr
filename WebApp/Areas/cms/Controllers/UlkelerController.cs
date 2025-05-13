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
    public class UlkelerController : BaseController
    {
        //
        // GET: /cms/Ulkeler/
        #region Degiskenler
        private UlkeRepository ulkeRepository = null;
        private DilRepository dilRepository = null;
        private ParaBirimiRepository paraBirimiRepository = null;
        private GenericRepository<DilOkulu_Ulkeler> ulkeGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            ulkeRepository = new UlkeRepository();
            var ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).OrderByDescending(u => u.Durumu).ThenBy(u=>u.Oncelik).ToList();
            return View(ulkeler);
        }

        public ActionResult yeni()
        {
            #region Diller Listesi
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            ViewBag.Diller = diller;
            #endregion

            #region Para birimleri Listesi
            paraBirimiRepository = new ParaBirimiRepository();
            var paraBirimleri = paraBirimiRepository.Liste().Where(pb => pb.Durumu != 3).OrderBy(d => d.Oncelik).ToList();
            ViewBag.ParaBirimleri = paraBirimleri;
            #endregion

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fCollection, HttpPostedFileBase fileResim)
        {

            #region Form Collection
            int DilId = Convert.ToInt32(fCollection["DilId"]);
            string baslik = fCollection["Baslik"];
            string aciklama = fCollection["Aciklama"];
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            string vizeBilgileri = fCollection["VizeBilgileri"];
            string calismaIzni = fCollection["CalismaIzni"];
            string yasamGiderleri = fCollection["YasamGiderleri"];
            string ulasim = fCollection["Ulasim"];
            string saglikSistemi = fCollection["SaglikSistemi"];

            int? paraBirimId = Tools.IsInteger(fCollection["ParaBirimleri"]) ? Tools.ParseNullableInt(fCollection["ParaBirimleri"]) : null;
            decimal vizeUcreti1 = Tools.IsDecimal(fCollection["VizeUcreti1"]) ? Convert.ToDecimal(fCollection["VizeUcreti1"]) : Convert.ToDecimal(0);
            int? kacAyaKadar1 = Tools.IsInteger(fCollection["KacAyaKadar1"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar1"]) : null;
            decimal vizeUcreti2 = Tools.IsDecimal(fCollection["VizeUcreti2"]) ? Convert.ToDecimal(fCollection["VizeUcreti2"]) : Convert.ToDecimal(0);
            int? kacAyaKadar2 = Tools.IsInteger(fCollection["KacAyaKadar2"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar2"]) : null;
            decimal vizeUcreti3 = Tools.IsDecimal(fCollection["VizeUcreti3"]) ? Convert.ToDecimal(fCollection["VizeUcreti3"]) : Convert.ToDecimal(0);
            int? kacAyaKadar3 = Tools.IsInteger(fCollection["KacAyaKadar3"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar3"]) : null;
           
            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            byte oncelik = Tools.IsInteger(fCollection["Oncelik"]) ? Convert.ToByte(fCollection["Oncelik"]) : Convert.ToByte(0);
            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_Ulkeler ulke = new DilOkulu_Ulkeler()
                {
                    DilId = DilId,
                    Baslik = baslik.Trim(),
                    Aciklama = baslik,
                    Seo_Keywords = seo_Keywords,
                    Seo_Descriptions = seo_Descriptions,
                    ParaBirimId = paraBirimId,
                    
                    VizeUcreti1 = vizeUcreti1,
                    KacAyaKadar1=kacAyaKadar1,
                    VizeUcreti2 = vizeUcreti2,
                    KacAyaKadar2 = kacAyaKadar2,
                    VizeUcreti3 = vizeUcreti3,
                    KacAyaKadar3 = kacAyaKadar3,
                    VizeBilgileri = vizeBilgileri,
                    CalismaIzni = calismaIzni,
                    YasamGiderleri = yasamGiderleri,
                    Ulasim = ulasim,
                    SaglikSistemi = saglikSistemi,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Durumu = durumu,
                    Oncelik = oncelik,
                    KayitTarihi = DateTime.Now
                };

                if (fileResim != null && fileResim.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(fileResim.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    ulke.Resim = resimUrl;
                    ulke.Resim = resimThumbUrl;
                }

                ulkeGenericRepository = new GenericRepository<DilOkulu_Ulkeler>();
                var RetUlke = ulkeGenericRepository.Insert(ulke);
                if (RetUlke != null)
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

            #region Diller Listesi
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            ViewBag.Diller = diller;
            #endregion

            return View();
        }

        public ActionResult detay(int Id = 0)
        {
            ulkeRepository = new UlkeRepository();
            var ulke = ulkeRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            #region Diller Listesi
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            ViewBag.Diller = diller;
            #endregion

            #region Para birimleri Listesi
            paraBirimiRepository = new ParaBirimiRepository();
            var paraBirimleri = paraBirimiRepository.Liste().Where(pb => pb.Durumu != 3).OrderBy(d => d.Oncelik).ToList();
            ViewBag.ParaBirimleri = paraBirimleri;
            #endregion

            return View(ulke);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult detay(FormCollection fCollection, HttpPostedFileBase fileResim)
        {

            #region Form Collection
            int id = Convert.ToInt32(fCollection["Id"]);
            int dilId = Convert.ToInt32(fCollection["DilId"]);
            string baslik = fCollection["Baslik"];
            string aciklama = fCollection["Aciklama"];
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            string vizeBilgileri = fCollection["VizeBilgileri"];
            string calismaIzni = fCollection["CalismaIzni"];
            string yasamGiderleri = fCollection["YasamGiderleri"];
            string ulasim = fCollection["Ulasim"];
            string saglikSistemi = fCollection["SaglikSistemi"];

            int? paraBirimId = Tools.IsInteger(fCollection["ParaBirimleri"]) ? Tools.ParseNullableInt(fCollection["ParaBirimleri"]) : null;
            decimal vizeUcreti1 = Tools.IsDecimal(fCollection["VizeUcreti1"]) ? Convert.ToDecimal(fCollection["VizeUcreti1"]) : Convert.ToDecimal(0);
            int? kacAyaKadar1 = Tools.IsInteger(fCollection["KacAyaKadar1"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar1"]) : null;
            decimal vizeUcreti2 = Tools.IsDecimal(fCollection["VizeUcreti2"]) ? Convert.ToDecimal(fCollection["VizeUcreti2"]) : Convert.ToDecimal(0);
            int? kacAyaKadar2 = Tools.IsInteger(fCollection["KacAyaKadar2"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar2"]) : null;
            decimal vizeUcreti3 = Tools.IsDecimal(fCollection["VizeUcreti3"]) ? Convert.ToDecimal(fCollection["VizeUcreti3"]) : Convert.ToDecimal(0);
            int? kacAyaKadar3 = Tools.IsInteger(fCollection["KacAyaKadar3"]) ? Tools.ParseNullableInt(fCollection["KacAyaKadar3"]) : null;

            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            byte oncelik = Tools.IsInteger(fCollection["Oncelik"]) ? Convert.ToByte(fCollection["Oncelik"]) : Convert.ToByte(0);
            #endregion

            ulkeRepository = new UlkeRepository();
            var ulke = ulkeRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            
            if (!string.IsNullOrEmpty(baslik))
            {
                ulke.DilId = dilId;
                ulke.Baslik = baslik.Trim();
                ulke.Aciklama = aciklama;
                ulke.Seo_Keywords = seo_Keywords;
                ulke.Seo_Descriptions = seo_Descriptions;
                ulke.ParaBirimId = paraBirimId;
                ulke.VizeUcreti1 = vizeUcreti1;
                ulke.KacAyaKadar1 = kacAyaKadar1;
                ulke.VizeUcreti2 = vizeUcreti2;
                ulke.KacAyaKadar2 = kacAyaKadar2;
                ulke.VizeUcreti3 = vizeUcreti3;
                ulke.KacAyaKadar3 = kacAyaKadar3;

                ulke.VizeBilgileri = vizeBilgileri;
                ulke.CalismaIzni = calismaIzni;
                ulke.YasamGiderleri = yasamGiderleri;
                ulke.Ulasim = ulasim;
                ulke.Durumu = durumu;
                ulke.SaglikSistemi = saglikSistemi;
                ulke.Oncelik = oncelik;
                ulke.Url = Tools.ReplaceTitle(baslik.Trim());

                if (fileResim != null && fileResim.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(fileResim.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    ulke.Resim = resimUrl;
                    ulke.Resim = resimThumbUrl;
                }

                ulkeGenericRepository = new GenericRepository<DilOkulu_Ulkeler>(ulkeRepository.dbContext);
                var RetUlke = ulkeGenericRepository.Update(ulke);
                if (RetUlke != null)
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

            #region Diller Listesi
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu != 3).OrderBy(d => d.Baslik).ToList();
            ViewBag.Diller = diller;
            #endregion

            #region Para birimleri Listesi
            paraBirimiRepository = new ParaBirimiRepository();
            var paraBirimleri = paraBirimiRepository.Liste().Where(pb => pb.Durumu != 3).OrderBy(d => d.Oncelik).ToList();
            ViewBag.ParaBirimleri = paraBirimleri;
            #endregion

            return View(ulke);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Ulke) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Ulke) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/ulke/" + fileName;
                resimThumbUrl = "~/content/images/ulke/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
