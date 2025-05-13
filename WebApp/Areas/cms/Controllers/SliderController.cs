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
    public class SliderController : BaseController
    {
        //
        // GET: /cms/Slider/
        #region Degiskenler
        private ISliderRepository sliderRepository = null;
        private GenericRepository<DilOkulu_Slider> sliderGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            sliderRepository = new SliderRepository();
            var slider = sliderRepository.Liste().Where(h => h.Durumu != 3).OrderBy(h => h.Oncelik).ToList();
            return View(slider);
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
            string ozet = fColl["Ozet"];
            string pageLink = fColl["PageLink"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            if (!string.IsNullOrEmpty(baslik) && !string.IsNullOrEmpty(ozet))
            {
                DilOkulu_Slider slider = new DilOkulu_Slider()
                {
                    Baslik = baslik.Trim(),
                    Ozet = ozet,
                    PageLink = pageLink,
                    Oncelik = oncelik,
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                if (file != null && file.ContentLength > 0)
                {
                    string resimUrl = "";
                    string resimThumbUrl = "";
                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                    slider.ResimUrl = resimUrl;
                    slider.ResimThumbUrl = resimThumbUrl;
                }

                sliderGenericRepository = new GenericRepository<DilOkulu_Slider>();
                var retSlider = sliderGenericRepository.Insert(slider);
                if (retSlider != null)
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
            sliderRepository = new SliderRepository();
            var slider = sliderRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(slider);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detay(FormCollection fColl, HttpPostedFileBase file)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string ozet = fColl["Ozet"];
            string pageLink = fColl["PageLink"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"], out oncelik);
            #endregion

            sliderRepository = new SliderRepository();
            var slider = sliderRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (id > 0 && !string.IsNullOrEmpty(baslik) && !string.IsNullOrEmpty(ozet))
            {
                if (slider != null)
                {
                    slider.Baslik = baslik.Trim();
                    slider.Ozet = ozet;
                    slider.PageLink = pageLink;
                    slider.Durumu = durumu;
                    slider.Oncelik = oncelik;

                    if (file != null && file.ContentLength > 0)
                    {
                        string resimUrl = "";
                        string resimThumbUrl = "";
                        GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                        slider.ResimUrl = resimUrl;
                        slider.ResimThumbUrl = resimThumbUrl;
                    }

                    sliderGenericRepository = new GenericRepository<DilOkulu_Slider>();
                    var retSlider = sliderGenericRepository.Update(slider);
                    if (retSlider != null)
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

            return View(slider);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Slider) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Slider) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 980, 430);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 140, 80);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/slider/" + fileName;
                resimThumbUrl = "~/content/images/slider/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
