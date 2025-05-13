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
    public class VizeRehberiController : BaseController
    {
        //
        // GET: /cms/ViseRehberi/

        #region Degiskenler
        private IVizeRehberiRepository vizeRehberiRepository = null;
        private GenericRepository<DilOkulu_VizeRehberi> vizeRehberiGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            vizeRehberiRepository = new VizeRehberiRepository();
            var rehber = vizeRehberiRepository.Liste().Where(v => v.Durumu != 3).OrderBy(v => v.Oncelik).ToList();
            return View(rehber);
        }

        public ActionResult yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Yeni(FormCollection fColl, HttpPostedFileBase file)
        {
            #region Form Collection
            string baslik = fColl["Baslik"];
            string icerik = fColl["Icerik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_VizeRehberi rehber = new DilOkulu_VizeRehberi()
                {
                    Baslik = baslik.Trim(),
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
                    GetImagePath(file.InputStream, baslik, ref resimUrl);

                    rehber.ResimUrl = resimUrl;
                }

                vizeRehberiGenericRepository = new GenericRepository<DilOkulu_VizeRehberi>();
                var RetRehber = vizeRehberiGenericRepository.Insert(rehber);
                if (RetRehber != null)
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

        public ActionResult detay(int id = 0)
        {
            vizeRehberiRepository = new VizeRehberiRepository();
            var rehber = vizeRehberiRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(rehber);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detay(FormCollection fColl, HttpPostedFileBase file)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string icerik = fColl["Icerik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            vizeRehberiRepository = new VizeRehberiRepository();
            var rehber = vizeRehberiRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (id > 0 && !string.IsNullOrEmpty(baslik))
            {
                if (rehber != null)
                {
                    rehber.Baslik = baslik.Trim();
                    rehber.Icerik = icerik;
                    rehber.Seo_Keywords = seo_Keywords;
                    rehber.Seo_Descriptions = seo_Descriptions;
                    rehber.Url = Tools.ReplaceTitle(baslik.Trim());
                    rehber.Durumu = durumu;
                    rehber.Oncelik = oncelik;

                    if (file != null && file.ContentLength > 0)
                    {
                        string resimUrl = "";
                        GetImagePath(file.InputStream, baslik, ref resimUrl);

                        rehber.ResimUrl = resimUrl;
                    }

                    vizeRehberiGenericRepository = new GenericRepository<DilOkulu_VizeRehberi>();
                    var RetRehber = vizeRehberiGenericRepository.Update(rehber);
                    if (RetRehber != null)
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

            return View(rehber);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.VizeRehberi) + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                resimUrl = "~/content/images/haberler/" + fileName;
            }
            catch
            {
            }
        }

    }
}
