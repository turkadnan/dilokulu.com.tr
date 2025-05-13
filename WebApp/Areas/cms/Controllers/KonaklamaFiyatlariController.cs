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
    public class KonaklamaFiyatlariController : Controller
    {
        //
        // GET: /cms/KonaklamaFiyatlari/

        #region Degiskenler
        private IOkulRepository okulRepository = null;
        private IKonaklamaKategoriRepository konaklamaKategoriRepository = null;
        private IKonaklamaTipiRepository konaklamaTipiRepository = null;
        private IKonaklamaTipiFiyatlandirmalariRepository konaklamaTipiFiyatlandirmalariRepository = null;
        private IKonaklamaTipiPromosyonTanimlariRepository konaklamaTipiPromosyonTanimlariRepository = null;
        private IKonaklamaTipiEkUcretleriRepository konaklamaTipiEkUcretleriRepository = null;
        private GenericRepository<DilOkulu_KonaklamaTipleri> generickonaklamaTipleri = null;
        private GenericRepository<DilOkulu_KonaklamaTipiFiyatlandirmalari> genericKonaklamaTipiFiyatlandirmalari = null;
        private GenericRepository<DilOkulu_KonaklamaTipiPromosyonTanimlari> genericKonaklamaTipiPromosyonTanimlari = null;
        private GenericRepository<DilOkulu_KonaklamaTipiEkUcretleri> genericKonaklamaTipiEkUcretleri = null;
        private DilOkulu_Okullar okul = null;
        private List<DilOkulu_Okullar> okullar = null;
        #endregion

        #region KonaklamaTipleri
        public ActionResult KonaklamaTipi(int okulId = 0)
        {
            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var konaklamaKategorileri = konaklamaKategoriRepository.Liste()
                .Where(kk => kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

            ViewBag.KonaklamaKategorileri = konaklamaKategorileri;
            ViewBag.OkulId = okulId.ToString();

            konaklamaTipiRepository = new KonaklamaTipiRepository();
            var konaklamaTipleri = konaklamaTipiRepository.Liste().Where(kt => kt.OkulId == okulId && kt.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kt => kt.Baslik).ToList();

            return View(konaklamaTipleri);
        }

        public ActionResult KonaklamaTipiEdit(int id = 0, int okulId = 0)
        {
            #region Kurs Kategorileri
            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var konaklamaKategorileri = konaklamaKategoriRepository.Liste()
                .Where(kk => kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

            ViewBag.KonaklamaKategorileri = konaklamaKategorileri;
            #endregion

            ViewBag.OkulId = okulId.ToString();

            konaklamaTipiRepository = new KonaklamaTipiRepository();
            var konaklamaTipi = konaklamaTipiRepository.Detay(id, new int[] { (byte)GeneralVariables.Durum.Aktif });

            return View(konaklamaTipi);
        }

        [HttpPost]
        public ActionResult KonaklamaTipiEdit(FormCollection fColl, int id = 0, int okulId = 0)
        {
            #region Kurs Kategorileri
            konaklamaKategoriRepository = new KonaklamaKategoriRepository();
            var konaklamaKategorileri = konaklamaKategoriRepository.Liste()
                .Where(kk => kk.Durumu != (byte)GeneralVariables.Durum.Silindi).OrderBy(kk => kk.Baslik).ToList();

            ViewBag.KonaklamaKategorileri = konaklamaKategorileri;

            #endregion

            #region Variables
            var konaklamaKategoriId = Convert.ToInt32("0" + fColl["konaklamaKategorisi"]);
            var baslik = fColl["baslik"];
            var aciklama = HttpUtility.UrlDecode(fColl["kursTipiAciklama"]);
            var konaklamaTipiTarih = fColl["konaklamaTipiTarih"];
            var ozelTarihListesi = Request["ozelTarihListesi"].TrimEnd('|');
            bool basTarihiYilBoyu = false;
            bool basTarihiOzel = false;
            #endregion

            ViewBag.OkulId = okulId.ToString();

            konaklamaTipiRepository = new KonaklamaTipiRepository();
            var konaklamaTipi = konaklamaTipiRepository.Detay(id, new int[] { (byte)GeneralVariables.Durum.Aktif });
            if (konaklamaTipi != null)
            {
                konaklamaTipi.KonaklamaKategoriId = konaklamaKategoriId;
                konaklamaTipi.Baslik = baslik;
                konaklamaTipi.Aciklama = aciklama;
                if (!string.IsNullOrEmpty(konaklamaTipiTarih))
                {
                    if (konaklamaTipiTarih == "ozel")
                    {
                        basTarihiOzel = true;
                        if (!string.IsNullOrEmpty(ozelTarihListesi))
                        {
                            konaklamaTipi.BasTarihiYilBoyu = false;
                            konaklamaTipi.BasTarihiOzel = true;
                            konaklamaTipi.OzelTarihListesi = ozelTarihListesi;
                        }
                    }
                    else
                    {
                        basTarihiYilBoyu = true;
                        konaklamaTipi.BasTarihiYilBoyu = true;
                        konaklamaTipi.BasTarihiOzel = false;
                        konaklamaTipi.OzelTarihListesi = string.Empty;
                    }
                }

                generickonaklamaTipleri = new GenericRepository<DilOkulu_KonaklamaTipleri>(konaklamaTipiRepository.DBContext);
                var retVal = generickonaklamaTipleri.Update(konaklamaTipi);
                if (retVal != null)
                {
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "error";
                }

            }
            return View(konaklamaTipi);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KonaklamaTipiEkle()
        {
            #region Variables
            string status = "error";
            string message = "";
            var okulId = Convert.ToInt32("0" + Request["okulId"]);
            var baslik = Request["baslik"];
            var konaklamaKategoriId = Convert.ToInt32("0" + Request["konaklamaKategoriId"]);
            var basTarihiStatus = Request["basTarihiStatus"];
            var ozelTarihListesi = Request["ozelTarihListesi"].TrimEnd('|');
            var aciklama = HttpUtility.UrlDecode(Request["aciklama"]);

            bool basTarihiYilBoyu = false;
            bool basTarihiOzel = false;

            if (basTarihiStatus == "ozel")
            {
                basTarihiOzel = true;
            }
            else
            {
                basTarihiYilBoyu = true;
            }
            #endregion

            if (okulId > 0 && !string.IsNullOrEmpty(baslik) && konaklamaKategoriId > 0)
            {
                konaklamaTipiRepository = new KonaklamaTipiRepository();
                DilOkulu_KonaklamaTipleri yeniTip = new DilOkulu_KonaklamaTipleri()
                {
                    OkulId = okulId,
                    Baslik = baslik,
                    KonaklamaKategoriId = konaklamaKategoriId,
                    Aciklama = aciklama,
                    KayitTarihi = DateTime.Now,
                    BasTarihiYilBoyu = basTarihiYilBoyu,
                    BasTarihiOzel = basTarihiOzel,
                    OzelTarihListesi = ozelTarihListesi,
                    Durumu = (byte)GeneralVariables.Durum.Aktif
                };

                generickonaklamaTipleri = new GenericRepository<DilOkulu_KonaklamaTipleri>();
                var retVal = generickonaklamaTipleri.Insert(yeniTip);
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
        /// <returns></returns>
        public JsonResult KonaklamaTipiSil(int id = 0)
        {
            string status = "error";
            string message = "Hata:Silme işlemi yapılamadı!";

            if (id > 0)
            {
                konaklamaTipiRepository = new KonaklamaTipiRepository();
                var konaklamaTipi = konaklamaTipiRepository.Liste().Where(kt => kt.Id == id).FirstOrDefault(); ;
                //&& kt.Durumu != (byte)GeneralVariables.Durum.Silindi
                if (konaklamaTipi != null)
                {
                    generickonaklamaTipleri = new GenericRepository<DilOkulu_KonaklamaTipleri>(konaklamaTipiRepository.DBContext);
                    var retVal = generickonaklamaTipleri.Delete(konaklamaTipi);
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

        /// <summary>
        /// Tanımlı Fiyat modelini bulan metod
        /// </summary>
        /// <returns></returns>
        public JsonResult KonaklamaTipiFiyatModeliBul()
        {
            #region Variables
            string status = "error";
            var konaklamaTipiId = Convert.ToInt32("0" + Request["konaklamaTipiId"]);
            var fiyatlandirmaModelId = 0;
            #endregion

            konaklamaTipiFiyatlandirmalariRepository = new KonaklamaTipiFiyatlandirmalariRepository();
            var FiyatModeller = konaklamaTipiFiyatlandirmalariRepository.Liste().Where(pt => pt.KonaklamaTipiId == konaklamaTipiId).Select(pt => pt.FiyatlandirmaModelId).ToList();
            if (FiyatModeller != null && FiyatModeller.Count > 0)
            {
                fiyatlandirmaModelId = FiyatModeller[0];
                status = "ok";
            }

            return Json(new { fiyatlandirmaModelId = fiyatlandirmaModelId, status = status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Fiyat Tanimlari
        public ActionResult FiyatTanimlari(int okulId = 0, int konaklamaTipiId = 0)
        {
            if (okulId > 0)
            {
                ViewBag.KonaklamaTipleri = KonaklamaTipleriniGetir(okulId);
                if (konaklamaTipiId > 0)
                    ViewBag.KonaklamaTipiId = konaklamaTipiId;

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
        public PartialViewResult FiyatTanimlari_HaftaTanimlari(byte? fiyatlandirmaTipiId, int konaklamaTipiId = 0)
        {
            string partialViewName = "";
            List<DilOkulu_KonaklamaTipiFiyatlandirmalari> konaklamaTipiFiyatlari = null;
            if (konaklamaTipiId > 0)
            {
                konaklamaTipiFiyatlandirmalariRepository = new KonaklamaTipiFiyatlandirmalariRepository();
                konaklamaTipiFiyatlari = konaklamaTipiFiyatlandirmalariRepository.Liste()
                    .Where(f =>
                        f.FiyatlandirmaModelId == fiyatlandirmaTipiId &&
                        f.KonaklamaTipiId == konaklamaTipiId &&
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
            return PartialView(partialViewName, konaklamaTipiFiyatlari);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KonaklamaTipiFiyatiEkle()
        {
            #region Variables
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
            var konaklamaTipiId = Convert.ToInt32("0" + Request["konaklamaTipiId"]);
            var fiyatlandirmaModelId = Convert.ToByte("0" + Request["fiyatlandirmaTipiId"]);
            var hafta = Convert.ToInt32("0" + Request["hafta"]);
            var haftaBaslangic = Convert.ToInt32("0" + Request["haftaBaslangic"]);
            var haftaBitis = Convert.ToInt32("0" + Request["haftaBitis"]);
            var fiyat = Convert.ToDecimal("0" + Request["fiyat"]);
            #endregion

            if (konaklamaTipiId > 0 && fiyatlandirmaModelId > 0 && fiyat > 0)
            {
                konaklamaTipiFiyatlandirmalariRepository = new KonaklamaTipiFiyatlandirmalariRepository();

                genericKonaklamaTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KonaklamaTipiFiyatlandirmalari>(konaklamaTipiFiyatlandirmalariRepository.DBContext);




                //Yeni ekleme
                DilOkulu_KonaklamaTipiFiyatlandirmalari yeniFiyat = new DilOkulu_KonaklamaTipiFiyatlandirmalari();

                yeniFiyat.KonaklamaTipiId = konaklamaTipiId;
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
                        var mevcutFiyatlandirmaTanim = konaklamaTipiFiyatlandirmalariRepository.Liste().Where(f =>
                            f.KonaklamaTipiId == konaklamaTipiId &&
                            f.FiyatlandirmaModelId == fiyatlandirmaModelId &&
                            f.HerHaftayaOzel_Hafta == hafta
                            ).FirstOrDefault();
                        if (mevcutFiyatlandirmaTanim != null)
                        {
                            genericKonaklamaTipiFiyatlandirmalari.Delete(mevcutFiyatlandirmaTanim);
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


                var retVal = genericKonaklamaTipiFiyatlandirmalari.Insert(yeniFiyat);
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
        public JsonResult KonaklamaTipiFiyatiGuncelle()
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
                    konaklamaTipiFiyatlandirmalariRepository = new KonaklamaTipiFiyatlandirmalariRepository();

                    genericKonaklamaTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KonaklamaTipiFiyatlandirmalari>(konaklamaTipiFiyatlandirmalariRepository.DBContext);


                    //Fiyat Getir
                    DilOkulu_KonaklamaTipiFiyatlandirmalari fiyatDetay = konaklamaTipiFiyatlandirmalariRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });

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


                    var retVal = genericKonaklamaTipiFiyatlandirmalari.Update(fiyatDetay);
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
        public JsonResult KonaklamaTipiFiyatSil()
        {
            #region Variables
            string status = "error";
            string message = "İşlem sırasında problem ile karşılaşıldı!";
            var konaklamaTipiFiyatId = Convert.ToInt32("0" + Request["konaklamaTipiFiyatId"]);
            #endregion

            if (konaklamaTipiFiyatId > 0)
            {
                konaklamaTipiFiyatlandirmalariRepository = new KonaklamaTipiFiyatlandirmalariRepository();

                genericKonaklamaTipiFiyatlandirmalari = new GenericRepository<DilOkulu_KonaklamaTipiFiyatlandirmalari>(konaklamaTipiFiyatlandirmalariRepository.DBContext);

                var fiyat = konaklamaTipiFiyatlandirmalariRepository.Detay(konaklamaTipiFiyatId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (fiyat != null)
                {
                    var retVal = genericKonaklamaTipiFiyatlandirmalari.Delete(fiyat);
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

        #region Promosyon Tanımları
        public ActionResult PromosyonTanimlari(int okulId = 0, int konaklamaTipiId = 0)
        {
            if (okulId > 0)
            {
                ViewBag.KonaklamaTipleri = KonaklamaTipleriniGetir(okulId);
                if (konaklamaTipiId > 0)
                    ViewBag.KonaklamaTipiId = konaklamaTipiId;

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
        public PartialViewResult PromosyonTanimlari_PromosyonTanimlari(byte? promosyonTipiId, int konaklamaTipiId = 0)
        {
            string partialViewName = "";
            List<DilOkulu_KonaklamaTipiPromosyonTanimlari> konaklamaTipiPromosyonlari = null;
            if (konaklamaTipiId > 0)
            {
                konaklamaTipiPromosyonTanimlariRepository = new KonaklamaTipiPromosyonTanimlariRepository();
                konaklamaTipiPromosyonlari = konaklamaTipiPromosyonTanimlariRepository.Liste()
                    .Where(f =>
                        f.PromosyonModelId == promosyonTipiId &&
                        f.KonaklamaTipiId == konaklamaTipiId &&
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
                    //Yüzdesel İndirim Promosyonu
                    partialViewName = "_PromosyonTanimi_IndirimPromosyonu";
                    break;
                case 3:
                    //Fiyat Bazlı İndirim
                    partialViewName = "_PromosyonTanimi_KonaklamaBazliPromosyon";
                    break;
                case 4:
                    //İndirimli Birim Fiyat
                    partialViewName = "_PromosyonTanimi_IndirimliBirimFiyatPromosyonu";
                    break;
                default:
                    partialViewName = "";
                    break;
            }
            return PartialView(partialViewName, konaklamaTipiPromosyonlari);
        }

        /// <summary>
        /// Ajax Insert
        /// </summary>
        /// <returns></returns>
        public JsonResult KonaklamaTipiPromosyonEkle()
        {
            #region Variables
            string status = "error";
            string message = "Kayıt işlemi sırasında problem ile karşılaşıldı!";
            var konaklamaTipiId = Convert.ToInt32("0" + Request["konaklamaTipiId"]);
            var promosyonModelId = Convert.ToByte("0" + Request["promosyonTipiId"]);

            var extraHafta_MinimumHafta = Convert.ToInt32("0" + Request["extraHafta_MinimumHafta"]);
            var extraHafta_UcretsizHaftaSayisi = Convert.ToInt32("0" + Request["extraHafta_UcretsizHaftaSayisi"]);

            var indirimPromosyonu_Hafta = Convert.ToInt32("0" + Request["indirimPromosyonu_Hafta"]);
            var indirimPromosyonu_HaftaMax = Convert.ToInt32("0" + Request["indirimPromosyonu_HaftaMax"]);
            var indirimPromosyonu_Oran = Convert.ToInt32("0" + Request["indirimPromosyonu_Oran"]);
            var indirimPromosyonu_BirimFiyat = Convert.ToInt32("0" + Request["indirimPromosyonu_BirimFiyat"]);

            var konaklamaBazli_Hafta = Convert.ToInt32("0" + Request["konaklamaBazli_Hafta"]);
            var KonaklamaBazli_Fiyat = Convert.ToInt32("0" + Request["KonaklamaBazli_Fiyat"]);

            var promosyonBitisTarihi = Convert.ToDateTime(Request["promosyonBitisTarihi"]);

            string _kalisTarihiBaslangic = Request["kalisTarihiBaslangic"];
            string _kalisTarihiBitis = Request["kalisTarihiBitis"];

            DateTime? kalisTarihiBaslangic = null;
            DateTime? kalisTarihiBitis = null;

            if (Tools.IsDate(_kalisTarihiBaslangic))
                kalisTarihiBaslangic = Convert.ToDateTime(_kalisTarihiBaslangic);

            if (Tools.IsDate(_kalisTarihiBitis))
                kalisTarihiBitis = Convert.ToDateTime(_kalisTarihiBitis);
            #endregion

            if (konaklamaTipiId > 0 && promosyonModelId > 0)
            {
                konaklamaTipiPromosyonTanimlariRepository = new KonaklamaTipiPromosyonTanimlariRepository();

                genericKonaklamaTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KonaklamaTipiPromosyonTanimlari>();

                //Yeni ekleme
                DilOkulu_KonaklamaTipiPromosyonTanimlari yeniPromosyon = new DilOkulu_KonaklamaTipiPromosyonTanimlari();

                yeniPromosyon.KonaklamaTipiId = konaklamaTipiId;
                yeniPromosyon.PromosyonModelId = promosyonModelId;

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
                        //Konaklama Bazlı Promosyon
                        yeniPromosyon.KonaklamaBazli_Hafta = konaklamaBazli_Hafta;
                        yeniPromosyon.KonaklamaBazli_Fiyat = KonaklamaBazli_Fiyat;
                        break;
                    case 4:
                        //İndirimli Birim Fiyat
                        yeniPromosyon.IndirimPromosyonu_Hafta = indirimPromosyonu_Hafta;
                        yeniPromosyon.IndirimPromosyonu_HaftaMax = indirimPromosyonu_HaftaMax;
                        yeniPromosyon.IndirimPromosyonu_BirimFiyat = indirimPromosyonu_BirimFiyat;
                        break;
                }

                yeniPromosyon.PromosyonBitisTarihi = promosyonBitisTarihi;
                yeniPromosyon.KalisTarihiBaslangic = kalisTarihiBaslangic;
                yeniPromosyon.KalisTarihiBitis = kalisTarihiBitis;
                yeniPromosyon.Durumu = (byte)GeneralVariables.Durum.Aktif;
                yeniPromosyon.KayitTarihi = DateTime.Now;


                var retVal = genericKonaklamaTipiPromosyonTanimlari.Insert(yeniPromosyon);
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
        /// <returns></returns>
        public JsonResult KonaklamaTipiPromosyonSil()
        {
            #region Variables
            string status = "error";
            string message = "İşlem sırasında problem ile karşılaşıldı!";
            var konaklamaTipiFiyatId = Convert.ToInt32("0" + Request["konaklamaTipiFiyatId"]);
            #endregion

            if (konaklamaTipiFiyatId > 0)
            {
                konaklamaTipiPromosyonTanimlariRepository = new KonaklamaTipiPromosyonTanimlariRepository();

                genericKonaklamaTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KonaklamaTipiPromosyonTanimlari>(konaklamaTipiPromosyonTanimlariRepository.DBContext);

                var promosyon = konaklamaTipiPromosyonTanimlariRepository.Detay(konaklamaTipiFiyatId, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (promosyon != null)
                {
                    var retVal = genericKonaklamaTipiPromosyonTanimlari.Delete(promosyon);
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
        public JsonResult KonaklamaTipiPromosyonModeliBul()
        {
            #region Variables
            string status = "error";
            var konaklamaTipiId = Convert.ToInt32("0" + Request["konaklamaTipiId"]);
            var promosyonTipiId = 0;
            #endregion

            konaklamaTipiPromosyonTanimlariRepository = new KonaklamaTipiPromosyonTanimlariRepository();
            var PromosyonModeller = konaklamaTipiPromosyonTanimlariRepository.Liste().Where(pt => pt.KonaklamaTipiId == konaklamaTipiId).Select(pt => pt.PromosyonModelId).ToList();
            if (PromosyonModeller != null && PromosyonModeller.Count > 0)
            {
                promosyonTipiId = PromosyonModeller[0];
                status = "ok";
            }

            return Json(new { promosyonTipiId = promosyonTipiId, status = status }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Konaklama Ek Ücretler
        public ActionResult KonaklamaEkUcretler(int okulId = 0, int konaklamaTipiId = 0)
        {
            List<DilOkulu_KonaklamaTipiEkUcretleri> ekUcretler = null;

            if (okulId > 0)
            {
                ViewBag.KonaklamaTipleri = KonaklamaTipleriniGetir(okulId);
                if (konaklamaTipiId > 0)
                {
                    ViewBag.KonaklamaTipiId = konaklamaTipiId;

                    ekUcretler = KonaklamaEkUcretleriGetir(konaklamaTipiId);
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
        public ActionResult KonaklamaEkUcretler(FormCollection fColl, int okulId = 0, int konaklamaTipiId = 0)
        {
            #region Form Collection
            int konaklamaTipId = Convert.ToInt32("0" + fColl["konaklamaTipi"]);
            string _baslangicTarihi = fColl["BaslangicTarihi"];
            string _bitisTarihi = fColl["BititisTarihi"];
            string _fiyat = fColl["Fiyat"];
            int maksimumHafta = Convert.ToInt32("0" + fColl["MaksimunHafta"]);

            DateTime baslangicTarihi = Tools.IsDate(_baslangicTarihi) ? Convert.ToDateTime(_baslangicTarihi) : new DateTime();
            DateTime bitisTarihi = Tools.IsDate(_bitisTarihi) ? Convert.ToDateTime(_bitisTarihi) : new DateTime();
            decimal fiyat = Tools.IsDecimal(_fiyat) ? Convert.ToDecimal(_fiyat) : 0;

            #endregion

            List<DilOkulu_KonaklamaTipiEkUcretleri> ekUcretler = null;

            if (okulId > 0)
            {
                ViewBag.KonaklamaTipleri = KonaklamaTipleriniGetir(okulId);
                if (konaklamaTipiId > 0)
                {
                    ViewBag.KonaklamaTipiId = konaklamaTipiId;
                    ekUcretler = KonaklamaEkUcretleriGetir(konaklamaTipId);
                }

                ViewBag.OkulId = okulId.ToString();

                //Kayıt işlemi
                if (konaklamaTipId > 0 && baslangicTarihi.Year > 2000 && bitisTarihi.Year > 200 && fiyat > 0)
                {

                    konaklamaTipiEkUcretleriRepository = new KonaklamaTipiEkUcretleriRepository();
                    DilOkulu_KonaklamaTipiEkUcretleri yeniUcret = new DilOkulu_KonaklamaTipiEkUcretleri()
                    {
                        KonaklamaTipId = konaklamaTipId,
                        BaslangicTarihi = baslangicTarihi,
                        BititisTarihi = bitisTarihi,
                        MaksimumHafta = maksimumHafta,
                        Fiyat = fiyat,
                        KayitTarihi = DateTime.Now,
                        Durumu = 1
                    };

                    genericKonaklamaTipiEkUcretleri = new GenericRepository<DilOkulu_KonaklamaTipiEkUcretleri>(konaklamaTipiEkUcretleriRepository.DBContext);

                    var retVal = genericKonaklamaTipiEkUcretleri.Insert(yeniUcret);
                    if (retVal != null)
                    {
                        ekUcretler = KonaklamaEkUcretleriGetir(konaklamaTipId);
                    }
                }
            }
            else
            {
                return RedirectToAction("index", "okullar");
            }
            return View(ekUcretler);
        }

        private List<DilOkulu_KonaklamaTipiEkUcretleri> KonaklamaEkUcretleriGetir(int konaklamaTipId)
        {
            konaklamaTipiEkUcretleriRepository = new KonaklamaTipiEkUcretleriRepository();

            var ekUcretler = konaklamaTipiEkUcretleriRepository.Liste().Where(k => k.KonaklamaTipId == konaklamaTipId && k.Durumu == 1).ToList();

            return ekUcretler;
        }

        public JsonResult KonaklamaEkUcretSil(int id)
        {
            string status = "error";
            string message = "";

            if (id > 0)
            {
                konaklamaTipiEkUcretleriRepository = new KonaklamaTipiEkUcretleriRepository();

                genericKonaklamaTipiEkUcretleri = new GenericRepository<DilOkulu_KonaklamaTipiEkUcretleri>(konaklamaTipiEkUcretleriRepository.DBContext);

                var ekUcret = konaklamaTipiEkUcretleriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (ekUcret != null)
                {
                    var retVal = genericKonaklamaTipiEkUcretleri.Delete(ekUcret);
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

        private List<DilOkulu_KonaklamaTipleri> KonaklamaTipleriniGetir(int okulId = 0)
        {
            konaklamaTipiRepository = new KonaklamaTipiRepository();
            var konaklamaTipleri = konaklamaTipiRepository.Liste()
                .Where(kt => kt.OkulId == okulId && kt.Durumu != (byte)GeneralVariables.Durum.Silindi)
                .OrderBy(kt => kt.Baslik)
                .ToList();
            return konaklamaTipleri;

        }

        #region Kurs Tipi Promosyon Kopyala
        public ActionResult PromosyonKopyala(int merkezId = 0, int okulId = 0, int konaklamaTipiId = 0, int promosyonTipiId = 0)
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

                konaklamaTipiRepository = new KonaklamaTipiRepository(okulRepository.DBContext);
                ViewBag.MerkezId = merkezId;
                ViewBag.OkulId = okulId;
                ViewBag.KonaklamaTipiId = konaklamaTipiId;
                ViewBag.PromosyonTipiId = promosyonTipiId;
            }
            return View();
        }

        [HttpPost]
        public ActionResult PromosyonKopyala(int merkezId, int okulId, int konaklamaTipiId, int promosyonTipiId, string[] konaklamaTipleri)
        {

            if (merkezId > 0 && konaklamaTipiId > 0)
            {

                konaklamaTipiPromosyonTanimlariRepository = new KonaklamaTipiPromosyonTanimlariRepository();
                var secilenPromosyonTipleri = konaklamaTipiPromosyonTanimlariRepository
                    .Liste().Where(kp => kp.KonaklamaTipiId == konaklamaTipiId && kp.PromosyonModelId == promosyonTipiId).ToList();
                if (secilenPromosyonTipleri != null && secilenPromosyonTipleri.Count > 0)
                {
                    genericKonaklamaTipiPromosyonTanimlari = new GenericRepository<DilOkulu_KonaklamaTipiPromosyonTanimlari>(konaklamaTipiPromosyonTanimlariRepository.DBContext);

                    #region Silme işlemleri
                    int silinecekTipId = 0;
                    for (int i = 0; i < konaklamaTipleri.Length; i++)
                    {
                        silinecekTipId = Convert.ToInt32(konaklamaTipleri[i]);
                        var silinecekPromosyonTipleri = konaklamaTipiPromosyonTanimlariRepository.Liste().Where(kp => kp.KonaklamaTipiId == silinecekTipId).ToList();

                        foreach (var itemSilinecek in silinecekPromosyonTipleri)
                        {
                            genericKonaklamaTipiPromosyonTanimlari.Delete(itemSilinecek);
                        }

                    }
                    #endregion

                    #region Ekleme işlemleri
                    int eklenecekTipId = 0;
                    foreach (var item in secilenPromosyonTipleri)
                    {
                        for (int i = 0; i < konaklamaTipleri.Length; i++)
                        {
                            eklenecekTipId = Convert.ToInt32(konaklamaTipleri[i]);

                            item.KonaklamaTipiId = eklenecekTipId;

                            var retVal = genericKonaklamaTipiPromosyonTanimlari.Insert(item);

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

        #region Konaklama Tipi Ek Ucret Kopyala
        public ActionResult KonaklamaEkUcretKopyala(int merkezId = 0, int okulId = 0, int konaklamaTipiId = 0)
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

                konaklamaTipiRepository = new KonaklamaTipiRepository(okulRepository.DBContext);
                ViewBag.MerkezId = merkezId;
                ViewBag.OkulId = okulId;
                ViewBag.KonaklamaTipiId = konaklamaTipiId;
            }
            return View();
        }

        [HttpPost]
        public ActionResult KonaklamaEkUcretKopyala(int merkezId, int okulId, int konaklamaTipiId, string[] konaklamaTipleri)
        {

            if (merkezId > 0 && konaklamaTipiId > 0)
            {
                konaklamaTipiEkUcretleriRepository = new KonaklamaTipiEkUcretleriRepository();
                var secilenEkUcretTipleri = konaklamaTipiEkUcretleriRepository
                    .Liste().Where(kp => kp.KonaklamaTipId == konaklamaTipiId).ToList();

                if (secilenEkUcretTipleri != null && secilenEkUcretTipleri.Count > 0)
                {
                    genericKonaklamaTipiEkUcretleri = new GenericRepository<DilOkulu_KonaklamaTipiEkUcretleri>(konaklamaTipiEkUcretleriRepository.DBContext);

                    #region Silme işlemleri
                    int silinecekTipId = 0;
                    for (int i = 0; i < konaklamaTipleri.Length; i++)
                    {
                        silinecekTipId = Convert.ToInt32(konaklamaTipleri[i]);
                        var silinecekEkUcretTipleri = konaklamaTipiEkUcretleriRepository.Liste().Where(kp => kp.KonaklamaTipId == silinecekTipId).ToList();

                        foreach (var itemSilinecek in silinecekEkUcretTipleri)
                        {
                            genericKonaklamaTipiEkUcretleri.Delete(itemSilinecek);
                        }

                    }
                    #endregion

                    #region Ekleme işlemleri
                    int eklenecekTipId = 0;
                    foreach (var item in secilenEkUcretTipleri)
                    {
                        for (int i = 0; i < konaklamaTipleri.Length; i++)
                        {
                            eklenecekTipId = Convert.ToInt32(konaklamaTipleri[i]);

                            item.KonaklamaTipId = eklenecekTipId;
                            var retVal = genericKonaklamaTipiEkUcretleri.Insert(item);
                        }
                    }
                    #endregion
                }

                return RedirectToAction("KonaklamaEkUcretKopyalaTesekkur");
            }
            return View();
        }

        public ActionResult KonaklamaEkUcretKopyalaTesekkur()
        {
            return View();
        }
        #endregion
    }
}
