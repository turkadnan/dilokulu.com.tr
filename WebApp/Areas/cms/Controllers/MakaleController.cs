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
    public class MakaleController : BaseController
    {
        //
        // GET: /cms/Makale/
        #region Degiskenler
        private MakaleKategoriViewModel makaleKategoriViewModel = null;
        private IMakaleRepository makaleRepository = null;
        private IYazarRepository yazarRepository = null;
        private GenericRepository<DilOkulu_Makaleler> makaleGenericRepository = null;
        #endregion

        #region Liste Ekranı
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MakaleAcademix()
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.danisman, (int)GeneralVariables.MakaleKategorileri.Danisman);
            return View(myMakaleKategoriViewModel);
        }

        public ActionResult MakaleOgrenci()
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.ogrenci, (int)GeneralVariables.MakaleKategorileri.Ogrenci);
            return View(myMakaleKategoriViewModel);
        }

        #endregion

        #region Yeni Makale

        public ActionResult MakaleYeniAcademix()
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.danisman, 0);
            return View(myMakaleKategoriViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MakaleYeniAcademix(FormCollection fCol, HttpPostedFileBase file)
        {
            #region Form Collection
            int yazarId = Convert.ToInt32(fCol["YazarId"]);
            string baslik = fCol["Baslik"];
            string ozet = fCol["Ozet"];
            string icerik = fCol["Icerik"];
            string Seo_Keywords = fCol["Seo_Keywords"];
            string Seo_Descriptions = fCol["Seo_Descriptions"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);
            #endregion

            if (!string.IsNullOrEmpty(baslik) && yazarId > 0 && !string.IsNullOrEmpty(ozet))
            {
                DilOkulu_Makaleler makale = new DilOkulu_Makaleler()
                {
                    Baslik = baslik.Trim(),
                    KategoriId = (int)GeneralVariables.MakaleKategorileri.Danisman,
                    YazarId = yazarId,
                    Ozet = ozet,
                    Icerik = icerik,
                    Seo_Keywords = Seo_Keywords,
                    Seo_Descriptions = Seo_Descriptions,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Durumu = Durumu,
                    KayitTarihi = DateTime.Now
                };

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik,ref resimUrl,ref resimThumbUrl);

                    makale.ResimUrl = resimUrl;
                    makale.ResimThumbUrl = resimThumbUrl;
                }

                makaleGenericRepository = new GenericRepository<DilOkulu_Makaleler>();
                var RetMakale = makaleGenericRepository.Insert(makale);
                if (RetMakale != null)
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

            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.academix, 0);
            return View(myMakaleKategoriViewModel);
        }

        public ActionResult MakaleYeniOgrenci()
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.ogrenci, 0);
            return View(myMakaleKategoriViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MakaleYeniOgrenci(FormCollection fCol, HttpPostedFileBase file)
        {
            #region Form Collection
            int yazarId = Convert.ToInt32(fCol["YazarId"]);
            string baslik = fCol["Baslik"];
            string ozet = fCol["Ozet"];
            string icerik = fCol["Icerik"];
            string Seo_Keywords = fCol["Seo_Keywords"];
            string Seo_Descriptions = fCol["Seo_Descriptions"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);
            #endregion

            if (!string.IsNullOrEmpty(baslik) && yazarId  > 0 && !string.IsNullOrEmpty(ozet))
            {
                DilOkulu_Makaleler makale = new DilOkulu_Makaleler()
                {
                    Baslik = baslik.Trim(),
                    KategoriId = (int)GeneralVariables.MakaleKategorileri.Ogrenci,
                    YazarId = yazarId,
                    Ozet = ozet,
                    Icerik = icerik,
                    Seo_Keywords = Seo_Keywords,
                    Seo_Descriptions = Seo_Descriptions,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Durumu = Durumu,
                    KayitTarihi = DateTime.Now
                };

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    makale.ResimUrl = resimUrl;
                    makale.ResimThumbUrl = resimThumbUrl;
                }

                makaleGenericRepository = new GenericRepository<DilOkulu_Makaleler>();
                var RetMakale = makaleGenericRepository.Insert(makale);
                if (RetMakale != null)
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

            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.ogrenci, 0);
            return View(myMakaleKategoriViewModel);
        }
        #endregion

        #region Makale Detay

        public ActionResult MakaleAcademixDetay(int id = 0)
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.danisman, 0, id);
            return View(myMakaleKategoriViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MakaleAcademixDetay(FormCollection fCol, HttpPostedFileBase file)
        {
            #region Form Collection
            int id = Convert.ToInt32(fCol["id"]);
            int yazarId = Convert.ToInt32(fCol["YazarId"]);
            string baslik = fCol["Baslik"];
            string ozet = fCol["Ozet"];
            string icerik = fCol["Icerik"];
            string Seo_Keywords = fCol["Seo_Keywords"];
            string Seo_Descriptions = fCol["Seo_Descriptions"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);
            #endregion

            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.danisman, 0, id);

            if (!string.IsNullOrEmpty(baslik) && yazarId  > 0 && !string.IsNullOrEmpty(ozet))
            {

                myMakaleKategoriViewModel.Makale.Baslik = baslik.Trim();
                myMakaleKategoriViewModel.Makale.YazarId = yazarId;
                myMakaleKategoriViewModel.Makale.Ozet = ozet;
                myMakaleKategoriViewModel.Makale.Icerik = icerik;
                myMakaleKategoriViewModel.Makale.Seo_Keywords = Seo_Keywords;
                myMakaleKategoriViewModel.Makale.Seo_Descriptions = Seo_Descriptions;
                myMakaleKategoriViewModel.Makale.Url = Tools.ReplaceTitle(baslik.Trim());
                myMakaleKategoriViewModel.Makale.Durumu = Durumu;

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    myMakaleKategoriViewModel.Makale.ResimUrl = resimUrl;
                    myMakaleKategoriViewModel.Makale.ResimThumbUrl = resimThumbUrl;
                }

                makaleGenericRepository = new GenericRepository<DilOkulu_Makaleler>(makaleRepository.DBContext);
                var RetMakale = makaleGenericRepository.Update(myMakaleKategoriViewModel.Makale);
                if (RetMakale != null)
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


            return View(myMakaleKategoriViewModel);
        }

        public ActionResult MakaleOgrenciDetay(int id = 0)
        {
            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.ogrenci, 0, id);
            return View(myMakaleKategoriViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MakaleOgrenciDetay(FormCollection fCol, HttpPostedFileBase file)
        {
            #region Form Collection
            int id = Convert.ToInt32(fCol["id"]);
            int yazarId = Convert.ToInt32(fCol["YazarId"]);
            string baslik = fCol["Baslik"];
            string ozet = fCol["Ozet"];
            string icerik = fCol["Icerik"];
            string Seo_Keywords = fCol["Seo_Keywords"];
            string Seo_Descriptions = fCol["Seo_Descriptions"];
            byte Durumu = Convert.ToByte(fCol["selectDurum"]);
            #endregion

            MakaleKategoriViewModel myMakaleKategoriViewModel = MakaleDetaylariniGetir(GeneralVariables.OrtakTip.ogrenci, 0, id);

            if (!string.IsNullOrEmpty(baslik) && yazarId > 0 && !string.IsNullOrEmpty(ozet))
            {

                myMakaleKategoriViewModel.Makale.Baslik = baslik.Trim();
                myMakaleKategoriViewModel.Makale.YazarId = yazarId;
                myMakaleKategoriViewModel.Makale.Ozet = ozet;
                myMakaleKategoriViewModel.Makale.Icerik = icerik;
                myMakaleKategoriViewModel.Makale.Seo_Keywords = Seo_Keywords;
                myMakaleKategoriViewModel.Makale.Seo_Descriptions = Seo_Descriptions;
                myMakaleKategoriViewModel.Makale.Url = Tools.ReplaceTitle(baslik.Trim());
                myMakaleKategoriViewModel.Makale.Durumu = Durumu;

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    myMakaleKategoriViewModel.Makale.ResimUrl = resimUrl;
                    myMakaleKategoriViewModel.Makale.ResimThumbUrl = resimThumbUrl;
                }

                makaleGenericRepository = new GenericRepository<DilOkulu_Makaleler>(makaleRepository.DBContext);
                var RetMakale = makaleGenericRepository.Update(myMakaleKategoriViewModel.Makale);
                if (RetMakale != null)
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


            return View(myMakaleKategoriViewModel);
        }
        #endregion

        private MakaleKategoriViewModel MakaleDetaylariniGetir(GeneralVariables.OrtakTip yaziTipi, int kategoriId = 0, int makaleId = 0)
        {
            try
            {
                makaleRepository = new MakaleRepository();
                yazarRepository = new YazarRepository(makaleRepository.DBContext);

                string makaleYazarTipi = yaziTipi.ToString();

                makaleKategoriViewModel = new MakaleKategoriViewModel()
                {
                    //Makaleler
                    Makaleler = makaleRepository.Liste()
                    .Where(
                    m =>
                        m.DilOkulu_Yazarlar.YazarTipi == makaleYazarTipi &&
                        m.Durumu != (int)GeneralVariables.Durum.Silindi &&
                        (kategoriId > 0 ? m.KategoriId == kategoriId : 1 == 1)
                        ).ToList(),

                    //Yazarlar
                    Yazarlar = yazarRepository.Liste()
                     .Where(
                    y =>
                   y.YazarTipi == makaleYazarTipi &&
                    y.Durumu != (int)GeneralVariables.Durum.Silindi
                    ).Select(
                    y =>
                       new { y.Id, y.AdSoyad, y.Durumu, y.YazarTipi }
                    ).ToList().Select(k => new KeyValuePair<int, string>(k.Id, k.AdSoyad)),
                    //Makale Detay
                    Makale = (makaleId > 0 ? makaleRepository.Detay(makaleId, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif }) : null)
                };
            }
            catch (Exception)
            {

                makaleKategoriViewModel = null;
            }


            return makaleKategoriViewModel;
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Makale) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Makale) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/makaleler/" + fileName;
                resimThumbUrl = "~/content/images/makaleler/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
