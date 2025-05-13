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
    public class DillerController : BaseController
    {
        //
        // GET: /cms/Diller/

        #region Degiskernler
        private IDilRepository dilRepository = null;
        private GenericRepository<DilOkulu_Diller> dilGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu == 1).OrderBy(d => d.Baslik).ToList();

            return View(diller);
        }

        public ActionResult yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fColl, HttpPostedFileBase fileResim)
        {

            #region Form Collection
            string baslik = fColl["Baslik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string aciklama = fColl["Aciklama"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);

            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_Diller dil = new DilOkulu_Diller()
                {
                    Baslik = baslik,
                    Seo_Keywords = seo_Keywords,
                    Seo_Descriptions = seo_Descriptions,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Aciklama = aciklama,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                if (fileResim != null && fileResim.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(fileResim.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    dil.Resim = resimUrl;
                    dil.Resim = resimThumbUrl;
                }

                dilGenericRepository = new GenericRepository<DilOkulu_Diller>();
                var RetDil = dilGenericRepository.Insert(dil);
                if (RetDil != null)
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
            dilRepository = new DilRepository();
            var dil = dilRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(dil);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult detay(FormCollection fColl, HttpPostedFileBase fileResim)
        {

            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string aciklama = fColl["Aciklama"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            dilRepository = new DilRepository();
            var dil = dilRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(baslik))
            {
                dil.Baslik = baslik;
                dil.Url = Tools.ReplaceTitle(baslik.Trim());
                dil.Seo_Keywords = seo_Keywords;
                dil.Seo_Descriptions = seo_Descriptions;
                dil.Aciklama = aciklama;
                dil.Durumu = durumu;

                if (fileResim != null && fileResim.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(fileResim.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    dil.Resim = resimUrl;
                    dil.Resim = resimThumbUrl;
                }

                dilGenericRepository = new GenericRepository<DilOkulu_Diller>(dilRepository.DBContext);
                var RetDil = dilGenericRepository.Update(dil);
                if (RetDil != null)
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
            return View(dil);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Dil) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Dil) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/dil/" + fileName;
                resimThumbUrl = "~/content/images/dil/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
