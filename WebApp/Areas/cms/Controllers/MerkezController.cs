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
    public class MerkezController : BaseController
    {
        //
        // GET: /cms/Merkez/
        #region Değişkenler
        private IMerkezRepository merkezRepository = null;
        private GenericRepository<DilOkulu_Merkez> merkezGenericRepository = null;
        #endregion

        public ActionResult Index()
        {
            merkezRepository = new MerkezRepository();
            var merkezler = merkezRepository.Liste().Where(m => m.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(m => m.Baslik).ToList();
            return View(merkezler);
        }

        public ActionResult Yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Yeni(FormCollection fColl, IEnumerable<HttpPostedFileBase> files, string[] chk_arrAkreditasyonlar)
        {
            #region FormCollection
            string baslik = fColl["Baslik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string ozet = fColl["Ozet"];
            string genelBilgi = fColl["GenelBilgi"];
            string kurulusTarihi = fColl["KurulusTarihi"];
            string okulSiteLink = fColl["OkulSiteLink"];
            string facebookUrl = fColl["FacebookUrl"];
            string youtubeUrl = fColl["YoutubeUrl"];
            string twitterUrl = fColl["TwitterUrl"];
            DateTime kayitTarihi = DateTime.Now;
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_Merkez merkez = new DilOkulu_Merkez();

                //merkez.DilId = dilId;
                //merkez.UlkeId = ulkeId;
                //merkez.SehirId = sehirId;
                merkez.Baslik = baslik;
                merkez.Url = Tools.ReplaceTitle(baslik.Trim());
                merkez.GenelBilgi = genelBilgi;
                merkez.Seo_Keywords = seo_Keywords;
                merkez.Seo_Descriptions = seo_Descriptions;
                merkez.Ozet = ozet;
                merkez.KurulusTarihi = kurulusTarihi;
                merkez.Akreditasyonlar = (chk_arrAkreditasyonlar != null && chk_arrAkreditasyonlar.Length > 0 ? string.Join(", ", chk_arrAkreditasyonlar) : "");
                merkez.OkulSiteLink = okulSiteLink;
                merkez.FacebookLink = facebookUrl;
                merkez.YoutubeLink = youtubeUrl;
                merkez.TwitterLink = twitterUrl;
                merkez.KayitTarihi = DateTime.Now;
                merkez.Oncelik = 0;
                merkez.Durumu = durumu;

                #region File Uploads
                if (files != null && files.Count() > 0)
                {
                    string fileName = "";
                    int counter = 0;
                    foreach (var file in files)
                    {
                        counter++;
                        if (file != null && file.ContentLength > 0)
                        {
                            fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/content/uploads"), fileName);
                            file.SaveAs(path);
                        }
                        else
                        {
                            fileName = "";
                        }

                        switch (counter)
                        {
                            case 1:
                                merkez.Logo = fileName;
                                break;
                            case 2:
                                merkez.FiyatListesi = fileName;
                                break;
                            case 3:
                                merkez.Promosyonlar = fileName;
                                break;
                            case 4:
                                merkez.Brosur = fileName;
                                break;
                        }

                    }

                }
                #endregion

                merkezGenericRepository = new GenericRepository<DilOkulu_Merkez>();
                DilOkulu_Merkez retMerkez = merkezGenericRepository.Insert(merkez);

                if (retMerkez != null)
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

        public ActionResult Detay(int Id = 0)
        {
            merkezRepository = new MerkezRepository();
            var merkez = merkezRepository.Detay(Id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            return View(merkez);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detay(FormCollection fColl, IEnumerable<HttpPostedFileBase> files, string[] chk_arrAkreditasyonlar)
        {
            #region FormCollection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string seo_Keywords = fColl["Seo_Keywords"];
            string seo_Descriptions = fColl["Seo_Descriptions"];
            string ozet = fColl["Ozet"];
            string genelBilgi = fColl["GenelBilgi"];
            string kurulusTarihi = fColl["KurulusTarihi"];
            string okulSiteLink = fColl["OkulSiteLink"];
            string facebookUrl = fColl["FacebookUrl"];
            string youtubeUrl = fColl["YoutubeUrl"];
            string twitterUrl = fColl["TwitterUrl"];

            bool silindiFiyatListesi = Convert.ToBoolean(fColl["hfSilindiFiyatListesi"]);
            bool silindiPromosyonlar = Convert.ToBoolean(fColl["hfSilindiPromosyonlar"]);
            bool silindiBrosur = Convert.ToBoolean(fColl["hfSilindiBrosur"]);

            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            merkezRepository = new MerkezRepository();
            var merkez = merkezRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            if (!string.IsNullOrEmpty(baslik))
            {
                merkez.Baslik = baslik;
                merkez.Url = Tools.ReplaceTitle(baslik.Trim());
                merkez.GenelBilgi = genelBilgi;
                merkez.Seo_Keywords = seo_Keywords;
                merkez.Seo_Descriptions = seo_Descriptions;
                merkez.Ozet = ozet;
                merkez.KurulusTarihi = kurulusTarihi;
                merkez.Akreditasyonlar = (chk_arrAkreditasyonlar != null && chk_arrAkreditasyonlar.Length > 0 ? string.Join(", ", chk_arrAkreditasyonlar) : "");
                merkez.OkulSiteLink = okulSiteLink;
                merkez.FacebookLink = facebookUrl;
                merkez.YoutubeLink = youtubeUrl;
                merkez.TwitterLink = twitterUrl;
                merkez.KayitTarihi = DateTime.Now;
                merkez.Oncelik = 0;
                merkez.Durumu = durumu;

                if (silindiFiyatListesi)
                    merkez.FiyatListesi = string.Empty;

                if (silindiPromosyonlar)
                    merkez.Promosyonlar = string.Empty;

                if (silindiBrosur)
                    merkez.Brosur = string.Empty;

                #region File Uploads
                if (files != null && files.Count() > 0)
                {
                    string fileName = "";
                    int counter = 0;
                    foreach (var file in files)
                    {
                        counter++;
                        if (file != null && file.ContentLength > 0)
                        {
                            fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/content/uploads"), fileName);
                            file.SaveAs(path);
                        }
                        else
                        {
                            fileName = "";
                        }

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            switch (counter)
                            {
                                case 1:
                                    merkez.Logo = fileName;
                                    break;
                                case 2:
                                    merkez.FiyatListesi = fileName;
                                    break;
                                case 3:
                                    merkez.Promosyonlar = fileName;
                                    break;
                                case 4:
                                    merkez.Brosur = fileName;
                                    break;
                            }
                        }
                    }

                }
                #endregion

                merkezGenericRepository = new GenericRepository<DilOkulu_Merkez>(merkezRepository.DBContext);
                DilOkulu_Merkez retMerkez = merkezGenericRepository.Update(merkez);

                if (retMerkez != null)
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

            return View(merkez);
        }
    }
}
