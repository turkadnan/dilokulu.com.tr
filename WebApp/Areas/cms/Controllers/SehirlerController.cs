using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class SehirlerController : BaseController
    {
        //
        // GET: /cms/Sehirler/

        #region Degiskenler
        private UlkeRepository ulkeRepository = null;
        private GenericRepository<DilOkulu_Sehirler> sehirGenericRepository = null;
        private SehirRepository sehirRepository = null;
        private UlkeSehirViewModel ulkeSehirViewModel = null;
        #endregion

        public ActionResult Index()
        {
            ulkeRepository = new UlkeRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value)
            };

            return View(ulkeSehirViewModel);
        }

        [HttpPost]
        public ActionResult Index(FormCollection fCollection)
        {
            int UlkeId = Convert.ToInt32(fCollection["UlkeId"]);

            ulkeRepository = new UlkeRepository();
            sehirRepository = new SehirRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value),

                Sehirler = sehirRepository.Liste().Where(s => s.UlkeId == UlkeId && s.Durumu != 3).OrderBy(s => s.Baslik).ToList()
            };

            ViewBag.UlkeId = UlkeId.ToString();
            return View(ulkeSehirViewModel);
        }

        public ActionResult yeni()
        {
            ulkeRepository = new UlkeRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value)
            };
            return View(ulkeSehirViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fCollection, IEnumerable<HttpPostedFileBase> files)
        {
            #region Form Collection
            int ulkeId = Convert.ToInt32(fCollection["UlkeId"]);
            string baslik = fCollection["Baslik"];
            string aciklama = fCollection["Aciklama"];
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            // byte Oncelik = Tools.IsInteger(fCollection["Oncelik"]) ? Convert.ToByte(fCollection["Oncelik"]) : Convert.ToByte(0);



            #endregion

            ulkeRepository = new UlkeRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value)
            };

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_Sehirler sehir = new DilOkulu_Sehirler()
                {
                    Baslik = baslik.Trim(),
                    UlkeId = ulkeId,
                    Aciklama = aciklama,
                    Seo_Keywords = seo_Keywords,
                    Seo_Descriptions = seo_Descriptions,
                    Url = Tools.ReplaceTitle(baslik.Trim()),
                    Durumu = durumu,
                    Oncelik = 0,
                    KayitTarihi = DateTime.Now,

                };


                #region File Uploads
                if (files != null && files.Count() > 0)
                {
                    string fileName = "";
                    int counter = 0;
                    foreach (var file in files)
                    {
                        counter++;


                        switch (counter)
                        {
                            case 1:
                                if (file != null && file.ContentLength > 0)
                                {
                                    string resimUrl = "";
                                    string resimThumbUrl = "";
                                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                                    sehir.Resim = resimUrl;
                                    sehir.Resim = resimThumbUrl;
                                }
                                break;
                            case 2:
                                if (file != null && file.ContentLength > 0)
                                {
                                    fileName = Path.GetFileName(file.FileName);
                                    var path = Path.Combine(Server.MapPath("~/content/uploads"), fileName);
                                    file.SaveAs(path);
                                    sehir.SehirProfili = fileName;

                                }
                                else
                                {
                                    fileName = "";
                                }


                                break;
                        }

                    }

                }
                #endregion

                sehirGenericRepository = new GenericRepository<DilOkulu_Sehirler>(ulkeRepository.DBContext);
                var RetSehir = sehirGenericRepository.Insert(sehir);
                if (RetSehir != null)
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

            return View(ulkeSehirViewModel);
        }

        public ActionResult detay(int Id = 0)
        {
            ulkeRepository = new UlkeRepository();
            sehirRepository = new SehirRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value),

                Sehir = sehirRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif })
            };

            return View(ulkeSehirViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult detay(FormCollection fCollection, IEnumerable<HttpPostedFileBase> files)
        {
            #region Form Collection
            int id = Convert.ToInt32(fCollection["Id"]);
            int ulkeId = Convert.ToInt32(fCollection["UlkeId"]);
            string baslik = fCollection["Baslik"];
            string aciklama = fCollection["Aciklama"];
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            //byte Oncelik = Tools.IsInteger(fCollection["Oncelik"]) ? Convert.ToByte(fCollection["Oncelik"]) : Convert.ToByte(0);

            bool silindiSehirRehberi = Convert.ToBoolean(fCollection["hfSilindiSehirRehberi"]);
            #endregion

            ulkeRepository = new UlkeRepository();

            ulkeSehirViewModel = new UlkeSehirViewModel()
            {
                Ulkeler = ulkeRepository.Liste().Where(u => u.Durumu != 3).Select(u => new { u.Id, u.Baslik, u.Durumu }).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik)).OrderBy(u => u.Value)
            };

            sehirRepository = new SehirRepository(ulkeRepository.DBContext);
            var sehir = sehirRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(baslik) && ulkeId > 0)
            {
                sehir.UlkeId = ulkeId;
                sehir.Baslik = baslik.Trim();
                sehir.Aciklama = aciklama;
                sehir.Seo_Keywords = seo_Keywords;
                sehir.Seo_Descriptions = seo_Descriptions;
                sehir.Durumu = durumu;
                sehir.Url = Tools.ReplaceTitle(baslik.Trim());
                if (silindiSehirRehberi)
                    sehir.SehirProfili = string.Empty;

                #region File Uploads
                if (files != null && files.Count() > 0)
                {
                    string fileName = "";
                    int counter = 0;
                    foreach (var file in files)
                    {
                        counter++;

                        switch (counter)
                        {
                            case 1:
                                if (file != null && file.ContentLength > 0)
                                {
                                    string resimUrl = "";
                                    string resimThumbUrl = "";
                                    GetImagePath(file.InputStream, baslik, ref resimUrl, ref resimThumbUrl);

                                    sehir.Resim = resimUrl;
                                    sehir.Resim = resimThumbUrl;
                                }
                                break;
                            case 2:
                                if (file != null && file.ContentLength > 0)
                                {
                                    fileName = Path.GetFileName(file.FileName);
                                    var path = Path.Combine(Server.MapPath("~/content/uploads"), fileName);
                                    file.SaveAs(path);
                                    sehir.SehirProfili = fileName;

                                }
                                else
                                {
                                    fileName = "";
                                }


                                break;
                        }

                    }

                }
                #endregion

                sehirGenericRepository = new GenericRepository<DilOkulu_Sehirler>(ulkeRepository.DBContext);
                var RetSehir = sehirGenericRepository.Update(sehir);
                if (RetSehir != null)
                {
                    ulkeSehirViewModel.Sehir = RetSehir;
                    ViewBag.Status = "ok";
                }
                else
                {
                    ulkeSehirViewModel.Sehir = sehir;
                    ViewBag.Status = "err";
                }
            }
            else
            {
                ulkeSehirViewModel.Sehir = sehir;
                ViewBag.Status = "err";
            }



            return View(ulkeSehirViewModel);
        }

        private void GetImagePath(System.IO.Stream streamImage, string fileNameWithoutExtension, ref string resimUrl, ref string resimThumbUrl)
        {
            try
            {
                string fileName = Tools.AppendTimeStamp(Tools.ReplaceTitle(fileNameWithoutExtension) + ".jpg");
                string filePath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Sehir) + fileName;
                string fileThumbPath = GeneralVariables.UploadFilePath(GeneralVariables.UploadType.Sehir) + "tn_" + fileName;

                System.Drawing.Bitmap img = Tools.ResizeImage(streamImage, 640, 400);
                System.Drawing.Bitmap imgThumb = Tools.ResizeImage(streamImage, 100, 100);

                img.Save(filePath);
                imgThumb.Save(fileThumbPath);
                resimUrl = "~/content/images/sehir/" + fileName;
                resimThumbUrl = "~/content/images/sehir/tn_" + fileName;
            }
            catch
            {
            }
        }
    }
}
