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
    public class HaberlerController : BaseController
    {
        //
        // GET: /cms/Haberler/

        #region Degiskenler
        private IHaberRepository haberRepository = null;
        private GenericRepository<DilOkulu_Haberler> haberGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            haberRepository = new HaberRepository();
            var haberler = haberRepository.Liste().Where(h => h.Durumu != 3).OrderBy(h => h.Oncelik).ToList();
            return View(haberler);
        }

        public ActionResult Yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Yeni(FormCollection fColl, HttpPostedFileBase file)
        {
            #region Form Collection
            string baslik = fColl["Baslik"];
            string ozet = fColl["Ozet"];
            string icerik = fColl["Icerik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            if (!string.IsNullOrEmpty(baslik) && !string.IsNullOrEmpty(ozet))
            {
                DilOkulu_Haberler haber = new DilOkulu_Haberler()
                {
                    Baslik = baslik.Trim(),
                    Ozet = ozet,
                    Icerik = icerik,
                    Seo_Keywords = seo_Keywords,
                    Seo_Descriptions = seo_Descriptions,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Oncelik = oncelik,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    haber.ResimUrl = resimUrl;
                    haber.ResimThumbUrl = resimThumbUrl;
                }

                haberGenericRepository = new GenericRepository<DilOkulu_Haberler>();
                var RetMakale = haberGenericRepository.Insert(haber);
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

            return View();
        }

        public ActionResult Detay(int id = 0)
        {
            haberRepository = new HaberRepository();
            var haber = haberRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(haber);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detay(FormCollection fColl, HttpPostedFileBase file)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string ozet = fColl["Ozet"];
            string icerik = fColl["Icerik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            haberRepository = new HaberRepository();
            var haber = haberRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (id > 0 && !string.IsNullOrEmpty(baslik) && !string.IsNullOrEmpty(ozet))
            {
                haberRepository = new HaberRepository();

                if (haber != null)
                {
                    haber.Baslik = baslik.Trim();
                    haber.Ozet = ozet;
                    haber.Icerik = icerik;
                    haber.Seo_Keywords = seo_Keywords;
                    haber.Seo_Descriptions = seo_Descriptions;
                    haber.Url = Tools.ReplaceTitle(baslik.Trim());
                    haber.Durumu = durumu;
                    haber.Oncelik = oncelik;

                    if (file != null && file.ContentLength > 0)
                    {
                        string resimUrl = "";
                        string resimThumbUrl = "";
                        GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                        haber.ResimUrl = resimUrl;
                        haber.ResimThumbUrl = resimThumbUrl;
                    }

                    haberGenericRepository = new GenericRepository<DilOkulu_Haberler>();
                    var retHaber = haberGenericRepository.Update(haber);
                    if (retHaber != null)
                    {
                        ViewBag.Status = "ok";
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

            return View(haber);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Haber) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Haber) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/haberler/" + fileName;
                resimThumbUrl = "~/content/images/haberler/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
