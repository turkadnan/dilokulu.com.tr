using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class OkulFiyatlariController : Controller
    {
        //
        // GET: /cms/OkulUcretleri/

        #region Degiskenler
        private IOkulRepository okulRepository = null;
        private IKursKategoriRepository kursKategoriRepository = null;
        private IKursTipiRepository kursTipiRepository = null;
        private IKursTipiFiyatlandirmalariRepository kursTipiFiyatlandirmalariRepository = null;
        private IKursTipiPromosyonTanimlariRepository kursTipiPromosyonTanimlariRepository = null;
        private IKursTipiEkUcretleriRepository kursTipiEkUcretleriRepository = null;
        private IMerkezRepository merkezRepository = null;
        private IKursTipiKargoHariclerRepository kursTipiKargoHariclerRepository = null;
        private GenericRepository<DilOkulu_Okullar> genericOkul = null;
        private GenericRepository<DilOkulu_KursTipleri> genericKursTipleri = null;
        private GenericRepository<DilOkulu_KursTipiFiyatlandirmalari> genericKursTipiFiyatlandirmalari = null;
        private GenericRepository<DilOkulu_KursTipiPromosyonTanimlari> genericKursTipiPromosyonTanimlari = null;
        private GenericRepository<DilOkulu_KursTipiEkUcretleri> genericKursTipiEkUcretleri = null;
        private GenericRepository<DilOkulu_KursTipiKargoHaricler> genericKursTipiKargoHaricler = null;
        private DilOkulu_Okullar okul = null;
        private List<DilOkulu_Okullar> okullar = null;
        #endregion

        #region KursTipleri
        public ActionResult KursTipi(int okulId = 0)
        {
            var okulDilId = OkulDilId(okulId);
            if (okulDilId > 0)
            {
                kursKategoriRepository = new KursKategoriRepository();
                var kursKategorileri = kursKategoriRepository.Liste()
                    .Where(kk => kk.DilId == okulDilId && kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

                ViewBag.KursKategorileri = kursKategorileri;
            }

            ViewBag.OkulId = okulId.ToString();

            kursTipiRepository = new KursTipiRepository();
            var kursTipleri = kursTipiRepository.Liste().Where(kt => kt.OkulId == okulId && kt.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kt => kt.Baslik).ToList();

            return View(kursTipleri);
        }


        public ActionResult KursTipiEdit(int id = 0, int okulId = 0)
        {
            #region Kurs Kategorileri
            var okulDilId = OkulDilId(okulId);
            if (okulDilId > 0)
            {
                kursKategoriRepository = new KursKategoriRepository();
                var kursKategorileri = kursKategoriRepository.Liste()
                    .Where(kk => kk.DilId == okulDilId && kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

                ViewBag.KursKategorileri = kursKategorileri;
            }
            #endregion

            ViewBag.OkulId = okulId.ToString();

            kursTipiRepository = new KursTipiRepository();
            var kursTipi = kursTipiRepository.Detay(id, new int[] { (byte)GeneralVariables.Durum.Aktif });

            return View(kursTipi);
        }

        [HttpPost]
        public ActionResult KursTipiEdit(FormCollection fColl, int id = 0, int okulId = 0)
        {
            #region Kurs Kategorileri
            var okulDilId = OkulDilId(okulId);
            if (okulDilId > 0)
            {
                kursKategoriRepository = new KursKategoriRepository();
                var kursKategorileri = kursKategoriRepository.Liste()
                    .Where(kk => kk.DilId == okulDilId && kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

                ViewBag.KursKategorileri = kursKategorileri;
            }
            #endregion

            #region Variables
            var kursKategoriId = Convert.ToInt32("0" + fColl["kursKategorisi"]);
            var baslik = fColl["baslik"];
            var aciklama = HttpUtility.UrlDecode(fColl["kursTipiAciklama"]);
            var kursTipiTarih = fColl["kursTipiTarih"];
            var ozelTarihListesi = Request["ozelTarihListesi"].TrimEnd('|');
            bool basTarihiHerPazartesi = false;
            bool basTarihiOzel = false;
            #endregion

            ViewBag.OkulId = okulId.ToString();

            kursTipiRepository = new KursTipiRepository();
            var kursTipi = kursTipiRepository.Detay(id, new int[] { (byte)GeneralVariables.Durum.Aktif });
            if (kursTipi != null)
            {
                kursTipi.KursKategoriId = kursKategoriId;
                kursTipi.Baslik = baslik;
                kursTipi.Aciklama = aciklama;
                if (!string.IsNullOrEmpty(kursTipiTarih))
                {
                    if (kursTipiTarih == "ozel")
                    {
                        basTarihiOzel = true;
                        if (!string.IsNullOrEmpty(ozelTarihListesi))
                        {
                            kursTipi.BasTarihiHerPazartesi = false;
                            kursTipi.BasTarihiOzel = true;
                            kursTipi.OzelTarihListesi = ozelTarihListesi;
                        }

                    }
                    else
                    {
                        basTarihiHerPazartesi = true;
                        kursTipi.BasTarihiHerPazartesi = true;
                        kursTipi.BasTarihiOzel = false;
                        kursTipi.OzelTarihListesi = string.Empty;
                    }
                }

                genericKursTipleri = new GenericRepository<DilOkulu_KursTipleri>(kursTipiRepository.DBContext);
                var retVal = genericKursTipleri.Update(kursTipi);
                if (retVal != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "error";
                }

            }
            return View(kursTipi);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiEkle()
        {
            #region Variables
            string status = "error";
            string message = "";
            var okulId = Convert.ToInt32("0" + Request["okulId"]);
            var baslik = Request["baslik"];
            var kursKategoriId = Convert.ToInt32("0" + Request["kursKategoriId"]);
            var basTarihiStatus = Request["basTarihiStatus"];
            var ozelTarihListesi = Request["ozelTarihListesi"].TrimEnd('|');
            var aciklama = HttpUtility.UrlDecode(Request["aciklama"]);

            bool basTarihiHerPazartesi = false;
            bool basTarihiOzel = false;

            if (basTarihiStatus == "ozel")
            {
                basTarihiOzel = true;
            }
            else
            {
                basTarihiHerPazartesi = true;
            }
            #endregion

            if (okulId > 0 && !string.IsNullOrEmpty(baslik) && kursKategoriId > 0)
            {
                kursTipiRepository = new KursTipiRepository();
                DilOkulu_KursTipleri yeniTip = new DilOkulu_KursTipleri()
                {
                    OkulId = okulId,
                    Baslik = baslik,
                    KursKategoriId = kursKategoriId,
                    Aciklama = aciklama,
                    KayitTarihi = DateTime.Now,
                    BasTarihiHerPazartesi = basTarihiHerPazartesi,
                    BasTarihiOzel = basTarihiOzel,
                    OzelTarihListesi = ozelTarihListesi,
                    Durumu = (byte)GeneralVariables.Durum.Aktif
                };

                genericKursTipleri = new GenericRepository<DilOkulu_KursTipleri>();
                var retVal = genericKursTipleri.Insert(yeniTip);
                if (retVal != null)
                {
                    status = "ok";
                }
                else
                {
                    message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult KursTipiSil(int id = 0)
        {
            string status = "error";
            string message = "Hata:Silme işlemi yapılamadı!";

            if (id > 0)
            {
                kursTipiRepository = new KursTipiRepository();
                var kursTipi = kursTipiRepository.Liste().Where(kt => kt.Id == id).FirstOrDefault(); ;
                //&& kt.Durumu != (byte)GeneralVariables.Durum.Silindi
                if (kursTipi != null)
                {
                    genericKursTipleri = new GenericRepository<DilOkulu_KursTipleri>(kursTipiRepository.DBContext);
                    var retVal = genericKursTipleri.Delete(kursTipi);
                    if (retVal > 0)
                    {
                        status = "ok";
                        message = "Kayıt silindi!";
                    }
                    else
                    {
                        message = "Hata:Silme işlemi yapılamadı!";
                    }

                }

            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fiyat Tanimlari
        public ActionResult FiyatTanimlari(int okulId = 0, int kursTipiId = 0)
        {
            if (okulId > 0)
            {
                ViewBag.KursTipleri = KursTipleriniGetir(okulId);
                if (kursTipiId > 0)
                    ViewBag.KursTipiId = kursTipiId;

            }
            else
            {
                return RedirectToAction("index", "okullar");
            }
            return View();
        }

        /// <summary>
        /// Ajax Partial View Viewer 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult FiyatTanimlari_HaftaTanimlari(byte? fiyatlandirmaTipiId, int kursTipiId = 0)
        {
            string partialViewName = "";
            List<DilOkulu_KursTipiFiyatlandirmalari> kursTipiFiyatlari = null;
            if (kursTipiId > 0)
            {
                kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();
                kursTipiFiyatlari = kursTipiFiyatlandirmalariRepository.Liste()
                    .Where(f =>
                        f.FiyatlandirmaModelId == fiyatlandirmaTipiId &&
                        f.KursTipiId == kursTipiId &&
                        f.Durumu == (byte)GeneralVariables.Durum.Aktif
                        )
                    .ToList();
            }

            switch (fiyatlandirmaTipiId)
            {
                case 1:
                    //Standart Hafta & Tek Fiyat
                    partialViewName = "_FiyatTanimi_StandartHafta_Tanim";
                    break;
                case 2:
                    //Her Haftaya Özel Fiyat
                    partialViewName = "_FiyatTanimi_HaftayaOzel_Tanim";
                    break;
                case 3:
                    //Hafta Bazlı & Birim Fiyatlı
                    partialViewName = "_FiyatTanimi_HaftaBazli_Tanim";
                    break;
                default:
                    partialViewName = "";
                    break;
            }
            return PartialView(partialViewName, kursTipiFiyatlari);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiFiyatiEkle()
        {
            #region Variables
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
            var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
            var fiyatlandirmaModelId = Convert.ToByte("0" + Request["fiyatlandirmaTipiId"]);
            var hafta = Convert.ToInt32("0" + Request["hafta"]);
            var haftaBaslangic = Convert.ToInt32("0" + Request["haftaBaslangic"]);
            var haftaBitis = Convert.ToInt32("0" + Request["haftaBitis"]);
            var fiyat = Convert.ToDecimal("0" + Request["fiyat"]);
            #endregion

            if (kursTipiId > 0 && fiyatlandirmaModelId > 0 && fiyat > 0)
            {
                kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();

                genericKursTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KursTipiFiyatlandirmalari>(kursTipiFiyatlandirmalariRepository.DBContext);


                //Yeni ekleme
                DilOkulu_KursTipiFiyatlandirmalari yeniFiyat = new DilOkulu_KursTipiFiyatlandirmalari();

                yeniFiyat.KursTipiId = kursTipiId;
                yeniFiyat.FiyatlandirmaModelId = fiyatlandirmaModelId;

                switch (fiyatlandirmaModelId)
                {
                    case 1:
                        //Standart Hafta & Tek Fiyat
                        yeniFiyat.StandartHafta_HaftaSayisi = hafta;
                        break;
                    case 2:
                        //Hafta Bazlı & Birim Fiyatlı


                        #region  Aynı haftaya ait kayıt giriliyorsa, kayıt önce siliniyor!
                        var mevcutFiyatlandirmaTanim = kursTipiFiyatlandirmalariRepository.Liste().Where(f =>
                            f.KursTipiId == kursTipiId &&
                            f.FiyatlandirmaModelId == fiyatlandirmaModelId &&
                            f.HerHaftayaOzel_Hafta == hafta
                            ).FirstOrDefault();
                        if (mevcutFiyatlandirmaTanim != null)
                        {
                            genericKursTipiFiyatlandirmalari.Delete(mevcutFiyatlandirmaTanim);
                        }
                        #endregion

                        yeniFiyat.HerHaftayaOzel_Hafta = hafta;
                        break;
                    case 3:
                        //Her Haftaya Özel Fiyat
                        yeniFiyat.HaftaBazliBirimFiyatli_HaftaBaslangic = haftaBaslangic;
                        yeniFiyat.HaftaBazliBirimFiyatli_HaftaBitis = haftaBitis;
                        break;
                }

                yeniFiyat.Fiyat = fiyat;
                yeniFiyat.Durumu = (byte)GeneralVariables.Durum.Aktif;
                yeniFiyat.KayitTarihi = DateTime.Now;


                var retVal = genericKursTipiFiyatlandirmalari.Insert(yeniFiyat);
                if (retVal != null)
                {
                    status = "ok";
                }
                else
                {
                    message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Update
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiFiyatiGuncelle()
        {
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";

            try
            {

                #region Variables

                var id = Convert.ToInt32("0" + Request["id"]);
                var fiyatlandirmaModelId = Convert.ToByte("0" + Request["fiyatlandirmaTipiId"]);
                var hafta = Convert.ToInt32("0" + Request["hafta"]);

                var haftaBaslangic = Convert.ToInt32("0" + Request["haftaBaslangic"]);
                var haftaBitis = Convert.ToInt32("0" + Request["haftaBitis"]);

                var fiyat = Convert.ToDecimal("0" + Request["fiyat"]);
                #endregion

                #region Process
                if (id > 0 && fiyatlandirmaModelId > 0 && fiyat > 0)
                {
                    kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();

                    genericKursTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KursTipiFiyatlandirmalari>(kursTipiFiyatlandirmalariRepository.DBContext);


                    //Fiyat Getir
                    DilOkulu_KursTipiFiyatlandirmalari fiyatDetay = kursTipiFiyatlandirmalariRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });

                    switch (fiyatlandirmaModelId)
                    {
                        case 1:
                            //Standart Hafta & Tek Fiyat
                            fiyatDetay.StandartHafta_HaftaSayisi = hafta;
                            break;
                        case 3:
                            //Her Haftaya Özel Fiyat
                            fiyatDetay.HaftaBazliBirimFiyatli_HaftaBaslangic = haftaBaslangic;
                            fiyatDetay.HaftaBazliBirimFiyatli_HaftaBitis = haftaBitis;
                            break;
                    }

                    fiyatDetay.Fiyat = fiyat;


                    var retVal = genericKursTipiFiyatlandirmalari.Update(fiyatDetay);
                    if (retVal != null)
                    {
                        status = "ok";
                    }
                    else
                    {
                        message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Delete
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiFiyatSil()
        {
            #region Variables
            string status = "error";
            string message = "İşlem sırasında problem ile karşılaşıldı!";
            var kursTipiFiyatId = Convert.ToInt32("0" + Request["kursTipiFiyatId"]);
            #endregion

            if (kursTipiFiyatId > 0)
            {
                kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();

                genericKursTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KursTipiFiyatlandirmalari>(kursTipiFiyatlandirmalariRepository.DBContext);

                var fiyat = kursTipiFiyatlandirmalariRepository.Detay(kursTipiFiyatId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (fiyat != null)
                {
                    var retVal = genericKursTipiFiyatlandirmalari.Delete(fiyat);
                    if (retVal > 0)
                    {
                        status = "ok";
                    }
                    else
                    {
                        message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                    }
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Delete
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiFiyatAllDelete()
        {
            #region Variables
            string status = "error";
            string message = "İşlem sırasında problem ile karşılaşıldı!";
            var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
            #endregion

            if (kursTipiId > 0)
            {
                kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();

                genericKursTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KursTipiFiyatlandirmalari>(kursTipiFiyatlandirmalariRepository.DBContext);

                List<DilOkulu_KursTipiFiyatlandirmalari> fiyatlar = kursTipiFiyatlandirmalariRepository.Liste().Where(l=>l.KursTipiId == kursTipiId).ToList();
                if (fiyatlar != null)
                {
                    var retVal = genericKursTipiFiyatlandirmalari.Delete(fiyatlar);

                    if (retVal > 0)
                    {
                        status = "ok";
                    }
                    else
                    {
                        message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                    }
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tanımlı Fiyat modelini bulan metod
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiFiyatModeliBul()
        {
            #region Variables
            string status = "error";
            var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
            var fiyatlandirmaModelId = 0;
            #endregion

            kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository();
            var FiyatModeller = kursTipiFiyatlandirmalariRepository.Liste().Where(pt => pt.KursTipiId == kursTipiId).Select(pt => pt.FiyatlandirmaModelId).ToList();
            if (FiyatModeller != null && FiyatModeller.Count > 0)
            {
                fiyatlandirmaModelId = FiyatModeller[0];
                status = "ok";
            }

            return Json(new { fiyatlandirmaModelId = fiyatlandirmaModelId, status = status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Promosyon Tanımları
        public ActionResult PromosyonTanimlari(int okulId = 0, int kursTipiId = 0)
        {
            if (okulId > 0)
            {
                ViewBag.KursTipleri = KursTipleriniGetir(okulId);
                if (kursTipiId > 0)
                    ViewBag.KursTipiId = kursTipiId;

            }
            else
            {
                return RedirectToAction("index", "okullar");
            }
            return View();
        }

        /// <summary>
        /// Ajax Partial View Viewer 
        /// </summary>
        /// <returns></returns>
        public PartialViewResult PromosyonTanimlari_PromosyonTanimlari(byte? promosyonTipiId, int kursTipiId = 0)
        {
            string partialViewName = "";
            List<DilOkulu_KursTipiPromosyonTanimlari> kursTipiPromosyonlari = null;
            if (kursTipiId > 0)
            {
                kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();
                kursTipiPromosyonlari = kursTipiPromosyonTanimlariRepository.Liste()
                    .Where(f =>
                        f.PromosyonModelId == promosyonTipiId &&
                        f.KursTipiId == kursTipiId &&
                        f.Durumu == (byte)GeneralVariables.Durum.Aktif
                        )
                    .ToList();
            }

            switch (promosyonTipiId)
            {
                case 1:
                    //Ekstra Hafta Promosyonu
                    partialViewName = "_PromosyonTanimi_ExtraHaftaPromosyonu";
                    break;
                case 2:
                    //Yüzdesel İndirim
                    partialViewName = "_PromosyonTanimi_IndirimPromosyonu";
                    break;
                case 3:
                    //Fiyat Bazlı İndirim
                    partialViewName = "_PromosyonTanimi_KursBazliPromosyon";
                    break;
                case 4:
                    //Kurs Bazlı Promosyon
                    partialViewName = "_PromosyonTanimi_IndirimliBirimFiyatPromosyonu";
                    break;
                default:
                    partialViewName = "";
                    break;
            }
            return PartialView(partialViewName, kursTipiPromosyonlari);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiPromosyonEkle()
        {
            #region Variables
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
            var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
            var promosyonModelId = Convert.ToByte("0" + Request["promosyonTipiId"]);

            var extraHafta_MinimumHafta = Convert.ToInt32("0" + Request["extraHafta_MinimumHafta"]);
            var extraHafta_UcretsizHaftaSayisi = Convert.ToInt32("0" + Request["extraHafta_UcretsizHaftaSayisi"]);

            var indirimPromosyonu_Hafta = Convert.ToInt32("0" + Request["indirimPromosyonu_Hafta"]);
            var indirimPromosyonu_HaftaMax = Convert.ToInt32("0" + Request["indirimPromosyonu_HaftaMax"]);
            var indirimPromosyonu_Oran = Convert.ToInt32("0" + Request["indirimPromosyonu_Oran"]);
            var indirimPromosyonu_BirimFiyat = Convert.ToInt32("0" + Request["indirimPromosyonu_BirimFiyat"]);

            var kursBazli_Hafta = Convert.ToInt32("0" + Request["kursBazli_Hafta"]);
            var KursBazli_Fiyat = Convert.ToInt32("0" + Request["KursBazli_Fiyat"]);

            var promosyonBitisTarihi = Convert.ToDateTime(Request["promosyonBitisTarihi"]);

            string _kursTarihiBaslangic = Request["kursTarihiBaslangic"];
            string _kursTarihiBitis = Request["kursTarihiBitis"];

            DateTime? kursTarihiBaslangic = null;
            DateTime? kursTarihiBitis = null;

            if (Tools.IsDate(_kursTarihiBaslangic))
                kursTarihiBaslangic = Convert.ToDateTime(_kursTarihiBaslangic);

            if (Tools.IsDate(_kursTarihiBitis))
                kursTarihiBitis = Convert.ToDateTime(_kursTarihiBitis);

            #endregion

            if (kursTipiId > 0 && promosyonModelId > 0)
            {
                kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();

                genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>();

                //Yeni ekleme
                DilOkulu_KursTipiPromosyonTanimlari yeniPromosyon = new DilOkulu_KursTipiPromosyonTanimlari();

                yeniPromosyon.KursTipiId = kursTipiId;
                yeniPromosyon.PromosyonModelId = promosyonModelId;
                yeniPromosyon.IndirimPromosyonu_BirimFiyat = 0;
                yeniPromosyon.IndirimPromosyonu_Oran = 0;

                switch (promosyonModelId)
                {
                    case 1:
                        //Ekstra Hafta Promosyonu
                        yeniPromosyon.ExtraHafta_MinimumHafta = extraHafta_MinimumHafta;
                        yeniPromosyon.ExtraHafta_UcretsizHaftaSayisi = extraHafta_UcretsizHaftaSayisi;
                        break;
                    case 2:
                        //Yüzdeli İndirim Promosyonu
                        yeniPromosyon.IndirimPromosyonu_Hafta = indirimPromosyonu_Hafta;
                        yeniPromosyon.IndirimPromosyonu_HaftaMax = indirimPromosyonu_HaftaMax;
                        yeniPromosyon.IndirimPromosyonu_Oran = indirimPromosyonu_Oran;
                        break;
                    case 3:
                        //Kurs Bazlı Promosyon
                        yeniPromosyon.KursBazli_Hafta = kursBazli_Hafta;
                        yeniPromosyon.KursBazli_Fiyat = KursBazli_Fiyat;
                        break;
                    case 4:
                        //İndirimli Birim Fiyat
                        yeniPromosyon.IndirimPromosyonu_Hafta = indirimPromosyonu_Hafta;
                        yeniPromosyon.IndirimPromosyonu_HaftaMax = indirimPromosyonu_HaftaMax;
                        yeniPromosyon.IndirimPromosyonu_BirimFiyat = indirimPromosyonu_BirimFiyat;

                        break;
                }

                yeniPromosyon.PromosyonBitisTarihi = promosyonBitisTarihi;
                yeniPromosyon.KursTarihiBaslangic = kursTarihiBaslangic;
                yeniPromosyon.KursTarihiBitis = kursTarihiBitis;
                yeniPromosyon.Durumu = (byte)GeneralVariables.Durum.Aktif;
                yeniPromosyon.KayitTarihi = DateTime.Now;


                var retVal = genericKursTipiPromosyonTanimlari.Insert(yeniPromosyon);
                if (retVal != null)
                {
                    status = "ok";
                }
                else
                {
                    message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Update
        /// </summary>
        /// <returns></returns>
        public JsonResult PromosyonTipiFiyatiGuncelle()
        {
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
            try
            {

                #region Variables

                var id = Convert.ToInt32("0" + Request["id"]);
                var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
                var promosyonModelId = Convert.ToByte("0" + Request["promosyonTipiId"]);

                var extraHafta_MinimumHafta = Convert.ToInt32("0" + Request["extraHafta_MinimumHafta"]);
                var extraHafta_UcretsizHaftaSayisi = Convert.ToInt32("0" + Request["extraHafta_UcretsizHaftaSayisi"]);

                var indirimPromosyonu_Hafta = Convert.ToInt32("0" + Request["indirimPromosyonu_Hafta"]);
                var indirimPromosyonu_HaftaMax = Convert.ToInt32("0" + Request["indirimPromosyonu_HaftaMax"]);
                var indirimPromosyonu_Oran = Convert.ToInt32("0" + Request["indirimPromosyonu_Oran"]);
                var indirimPromosyonu_BirimFiyat = Convert.ToInt32("0" + Request["indirimPromosyonu_BirimFiyat"]);

                var kursBazli_Hafta = Convert.ToInt32("0" + Request["kursBazli_Hafta"]);
                var KursBazli_Fiyat = Convert.ToInt32("0" + Request["KursBazli_Fiyat"]);

                var promosyonBitisTarihi = Convert.ToDateTime(Request["promosyonBitisTarihi"]);

                string _kursTarihiBaslangic = Request["kursTarihiBaslangic"];
                string _kursTarihiBitis = Request["kursTarihiBitis"];

                DateTime? kursTarihiBaslangic = null;
                DateTime? kursTarihiBitis = null;

                if (Tools.IsDate(_kursTarihiBaslangic))
                    kursTarihiBaslangic = Convert.ToDateTime(_kursTarihiBaslangic);

                if (Tools.IsDate(_kursTarihiBitis))
                    kursTarihiBitis = Convert.ToDateTime(_kursTarihiBitis);

                #endregion

                #region Process
                if (id > 0 && promosyonModelId > 0)
                {
                    kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();

                    genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>(kursTipiPromosyonTanimlariRepository.DBContext);


                    //Promosyon Getir
                    DilOkulu_KursTipiPromosyonTanimlari promosyonDetay = kursTipiPromosyonTanimlariRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });


                    switch (promosyonModelId)
                    {
                        case 1:
                            //Ekstra Hafta Promosyonu
                            promosyonDetay.ExtraHafta_MinimumHafta = extraHafta_MinimumHafta;
                            promosyonDetay.ExtraHafta_UcretsizHaftaSayisi = extraHafta_UcretsizHaftaSayisi;
                            break;
                        case 2:
                            //Yüzdeli İndirim Promosyonu
                            promosyonDetay.IndirimPromosyonu_Hafta = indirimPromosyonu_Hafta;
                            promosyonDetay.IndirimPromosyonu_HaftaMax = indirimPromosyonu_HaftaMax;
                            promosyonDetay.IndirimPromosyonu_Oran = indirimPromosyonu_Oran;
                            break;
                        case 3:
                            //Fiyat Bazlı İndirim  (Eski Değer : Kurs Bazlı Promosyon)
                            promosyonDetay.KursBazli_Hafta = kursBazli_Hafta;
                            promosyonDetay.KursBazli_Fiyat = KursBazli_Fiyat;
                            break;
                        case 4:
                            //İndirimli Birim Fiyat
                            promosyonDetay.IndirimPromosyonu_Hafta = indirimPromosyonu_Hafta;
                            promosyonDetay.IndirimPromosyonu_HaftaMax = indirimPromosyonu_HaftaMax;
                            promosyonDetay.IndirimPromosyonu_BirimFiyat = indirimPromosyonu_BirimFiyat;

                            break;
                    }

                    promosyonDetay.PromosyonBitisTarihi = promosyonBitisTarihi;
                    promosyonDetay.KursTarihiBaslangic = kursTarihiBaslangic;
                    promosyonDetay.KursTarihiBitis = kursTarihiBitis;


                    var retVal = genericKursTipiPromosyonTanimlari.Update(promosyonDetay);
                  if (retVal != null)
                  {
                      status = "ok";
                  }
                  else
                  {
                      message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                  }
                  
                }
                #endregion
            }
            catch (Exception)
            {
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Ajax Delete
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiPromosyonSil()
        {
            #region Variables
            string status = "error";
            string message = "İşlem sırasında problem ile karşılaşıldı!";
            var kursTipiFiyatId = Convert.ToInt32("0" + Request["kursTipiFiyatId"]);
            #endregion

            if (kursTipiFiyatId > 0)
            {
                kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();

                genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>(kursTipiPromosyonTanimlariRepository.DBContext);

                var promosyon = kursTipiPromosyonTanimlariRepository.Detay(kursTipiFiyatId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (promosyon != null)
                {
                    var retVal = genericKursTipiPromosyonTanimlari.Delete(promosyon);
                    if (retVal > 0)
                    {
                        status = "ok";
                    }
                    else
                    {
                        message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                    }
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Tanımlı Promosyon modelini bulan metod
        /// </summary>
        /// <returns></returns>
        public JsonResult KursTipiPromosyonModeliBul()
        {
            #region Variables
            string status = "error";
            var kursTipiId = Convert.ToInt32("0" + Request["kursTipiId"]);
            var promosyonTipiId = 0;
            #endregion

            kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();
            var PromosyonModeller = kursTipiPromosyonTanimlariRepository.Liste().Where(pt => pt.KursTipiId == kursTipiId).Select(pt => pt.PromosyonModelId).ToList();
            if (PromosyonModeller != null && PromosyonModeller.Count > 0)
            {
                promosyonTipiId = PromosyonModeller[0];
                status = "ok";
            }

            return Json(new { promosyonTipiId = promosyonTipiId, status = status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Okul Ek Ücretler
        public ActionResult OkulEkUcretler(int okulId = 0)
        {
            if (okulId > 0)
            {
                okulRepository = new OkulRepository();
                var okulDigerUcretleri = okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (okulDigerUcretleri != null)
                {
                    #region Kargo Hariçler için liste oluşturuluyor!
                    kursTipiRepository = new KursTipiRepository(okulRepository.DBContext);
                    var kursTipleri = kursTipiRepository.Liste().Where(kt => kt.OkulId == okulId).OrderBy(kt => kt.Baslik).ToList();


                    kursTipiKargoHariclerRepository = new KursTipiKargoHariclerRepository(okulRepository.DBContext);
                    var kargoHaricKurslar = kursTipiKargoHariclerRepository.Liste().Where(kl => kl.OkulId == okulId).ToList();

                    string kargoHariclerIcinKursTipleri = "[";

                    foreach (var item in kargoHaricKurslar)
                    {
                        kargoHariclerIcinKursTipleri += "{\"okulId\":\"" + item.OkulId + "\",\"kursTipiId\":\"" + item.KursTipiId + "\",\"selected\":true},";
                    }
                    kargoHariclerIcinKursTipleri = kargoHariclerIcinKursTipleri.TrimEnd(',');
                    kargoHariclerIcinKursTipleri += "]";
                    #endregion

                    ViewBag.KursTipleri = kursTipleri;
                    ViewBag.KargoHariclerIcinKursTipleri = kargoHariclerIcinKursTipleri;
                    ViewBag.OkulId = okulId.ToString();
                    return View(okulDigerUcretleri);
                }
                else
                {
                    return RedirectToAction("index", "okullar");
                }

            }
            else
            {
                return RedirectToAction("index", "okullar");
            }

        }

        [HttpPost]
        public ActionResult OkulEkUcretler(FormCollection fColl, int okulId = 0)
        {
            #region Form Collections
            decimal ucretler_OkulKayitUcreti = Convert.ToDecimal("0" + (Tools.IsDecimal(fColl["ucretler_OkulKayitUcreti"].ToString()) ? fColl["ucretler_OkulKayitUcreti"] : ""));
            string ucretler_OkulKayitUcretiArray = fColl["Ucretler_OkulKayitUcretiArray"].ToString();
            string ucretler_OkulKayitEkUcretArray = fColl["Ucretler_OkulKayitEkUcretArray"].ToString();

            decimal ucretler_KonaklamaAyarlamaUcreti = Convert.ToDecimal("0" + (Tools.IsDecimal(fColl["Ucretler_KonaklamaAyarlamaUcreti"].ToString()) ? fColl["Ucretler_KonaklamaAyarlamaUcreti"] : ""));
            string ucretler_KonaklamaAyarlamaUcretiArray = fColl["Ucretler_KonaklamaAyarlamaUcretiArray"].ToString();
            string ucretler_KonaklamaAyarlamaEkUcretArray = fColl["Ucretler_KonaklamaAyarlamaEkUcretArray"].ToString();

            decimal ucretler_MateryalUcreti = Convert.ToDecimal("0" + (fColl["Ucretler_MateryalUcreti"] != null && Tools.IsDecimal("0" + fColl["Ucretler_MateryalUcreti"].ToString()) ? fColl["Ucretler_MateryalUcreti"] : ""));
            string ucretler_MateryalTahsilatTipi = fColl["Ucretler_MateryalTahsilatTipi"].ToString();
            string ucretler_MateryalUcretiArray = fColl["Ucretler_MateryalUcretiArray"].ToString();

            decimal ucretler_KargoUcreti = Convert.ToDecimal("0" + (Tools.IsDecimal(fColl["Ucretler_KargoUcreti"].ToString()) ? fColl["Ucretler_KargoUcreti"] : ""));

            decimal ucretler_SaglikSigortasi = Convert.ToDecimal("0" + (fColl["Ucretler_SaglikSigortasi"] != null && Tools.IsDecimal(fColl["Ucretler_SaglikSigortasi"].ToString()) ? fColl["Ucretler_SaglikSigortasi"] : ""));
            string ucretler_SaglikSigortasiTahsilatTipi = fColl["Ucretler_SaglikSigortasiTahsilatTipi"].ToString();
            string ucretler_SaglikSigortasiArray = fColl["Ucretler_SaglikSigortasiArray"].ToString();


            string ucretler_HavaAlaniKarsilama = fColl["Ucretler_HavaAlaniKarsilama"].ToString();

            ucretler_HavaAlaniKarsilama = ucretler_HavaAlaniKarsilama.TrimEnd('|');
            ucretler_OkulKayitUcretiArray = ucretler_OkulKayitUcretiArray.TrimEnd('|');
            ucretler_OkulKayitEkUcretArray = ucretler_OkulKayitEkUcretArray.TrimEnd('|');
            ucretler_KonaklamaAyarlamaUcretiArray = ucretler_KonaklamaAyarlamaUcretiArray.TrimEnd('|');
            ucretler_KonaklamaAyarlamaEkUcretArray = ucretler_KonaklamaAyarlamaEkUcretArray.TrimEnd('|');
            ucretler_MateryalUcretiArray = ucretler_MateryalUcretiArray.TrimEnd('|');
            ucretler_SaglikSigortasiArray = ucretler_SaglikSigortasiArray.TrimEnd('|');

            if (ucretler_MateryalUcreti == 0 && ucretler_MateryalTahsilatTipi != "HaftaAralikli")
            {
                ucretler_MateryalTahsilatTipi = "TekSefer";
                ucretler_MateryalUcretiArray = "";
            }

            if (ucretler_MateryalTahsilatTipi == "HaftaAralikli")
            {

                if (!string.IsNullOrEmpty(ucretler_MateryalUcretiArray))
                {
                    ucretler_MateryalUcreti = 0;
                }
            }

            if (ucretler_SaglikSigortasi == 0 && ucretler_SaglikSigortasiTahsilatTipi != "HaftaAralikli")
            {
                ucretler_SaglikSigortasiTahsilatTipi = "TekSefer";
                ucretler_SaglikSigortasiArray = "";
            }


            if (ucretler_SaglikSigortasiTahsilatTipi == "HaftaAralikli")
            {

                if (!string.IsNullOrEmpty(ucretler_SaglikSigortasiArray))
                {
                    ucretler_SaglikSigortasi = 0;
                }
            }

            string Ucretler_KargoHariciKurslar = fColl["Ucretler_KargoHariciKurslar"].ToString();

            #endregion

            if (okulId > 0)
            {

                string kargoHariclerIcinKursTipleri = "[]";

                okulRepository = new OkulRepository();
                var okulDigerUcretleri = okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (okulDigerUcretleri != null)
                {
                    okulDigerUcretleri.Ucretler_OkulKayitUcreti = ucretler_OkulKayitUcreti;
                    okulDigerUcretleri.Ucretler_OkulKayitUcretiArray = ucretler_OkulKayitUcretiArray;
                    okulDigerUcretleri.Ucretler_OkulKayitEkUcretArray = ucretler_OkulKayitEkUcretArray;

                    okulDigerUcretleri.Ucretler_KonaklamaAyarlamaUcreti = ucretler_KonaklamaAyarlamaUcreti;
                    okulDigerUcretleri.Ucretler_KonaklamaAyarlamaUcretiArray = ucretler_KonaklamaAyarlamaUcretiArray;
                    okulDigerUcretleri.Ucretler_KonaklamaAyarlamaEkUcretArray = ucretler_KonaklamaAyarlamaEkUcretArray;

                    okulDigerUcretleri.Ucretler_KargoUcreti = ucretler_KargoUcreti;

                    #region Materyal Ücretleri
                    if (ucretler_MateryalTahsilatTipi == "HaftaAralikli" && string.IsNullOrEmpty(ucretler_MateryalUcretiArray))
                    {
                        okulDigerUcretleri.Ucretler_MateryalUcreti = okulDigerUcretleri.Ucretler_MateryalUcreti;
                        ucretler_MateryalTahsilatTipi = "TekSefer";
                    }
                    else
                    {
                        okulDigerUcretleri.Ucretler_MateryalUcreti = ucretler_MateryalUcreti;
                    }

                    okulDigerUcretleri.Ucretler_MateryalTahsilatTipi = ucretler_MateryalTahsilatTipi;
                    okulDigerUcretleri.Ucretler_MateryalUcretiArray = ucretler_MateryalUcretiArray;
                    #endregion

                    #region Sağlık Sigortası
                    if (ucretler_SaglikSigortasiTahsilatTipi == "HaftaAralikli" && string.IsNullOrEmpty(ucretler_SaglikSigortasiArray))
                    {
                        okulDigerUcretleri.Ucretler_SaglikSigortasi = okulDigerUcretleri.Ucretler_SaglikSigortasi;
                        ucretler_SaglikSigortasiTahsilatTipi = "TekSefer";
                    }
                    else
                    {
                        okulDigerUcretleri.Ucretler_SaglikSigortasi = ucretler_SaglikSigortasi;
                    }

                    okulDigerUcretleri.Ucretler_SaglikSigortasiTahsilatTipi = ucretler_SaglikSigortasiTahsilatTipi;
                    okulDigerUcretleri.Ucretler_SaglikSigortasiArray = ucretler_SaglikSigortasiArray;
                    #endregion

                    okulDigerUcretleri.Ucretler_HavaAlaniKarsilama = ucretler_HavaAlaniKarsilama;

                    genericOkul = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);

                    var retVal = genericOkul.Update(okulDigerUcretleri);
                    if (retVal != null)
                    {

                        #region Kargo Uygulanmayacak Kurs Tipleri

                        kursTipiKargoHariclerRepository = new KursTipiKargoHariclerRepository(okulRepository.DBContext);
                        genericKursTipiKargoHaricler = new GenericRepository<DilOkulu_KursTipiKargoHaricler>(okulRepository.DBContext);


                        JObject kargoHaricJson = JObject.Parse("{\"lst\":" + Ucretler_KargoHariciKurslar + "}");
                        JArray arrayKargoHaricListe = (JArray)kargoHaricJson["lst"];
                        foreach (var item in arrayKargoHaricListe)
                        {
                            var jsonOkulId = Convert.ToInt32(item["okulId"].ToString());
                            var jsonKursTipiId = Convert.ToInt32(item["kursTipiId"].ToString());
                            var jsonSelected = item["selected"].ToString();


                            var kargoHaricKurs = kursTipiKargoHariclerRepository.Liste().Where(kl => kl.OkulId == jsonOkulId && kl.KursTipiId == jsonKursTipiId).FirstOrDefault();

                            if (kargoHaricKurs == null && jsonSelected == "True")
                            {
                                DilOkulu_KursTipiKargoHaricler newKargoHaric = new DilOkulu_KursTipiKargoHaricler()
                                {
                                    OkulId = jsonOkulId,
                                    KursTipiId = jsonKursTipiId
                                };

                                genericKursTipiKargoHaricler.Insert(newKargoHaric);
                            }
                            else if (kargoHaricKurs != null && jsonSelected == "False")
                            {
                                genericKursTipiKargoHaricler.Delete(kargoHaricKurs);
                            }
                        }
                        #endregion

                        #region Kargo Hariçler için liste oluşturuluyor!
                        kursTipiRepository = new KursTipiRepository(okulRepository.DBContext);
                        var kursTipleri = kursTipiRepository.Liste().Where(kt => kt.OkulId == okulId).OrderBy(kt => kt.Baslik).ToList();

                        var kargoHaricKurslar = kursTipiKargoHariclerRepository.Liste().Where(kl => kl.OkulId == okulId).ToList();

                        kargoHariclerIcinKursTipleri = "[";

                        foreach (var item in kargoHaricKurslar)
                        {
                            kargoHariclerIcinKursTipleri += "{\"okulId\":\"" + item.OkulId + "\",\"kursTipiId\":\"" + item.KursTipiId + "\",\"selected\":true},";
                        }
                        kargoHariclerIcinKursTipleri = kargoHariclerIcinKursTipleri.TrimEnd(',');
                        kargoHariclerIcinKursTipleri += "]";
                        #endregion

                        ViewBag.KursTipleri = kursTipleri;
                        ViewBag.Status = "ok";
                    }
                    else
                    {
                        ViewBag.Status = "err";
                    }


                    ViewBag.KargoHariclerIcinKursTipleri = kargoHariclerIcinKursTipleri;
                    ViewBag.OkulId = okulId.ToString();
                    return View(okulDigerUcretleri);
                }
                else
                {
                    return RedirectToAction("index", "okullar");
                }

            }
            else
            {
                return RedirectToAction("index", "okullar");
            }

        }
        #endregion

        #region SummerSupplement
        public ActionResult SummerSupplement(int okulId = 0, int kursTipiId = 0)
        {
            List<DilOkulu_KursTipiEkUcretleri> ekUcretler = null;

            if (okulId > 0)
            {
                ViewBag.KursTipleri = KursTipleriniGetir(okulId);
                if (kursTipiId > 0)
                {
                    ViewBag.kursTipiId = kursTipiId;
                    ekUcretler = KursEkUcretleriGetir(kursTipiId);
                }

                ViewBag.OkulId = okulId.ToString();

            }
            else
            {
                return RedirectToAction("index", "okullar");
            }
            return View(ekUcretler);
        }

        [HttpPost]
        public ActionResult SummerSupplement(FormCollection fColl, int okulId = 0, int kursTipiId = 0)
        {
            #region Form Collection
            int kursTipId = Convert.ToInt32("0" + fColl["kursTipi"]);
            string _baslangicTarihi = fColl["BaslangicTarihi"];
            string _bitisTarihi = fColl["BititisTarihi"];
            string _fiyat = fColl["Fiyat"];
            int maksimumHafta = Convert.ToInt32("0" + fColl["MaksimunHafta"]);

            DateTime baslangicTarihi = Tools.IsDate(_baslangicTarihi) ? Convert.ToDateTime(_baslangicTarihi) : new DateTime();
            DateTime bitisTarihi = Tools.IsDate(_bitisTarihi) ? Convert.ToDateTime(_bitisTarihi) : new DateTime();
            decimal fiyat = Tools.IsDecimal(_fiyat) ? Convert.ToDecimal(_fiyat) : 0;

            #endregion

            List<DilOkulu_KursTipiEkUcretleri> ekUcretler = null;

            if (okulId > 0)
            {
                ViewBag.KursTipleri = KursTipleriniGetir(okulId);
                if (kursTipiId > 0)
                {
                    ViewBag.kursTipiId = kursTipiId;
                    ekUcretler = KursEkUcretleriGetir(kursTipiId);
                }

                ViewBag.OkulId = okulId.ToString();

                //Kayıt işlemi
                if (kursTipiId > 0 && baslangicTarihi.Year > 2000 && bitisTarihi.Year > 200 && fiyat > 0)
                {

                    kursTipiEkUcretleriRepository = new KursTipiEkUcretleriRepository();
                    DilOkulu_KursTipiEkUcretleri yeniUcret = new DilOkulu_KursTipiEkUcretleri()
                    {
                        KursTipId = kursTipiId,
                        BaslangicTarihi = baslangicTarihi,
                        BititisTarihi = bitisTarihi,
                        MaksimumHafta = maksimumHafta,
                        Fiyat = fiyat,
                        KayitTarihi = DateTime.Now,
                        Durumu = 1
                    };

                    genericKursTipiEkUcretleri = new GenericRepository<DilOkulu_KursTipiEkUcretleri>(kursTipiEkUcretleriRepository.DBContext);

                    var retVal = genericKursTipiEkUcretleri.Insert(yeniUcret);
                    if (retVal != null)
                    {
                        ekUcretler = KursEkUcretleriGetir(kursTipiId);
                    }
                }
            }
            else
            {
                return RedirectToAction("index", "okullar");
            }
            return View(ekUcretler);
        }

        private List<DilOkulu_KursTipiEkUcretleri> KursEkUcretleriGetir(int kursTipiId)
        {
            kursTipiEkUcretleriRepository = new KursTipiEkUcretleriRepository();

            var ekUcretler = kursTipiEkUcretleriRepository.Liste().Where(k => k.KursTipId == kursTipiId && k.Durumu == 1).ToList();

            return ekUcretler;
        }

        public JsonResult KursEkUcretSil(int id)
        {
            string status = "error";
            string message = "";

            if (id > 0)
            {
                kursTipiEkUcretleriRepository = new KursTipiEkUcretleriRepository();

                genericKursTipiEkUcretleri = new GenericRepository<DilOkulu_KursTipiEkUcretleri>(kursTipiEkUcretleriRepository.DBContext);

                var ekUcret = kursTipiEkUcretleriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (ekUcret != null)
                {
                    var retVal = genericKursTipiEkUcretleri.Delete(ekUcret);
                    if (retVal > 0)
                    {
                        status = "ok";
                    }
                    else
                    {
                        message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
                    }
                }
            }

            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        private int OkulDilId(int okulId = 0)
        {
            if (okulId > 0)
            {
                try
                {
                    okulRepository = new OkulRepository();
                    return okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif }).DilId;

                }
                catch (Exception)
                {

                    return 0;
                }

            }
            else
            {
                return 0;
            }
        }

        public PartialViewResult _OkulOzetBilgileri()
        {
            int okulId = Convert.ToInt32("0" + Request.QueryString["okulId"]);
            if (okulId > 0)
            {
                okulRepository = new OkulRepository();
                okul = okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            }
            return PartialView(okul);
        }

        private List<DilOkulu_KursTipleri> KursTipleriniGetir(int okulId = 0)
        {
            kursTipiRepository = new KursTipiRepository();
            var kursTipleri = kursTipiRepository.Liste()
                .Where(kt => kt.OkulId == okulId && kt.Durumu != (byte)GeneralVariables.Durum.Silindi)
                .OrderBy(kt => kt.Baslik)
                .ToList();
            return kursTipleri;

        }

        #region Kopyalama
        public ActionResult Kopyala(int okulId)
        {
            merkezRepository = new MerkezRepository();

            var merkezler = merkezRepository.Liste().Where(m => m.Durumu == 1).OrderBy(m => m.Baslik).ToList();
            ViewBag.Merkezler = merkezler;
            ViewBag.OkulId = okulId;
            return View();
        }

        public JsonResult Kopyala_OkullariGetir(int merkezId)
        {
            okulRepository = new OkulRepository();
            var okullar = okulRepository.Liste().Where(
                        o =>
                        o.MerkezId == merkezId &&
                        o.Durumu == (byte)GeneralVariables.Durum.Aktif &&
                        o.DilOkulu_KursTipleri.Count <= 0
                    ).OrderBy(o => o.Baslik).Select(o => new
                    {
                        o.Id,
                        o.Baslik
                    }
                    ).ToList();

            return Json(okullar, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Kopyala_Process(int kopyalanacakOkulId, int kaydedilecekOkulId)
        {
            string status = "error";
            string message = "Hata: Merkez ve okul alanlarını kontrol edin!";
            if (kopyalanacakOkulId > 0 && kaydedilecekOkulId > 0)
            {
                #region New Instances
                kursTipiRepository = new KursTipiRepository();
                genericKursTipleri = new GenericRepository<DilOkulu_KursTipleri>(kursTipiRepository.DBContext);

                kursTipiEkUcretleriRepository = new KursTipiEkUcretleriRepository(kursTipiRepository.DBContext);
                genericKursTipiEkUcretleri = new GenericRepository<DilOkulu_KursTipiEkUcretleri>(kursTipiRepository.DBContext);

                kursTipiFiyatlandirmalariRepository = new KursTipiFiyatlandirmalariRepository(kursTipiRepository.DBContext);
                genericKursTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KursTipiFiyatlandirmalari>(kursTipiRepository.DBContext);

                kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository(kursTipiRepository.DBContext);
                genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>(kursTipiRepository.DBContext);
                #endregion

                //Yeni ekleme
                var kursTipleri = kursTipiRepository.Liste().Where(kt => kt.OkulId == kopyalanacakOkulId && kt.Durumu != (byte)GeneralVariables.Durum.Silindi).ToList();
                if (kursTipleri != null && kursTipleri.Count > 0)
                {
                    foreach (var itemKursTipi in kursTipleri)
                    {
                        DilOkulu_KursTipleri yeniTip = new DilOkulu_KursTipleri()
                        {
                            OkulId = kaydedilecekOkulId,
                            Baslik = itemKursTipi.Baslik,
                            KursKategoriId = itemKursTipi.KursKategoriId,
                            Aciklama = itemKursTipi.Aciklama,
                            KayitTarihi = DateTime.Now,
                            BasTarihiHerPazartesi = itemKursTipi.BasTarihiHerPazartesi,
                            BasTarihiOzel = itemKursTipi.BasTarihiOzel,
                            OzelTarihListesi = itemKursTipi.OzelTarihListesi,
                            Durumu = (byte)GeneralVariables.Durum.Aktif
                        };

                        var retVal = genericKursTipleri.Insert(yeniTip);
                        if (retVal != null)
                        {
                            #region Ek Ücretler
                            if (itemKursTipi.DilOkulu_KursTipiEkUcretleri != null && itemKursTipi.DilOkulu_KursTipiEkUcretleri.Count > 0)
                            {
                                foreach (var itemEkUcret in itemKursTipi.DilOkulu_KursTipiEkUcretleri)
                                {
                                    DilOkulu_KursTipiEkUcretleri yeniUcret = new DilOkulu_KursTipiEkUcretleri()
                                    {
                                        KursTipId = retVal.Id,
                                        BaslangicTarihi = itemEkUcret.BaslangicTarihi,
                                        BititisTarihi = itemEkUcret.BititisTarihi,
                                        Fiyat = itemEkUcret.Fiyat,
                                        KayitTarihi = DateTime.Now,
                                        Durumu = 1
                                    };
                                    var retValEkUcret = genericKursTipiEkUcretleri.Insert(yeniUcret);
                                }
                            }
                            #endregion

                            #region FiyatEkleme
                            if (itemKursTipi.DilOkulu_KursTipiFiyatlandirmalari != null && itemKursTipi.DilOkulu_KursTipiFiyatlandirmalari.Count > 0)
                            {
                                foreach (var itemFiyat in itemKursTipi.DilOkulu_KursTipiFiyatlandirmalari)
                                {

                                    DilOkulu_KursTipiFiyatlandirmalari yeniFiyat = new DilOkulu_KursTipiFiyatlandirmalari()
                                    {
                                        KursTipiId = retVal.Id,
                                        FiyatlandirmaModelId = itemFiyat.FiyatlandirmaModelId,
                                        StandartHafta_HaftaSayisi = itemFiyat.StandartHafta_HaftaSayisi,
                                        HerHaftayaOzel_Hafta = itemFiyat.HerHaftayaOzel_Hafta,
                                        HaftaBazliBirimFiyatli_HaftaBaslangic = itemFiyat.HaftaBazliBirimFiyatli_HaftaBaslangic,
                                        HaftaBazliBirimFiyatli_HaftaBitis = itemFiyat.HaftaBazliBirimFiyatli_HaftaBitis,
                                        Fiyat = itemFiyat.Fiyat,
                                        KayitTarihi = DateTime.Now,
                                        Durumu = (byte)GeneralVariables.Durum.Aktif
                                    };

                                    var retFiyat = genericKursTipiFiyatlandirmalari.Insert(yeniFiyat);
                                }
                            }
                            #endregion

                            #region Promosyon Tanımları

                            //if (itemKursTipi.DilOkulu_KursTipiPromosyonTanimlari != null && itemKursTipi.DilOkulu_KursTipiPromosyonTanimlari.Count > 0)
                            //{
                            //    foreach (var itemPromosyon in itemKursTipi.DilOkulu_KursTipiPromosyonTanimlari)
                            //    {
                            //        DilOkulu_KursTipiPromosyonTanimlari yeniPromosyon = new DilOkulu_KursTipiPromosyonTanimlari()
                            //        {
                            //            KursTipiId = retVal.Id,
                            //            PromosyonModelId = itemPromosyon.PromosyonModelId,
                            //            ExtraHafta_MinimumHafta = itemPromosyon.ExtraHafta_MinimumHafta,
                            //            ExtraHafta_UcretsizHaftaSayisi = itemPromosyon.ExtraHafta_UcretsizHaftaSayisi,
                            //            IndirimPromosyonu_Hafta = itemPromosyon.IndirimPromosyonu_Hafta,
                            //            IndirimPromosyonu_Oran = itemPromosyon.IndirimPromosyonu_Oran,
                            //            KursBazli_Hafta = itemPromosyon.KursBazli_Hafta,
                            //            KursBazli_Fiyat = itemPromosyon.KursBazli_Fiyat,
                            //            PromosyonBitisTarihi = itemPromosyon.PromosyonBitisTarihi,
                            //            KayitTarihi = DateTime.Now,

                            //            Durumu = (byte)GeneralVariables.Durum.Aktif
                            //        };

                            //        genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>();
                            //        var retPromosyon = genericKursTipiPromosyonTanimlari.Insert(yeniPromosyon);
                            //    }
                            //}
                            #endregion
                        }
                    }
                }

                status = "ok";
                message = "ok";
            }
            return Json(new { status = status, message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Kurs Tipi Promosyon Kopyala
        public ActionResult PromosyonKopyala(int merkezId = 0, int okulId = 0, int kursTipiId = 0, int promosyonTipiId = 0)
        {
            if (merkezId > 0)
            {

                okulRepository = new OkulRepository();
                okullar = okulRepository.Liste().
                    Where(o =>
                        o.MerkezId == merkezId &&
                        (o.Durumu == (int)GeneralVariables.Durum.Aktif || o.Durumu == (int)GeneralVariables.Durum.Pasif))
                        .OrderBy(o => o.Baslik).ToList();
                ViewBag.Okullar = okullar;

                kursTipiRepository = new KursTipiRepository(okulRepository.DBContext);
                ViewBag.MerkezId = merkezId;
                ViewBag.OkulId = okulId;
                ViewBag.KursTipiId = kursTipiId;
                ViewBag.PromosyonTipiId = promosyonTipiId;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PromosyonKopyala(int merkezId, int okulId, int kursTipiId, int promosyonTipiId, string[] kursTipleri)
        {

            if (merkezId > 0 && kursTipiId > 0)
            {

                kursTipiPromosyonTanimlariRepository = new KursTipiPromosyonTanimlariRepository();
                var secilenPromosyonTipleri = kursTipiPromosyonTanimlariRepository
                    .Liste().Where(kp => kp.KursTipiId == kursTipiId && kp.PromosyonModelId == promosyonTipiId).ToList();
                if (secilenPromosyonTipleri != null && secilenPromosyonTipleri.Count > 0)
                {
                    genericKursTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KursTipiPromosyonTanimlari>(kursTipiPromosyonTanimlariRepository.DBContext);

                    #region Silme işlemleri
                    int silinecekTipId = 0;
                    for (int i = 0; i < kursTipleri.Length; i++)
                    {
                        silinecekTipId = Convert.ToInt32(kursTipleri[i]);
                        var silinecekPromosyonTipleri = kursTipiPromosyonTanimlariRepository.Liste().Where(kp => kp.KursTipiId == silinecekTipId).ToList();

                        foreach (var itemSilinecek in silinecekPromosyonTipleri)
                        {
                            genericKursTipiPromosyonTanimlari.Delete(itemSilinecek);
                        }

                    }
                    #endregion

                    #region Ekleme işlemleri
                    int eklenecekTipId = 0;
                    foreach (var item in secilenPromosyonTipleri)
                    {
                        for (int i = 0; i < kursTipleri.Length; i++)
                        {
                            eklenecekTipId = Convert.ToInt32(kursTipleri[i]);

                            item.KursTipiId = eklenecekTipId;

                            var retVal = genericKursTipiPromosyonTanimlari.Insert(item);

                        }
                    }
                    #endregion
                }

                return RedirectToAction("PromosyonKopyalaTesekkur");
            }
            return View();
        }

        public ActionResult PromosyonKopyalaTesekkur()
        {
            return View();
        }
        #endregion

        #region Kurs Tipi SummerSupplement Kopyala
        public ActionResult SummerSupplementKopyala(int merkezId = 0, int okulId = 0, int kursTipiId = 0)
        {
            if (merkezId > 0)
            {

                okulRepository = new OkulRepository();
                okullar = okulRepository.Liste().
                    Where(o =>
                        o.MerkezId == merkezId &&
                        (o.Durumu == (int)GeneralVariables.Durum.Aktif || o.Durumu == (int)GeneralVariables.Durum.Pasif))
                        .OrderBy(o => o.Baslik).ToList();
                ViewBag.Okullar = okullar;

                kursTipiRepository = new KursTipiRepository(okulRepository.DBContext);
                ViewBag.MerkezId = merkezId;
                ViewBag.OkulId = okulId;
                ViewBag.KursTipiId = kursTipiId;
            }
            return View();
        }

        [HttpPost]
        public ActionResult SummerSupplementKopyala(int merkezId, int okulId, int kursTipiId, string[] kursTipleri)
        {

            if (merkezId > 0 && kursTipiId > 0)
            {
                kursTipiEkUcretleriRepository = new KursTipiEkUcretleriRepository();
                var secilenEkUcretTipleri = kursTipiEkUcretleriRepository
                    .Liste().Where(kp => kp.KursTipId == kursTipiId).ToList();

                if (secilenEkUcretTipleri != null && secilenEkUcretTipleri.Count > 0)
                {
                    genericKursTipiEkUcretleri = new GenericRepository<DilOkulu_KursTipiEkUcretleri>(kursTipiEkUcretleriRepository.DBContext);

                    #region Silme işlemleri
                    int silinecekTipId = 0;
                    for (int i = 0; i < kursTipleri.Length; i++)
                    {
                        silinecekTipId = Convert.ToInt32(kursTipleri[i]);
                        var silinecekEkUcretTipleri = kursTipiEkUcretleriRepository.Liste().Where(kp => kp.KursTipId == silinecekTipId).ToList();

                        foreach (var itemSilinecek in silinecekEkUcretTipleri)
                        {
                            genericKursTipiEkUcretleri.Delete(itemSilinecek);
                        }

                    }
                    #endregion

                    #region Ekleme işlemleri
                    int eklenecekTipId = 0;
                    foreach (var item in secilenEkUcretTipleri)
                    {
                        for (int i = 0; i < kursTipleri.Length; i++)
                        {
                            eklenecekTipId = Convert.ToInt32(kursTipleri[i]);

                            item.KursTipId = eklenecekTipId;

                            var retVal = genericKursTipiEkUcretleri.Insert(item);

                        }
                    }
                    #endregion
                }

                return RedirectToAction("SummerSupplementKopyalaTesekkur");
            }
            return View();
        }

        public ActionResult SummerSupplementKopyalaTesekkur()
        {
            return View();
        }
        #endregion

        #region Promosyon PDF İşlemleri

        //Kaydet
        public JsonResult PromosyonPdfKaydet(int okulId, string tarih)
        {
            string status = "ok";
            string msg = "";
            var fileName = "";
            try
            {
                if (!string.IsNullOrEmpty(tarih) && Tools.IsDate(tarih))
                {
                    okulRepository = new OkulRepository();
                    genericOkul = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);

                    var okul = okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });


                    if (Request.Files != null && Request.Files.Count > 0)
                    {
                        foreach (string file in Request.Files)
                        {
                            var fileContent = Request.Files[file];
                            if (fileContent != null && fileContent.ContentLength > 0)
                            {
                                // get a stream
                                var stream = fileContent.InputStream;
                                // and optionally write the file to disk
                                fileName = System.IO.Path.GetFileName(fileContent.FileName);
                                var path = System.IO.Path.Combine(Server.MapPath("~/content/uploads/"), fileName);
                                using (var fileStream = System.IO.File.Create(path))
                                {
                                    stream.CopyTo(fileStream);
                                }

                                fileName = "/content/uploads/" + fileName;
                            }
                        }
                        okul.PromosyonPdf = fileName;
                    }

                    okul.PromosyonBitisTarihi = Convert.ToDateTime(tarih);

                    genericOkul.Update(okul);

                }
                else
                {
                    status = "error";
                    msg = "Lütfen tarih ve dosya seçim alanını kontrol ediniz!";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new { status = status, message = msg, fileName = fileName }, JsonRequestBehavior.AllowGet);
        }
        //Sil
        public JsonResult PromosyonPdfSil(int okulId, string tarih)
        {
            string status = "ok";
            string msg = "";
            try
            {
                if (okulId > 0)
                {
                    okulRepository = new OkulRepository();
                    genericOkul = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);

                    var okul = okulRepository.Detay(okulId, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
                    okul.PromosyonPdf = "";
                    okul.PromosyonBitisTarihi = null;

                    genericOkul.Update(okul);

                }
                else
                {
                    status = "error";
                    msg = "Bir problem meydana geldi. Lütfen daha sonra tekrar deneyin!";
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new { status = status, message = msg }, JsonRequestBehavior.AllowGet);
        }


        #region PromosyonPdf Kopyala
        public ActionResult PromosyonPaketPDFKopyala(int merkezId = 0, int okulId = 0)
        {
            if (merkezId > 0)
            {

                okulRepository = new OkulRepository();
                okullar = okulRepository.Liste().
                    Where(o =>
                        o.MerkezId == merkezId &&
                        (o.Durumu == (int)GeneralVariables.Durum.Aktif || o.Durumu == (int)GeneralVariables.Durum.Pasif))
                        .OrderBy(o => o.Baslik).ToList();
                ViewBag.Okullar = okullar;

                kursTipiRepository = new KursTipiRepository(okulRepository.DBContext);
                ViewBag.MerkezId = merkezId;
                ViewBag.OkulId = okulId;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PromosyonPaketPDFKopyala(int merkezId, int okulId, string[] secilenOkullar)
        {
            if (merkezId > 0 && okulId > 0 && secilenOkullar.Length > 0)
            {

                okulRepository = new OkulRepository();
                var _secilenOkullar = okulRepository
                    .Liste().Where(o => o.MerkezId == merkezId && o.Id != okulId).ToList();
                if (_secilenOkullar != null && _secilenOkullar.Count > 0)
                {
                    genericOkul = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);

                    #region Silme işlemleri
                    int silinecekOkulId = 0;
                    for (int i = 0; i < secilenOkullar.Length; i++)
                    {
                        silinecekOkulId = Convert.ToInt32(secilenOkullar[i]);

                        var silinecekPromosyonKaydi = _secilenOkullar.Where(o => o.Id == silinecekOkulId).FirstOrDefault();
                        silinecekPromosyonKaydi.PromosyonPdf = "";
                        silinecekPromosyonKaydi.PromosyonBitisTarihi = null;
                        genericOkul.Update(silinecekPromosyonKaydi);

                    }
                    #endregion

                    #region Ekleme işlemleri

                    var secilenOkul = okulRepository
                    .Liste().Where(o => o.Id == okulId).FirstOrDefault();

                    if (secilenOkul != null)
                    {
                        foreach (var item in _secilenOkullar)
                        {

                            item.PromosyonPdf = secilenOkul.PromosyonPdf;
                            item.PromosyonBitisTarihi = secilenOkul.PromosyonBitisTarihi;

                            genericOkul.Update(item);
                        }
                    }

                    #endregion
                }

                return RedirectToAction("PromosyonPaketPDFKopyalaTesekkur");
            }
            return View();
        }

        public ActionResult PromosyonPaketPDFKopyalaTesekkur()
        {
            return View();
        }
        #endregion


        #endregion
    }
}
