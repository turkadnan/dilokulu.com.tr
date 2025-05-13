using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class OkullarController : Controller
    {
        //
        // GET: /Okullar/

        #region Degiskenler
        private OkulRepository okulRepository = null;
        private DilRepository dilRepository = null;
        private SehirRepository sehirRepository = null;
        private UlkeRepository ulkeRepository = null;
        private MerkezRepository merkezRepository = null;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dil(string url)
        {
            List<DilOkulu_Okullar> okullar = null;
            dilRepository = new DilRepository();
            var dil = dilRepository.Detay(url, new int[] { (int)GeneralVariables.Durum.Aktif });

            if (dil != null)
            {
                ViewBag.Baslik = dil.Baslik;
                ViewBag.Aciklama = dil.Aciklama;
                ViewBag.Resim = dil.Resim;
                ViewBag.SeoKeywords = dil.Seo_Keywords;
                ViewBag.Description = dil.Seo_Descriptions;

                okulRepository = new OkulRepository();
                okullar = okulRepository.Liste().Where(o => o.DilId == dil.Id && o.Durumu == (int)GeneralVariables.Durum.Aktif).ToList();
            }

            return View(okullar);
        }

        public ActionResult Detay(string merkez, string url)
        {
            string responseUrl = merkez + "-" + url;
            okulRepository = new OkulRepository();
            DilOkulu_Okullar okul = null;
            okul = okulRepository.Detay(responseUrl, new int[] { (int)GeneralVariables.Durum.Aktif });

            ///Benzer okulları getir (Şehire göre)
            ///
            if (okul != null)
            {
                List<DilOkulu_Okullar> benzerOkullar = okulRepository.Liste()
                    .Where
                    (
                    o =>
                        o.DilOkulu_Sehirler.Id == okul.DilOkulu_Sehirler.Id &&
                        o.Id != okul.Id &&
                        o.Durumu == (int)GeneralVariables.Durum.Aktif
                    ).OrderBy(o => Guid.NewGuid()).ToList();

                ViewBag.BenzerOkullar = benzerOkullar;
            }
            else
            {
                return RedirectToAction("index", "PageNotFound");
            }


            return View(okul);
        }

        public ActionResult UlkeSehir(string url)
        {
            List<DilOkulu_Okullar> okullar = null;
            bool isSehir = false;

            sehirRepository = new SehirRepository();
            ulkeRepository = new UlkeRepository(sehirRepository.dbContext);

            Dictionary<int, string> sehirUrlList = sehirRepository.Liste().Where(s => s.Durumu == (int)GeneralVariables.Durum.Aktif)
                                                    .Select(s => new { s.Id, s.Url }).ToDictionary(s => s.Id, s => s.Url);


            //Okul Şehirde aranıyor
            if (sehirUrlList != null && sehirUrlList.Count > 0)
            {
                foreach (KeyValuePair<int, string> item in sehirUrlList)
                {
                    if (item.Value == url)
                    {
                        okulRepository = new OkulRepository();
                        okullar = okulRepository.Liste().Where(o => o.SehirId == item.Key && o.Durumu == (byte)GeneralVariables.Durum.Aktif).ToList();

                        if (okullar != null && okullar.Count > 0)
                        {
                            ViewBag.Aciklama = okullar[0].DilOkulu_Sehirler.Aciklama;
                            ViewBag.Baslik = okullar[0].DilOkulu_Sehirler.Baslik;
                            ViewBag.Resim = okullar[0].DilOkulu_Sehirler.Resim;
                            ViewBag.SeoKeywords = okullar[0].DilOkulu_Sehirler.Seo_Keywords;
                            ViewBag.Description = okullar[0].DilOkulu_Sehirler.Seo_Descriptions;
                            ViewBag.ListeTipi = "Sehir";
                        }

                        isSehir = true;
                        break;
                    }
                }
            }

            //Okul Ülkede aranıyor
            if (isSehir == false)
            {
                Dictionary<int, string> ulkeUrlList = ulkeRepository.Liste().Where(u => u.Durumu == (int)GeneralVariables.Durum.Aktif)
                                                      .Select(u => new { u.Id, u.Url }).ToDictionary(u => u.Id, u => u.Url);
                foreach (KeyValuePair<int, string> item in ulkeUrlList)
                {
                    if (item.Value == url)
                    {
                        okulRepository = new OkulRepository();
                        okullar = okulRepository.Liste().Where(o => o.UlkeId == item.Key && o.Durumu==(byte)GeneralVariables.Durum.Aktif).ToList();
                        if (okullar != null && okullar.Count > 0)
                        {
                            ViewBag.Aciklama = okullar[0].DilOkulu_Ulkeler.Aciklama;
                            ViewBag.Baslik = okullar[0].DilOkulu_Ulkeler.Baslik;
                            ViewBag.Resim = okullar[0].DilOkulu_Ulkeler.Resim;
                            ViewBag.SeoKeywords = okullar[0].DilOkulu_Ulkeler.Seo_Keywords;
                            ViewBag.Description = okullar[0].DilOkulu_Ulkeler.Seo_Descriptions;

                            ViewBag.VizeBilgileri = okullar[0].DilOkulu_Ulkeler.VizeBilgileri;
                            ViewBag.CalismaIzni = okullar[0].DilOkulu_Ulkeler.CalismaIzni;
                            ViewBag.YasamGiderleri = okullar[0].DilOkulu_Ulkeler.YasamGiderleri;
                            ViewBag.Ulasim = okullar[0].DilOkulu_Ulkeler.Ulasim;
                            ViewBag.SaglikSistemi = okullar[0].DilOkulu_Ulkeler.SaglikSistemi;

                            ViewBag.ListeTipi = "Ulke";
                        }
                        break;
                    }
                }
            }

            return View(okullar);
        }

        public ActionResult MerkezDetay(string url = "")
        {
            DilOkulu_Merkez merkez = null;
            if (!string.IsNullOrEmpty(url))
            {
                merkezRepository = new MerkezRepository();
                merkez = merkezRepository.Detay(url, new int[] { (int)GeneralVariables.Durum.Aktif });
            }

            ///Benzer okulları getir (Şehire göre)
            ///
            if (merkez != null)
            {
                okulRepository = new OkulRepository();
                var randomOkulDetay = okulRepository.Liste()
                    .Where
                    (
                    o =>
                        o.MerkezId == merkez.Id &&
                        o.Durumu == (int)GeneralVariables.Durum.Aktif &&
                        o.GaleriId > 0
                    ).Select(o =>
                        new
                        {
                            o.GaleriId,
                            o.VideoUrl
                        }
                        ).OrderBy(o => Guid.NewGuid()).ToList()
                    .Take(1);
                if (randomOkulDetay != null && randomOkulDetay.Count() > 0)
                {
                    ViewBag.EmbedVideo = randomOkulDetay.FirstOrDefault().VideoUrl;
                    ViewBag.GaleriId = randomOkulDetay.FirstOrDefault().GaleriId;
                }
                else
                {
                    ViewBag.EmbedVideo = "";
                    ViewBag.GaleriId = 0;
                }

            }

            return View(merkez);
        }

        [HttpPost]
        public ActionResult Arama(FormCollection fColl)
        {
            var dil = fColl["Diller"];
            var ulke = fColl["Ulkeler"];
            var sehir = fColl["Sehirler"];

            dil = (dil == "Dil Seçin" ? "" : Tools.ReplaceTitle(dil));
            ulke = (ulke == "Ülke Seçin" ? "" : Tools.ReplaceTitle(ulke));
            sehir = (sehir == "Şehir Seçin" ? "" : Tools.ReplaceTitle(sehir));

            if (dil != "" && ulke == "" && sehir == "")
            {
                return RedirectToAction("dil", "okullar", new { url = dil });
            }
            else if (ulke != "" && dil != "" && sehir == "")
            {
                return RedirectToAction("UlkeSehir", "okullar", new { url = ulke });
            }
            else if (sehir != "" && ulke != "" && dil != "")
            {
                return RedirectToAction("UlkeSehir", "okullar", new { url = sehir });
            }

            return View();
        }
    }
}
