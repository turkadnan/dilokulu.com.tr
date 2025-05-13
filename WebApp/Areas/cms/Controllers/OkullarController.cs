using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class OkullarController : BaseController
    {
        //
        // GET: /cms/Okullar/
        #region Degiskenler
        private IMerkezRepository merkezRepository = null;
        private IOkulRepository okulRepository = null;
        private IEtiketRepository etiketRepository = null;
        private IGaleriRepository galeriRepository = null;
        private IEtiketIliskileriRepository etiketIliskileriRepository = null;
        private ISunulanProgramRepository sunulanProgramRepository = null;
        private GenericRepository<DilOkulu_Okullar> okulGenericRepository = null;
        private GenericRepository<DilOkulu_Etiketler> etiketlerGenericRepository = null;
        private GenericRepository<DilOkulu_EtiketIliskileri> etiketIliskileriGenericRepository = null;
        private string merkezBaslik = "";
        #endregion

        public ActionResult Index()
        {
            ViewBag.Merkezler = MerkezleriGetir();
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fCollection)
        {
            #region FormCollection
            int merkezId = Convert.ToInt32(fCollection["SelectMerkez"]);
            #endregion

            okulRepository = new OkulRepository();
            var okullar = okulRepository.CMSOkullarListesi(merkezId);

            ViewBag.Merkezler = MerkezleriGetir();
            ViewBag.MerkezId = merkezId;

            return View(okullar);
        }

        public ActionResult yeni()
        {
            okulRepository = new OkulRepository();
            ViewBag.Merkezler = MerkezleriGetir();
            ViewBag.GaleriListesi = GaleriListesiGetir();
            ViewBag.SunulanProgramlar = SunulanProgramlariGetir();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fCollection, string[] chk_Okultipi, string[] chk_KonaklamaTurleri, string[] chk_SunulanProgramlar)
        {
            #region FormCollection
            int dilId = Convert.ToInt32(fCollection["DilId"]);
            int ulkeId = Convert.ToInt32(fCollection["UlkeId"]);
            int sehirId = Convert.ToInt32(fCollection["SehirId"]);
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            string okulKapasitesi = fCollection["OkulKapasitesi"];
            string sinifKapasitesi = fCollection["SinifKapasitesi"];
            string okulOlanaklari = fCollection["OkulOlanaklari"];
            string sosyalAktiviteler = fCollection["SosyalAktiviteler"];
            string baslik = fCollection["Baslik"];
            string icerik = fCollection["Icerik"];
            string harita = fCollection["Harita"];
            string streetView = fCollection["StreetView"];
            string videoUrl = fCollection["VideoUrl"];
            DateTime kayitTarihi = DateTime.Now;
            string onecikanOkul = fCollection["cbOnecikanOkul"];
            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            string selectMerkez = fCollection["SelectMerkez"];
            int galeriId = Convert.ToInt32(fCollection["selectGaleri"]);
            #endregion

            okulGenericRepository = new GenericRepository<DilOkulu_Okullar>();

            int merkezId = 0;
            if (!string.IsNullOrEmpty(selectMerkez))
            {
                merkezId = Convert.ToInt32(selectMerkez);

                merkezRepository = new MerkezRepository();
                merkezBaslik = merkezRepository.Liste().Where(m => m.Id == merkezId).FirstOrDefault().Baslik;
            }

            if (!string.IsNullOrEmpty(baslik) && merkezId > 0)
            {
                DilOkulu_Okullar okul = new DilOkulu_Okullar()
                {
                    MerkezId = merkezId,
                    DilId = dilId,
                    UlkeId = ulkeId,
                    SehirId = sehirId,
                    Baslik = baslik.Trim(),
                    Seo_Keywords = seo_Keywords,
                    Seo_Descriptions = seo_Descriptions,
                    OkulKapasitesi = okulKapasitesi,
                    SinifKapasitesi = sinifKapasitesi,
                    OkulOlanaklari = okulOlanaklari,
                    SosyalAktiviteler = sosyalAktiviteler,
                    OkulTipi = (chk_Okultipi != null && chk_Okultipi.Length > 0 ? string.Join(", ", chk_Okultipi) : ""),
                    KonaklamaTurleri = (chk_KonaklamaTurleri != null && chk_KonaklamaTurleri.Length > 0 ? string.Join(", ", chk_KonaklamaTurleri) : ""),
                    SunulanProgramlar = (chk_SunulanProgramlar != null && chk_SunulanProgramlar.Length > 0 ? string.Join(", ", chk_SunulanProgramlar) : ""),
                    Icerik = icerik,
                    Harita = harita,
                    StreetView = streetView,
                    GaleriId = galeriId,
                    VideoUrl = videoUrl,
                    Url = Tools.ReplaceTitle(merkezBaslik.Trim() + "-" + baslik.Trim()),
                    KayitTarihi = kayitTarihi,
                    OnecikanOkul = (!string.IsNullOrEmpty(onecikanOkul) && onecikanOkul.ToLower() == "on" ? true : false),
                    Durumu = durumu

                };

                DilOkulu_Okullar RetOkul = okulGenericRepository.Insert(okul);

                if (RetOkul != null)
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

            okulRepository = new OkulRepository(okulGenericRepository.dbContext);
            ViewBag.Merkezler = MerkezleriGetir();
            ViewBag.GaleriListesi = GaleriListesiGetir();
            ViewBag.SunulanProgramlar = SunulanProgramlariGetir();

            return View();
        }

        public ActionResult detay(int id)
        {
            okulRepository = new OkulRepository();
            var okul = okulRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });

            var etiketler = EtiketleriGetir();
            ViewBag.Merkezler = MerkezleriGetir();
            ViewBag.Etiketler = etiketler;
            ViewBag.IlislikiEtiketler = IliskiliEtiketleriGetir(etiketler, id);
            ViewBag.GaleriListesi = GaleriListesiGetir();
            ViewBag.SunulanProgramlar = SunulanProgramlariGetir();
            return View(okul);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult detay(FormCollection fCollection, HttpPostedFileBase file, string[] GenelBilgiMerkezdenAl, string[] chk_Okultipi, string[] chk_KonaklamaTurleri, string[] chk_SunulanProgramlar)
        {
            #region FormCollection
            int id = Convert.ToInt32(fCollection["Id"]);
            int dilId = Convert.ToInt32(fCollection["DilId"]);
            int ulkeId = Convert.ToInt32(fCollection["UlkeId"]);
            int sehirId = Convert.ToInt32(fCollection["SehirId"]);
            string seo_Keywords = fCollection["Seo_Keywords"];
            string seo_Descriptions = fCollection["Seo_Descriptions"];
            string okulKapasitesi = fCollection["OkulKapasitesi"];
            string sinifKapasitesi = fCollection["SinifKapasitesi"];
            string okulOlanaklari = fCollection["OkulOlanaklari"];
            string sosyalAktiviteler = fCollection["SosyalAktiviteler"];
            string baslik = fCollection["Baslik"];
            string icerik = fCollection["Icerik"];
            string harita = fCollection["Harita"];
            string streetView = fCollection["StreetView"];
            string videoUrl = fCollection["VideoUrl"];
            string onecikanOkul = fCollection["cbOnecikanOkul"];
            byte durumu = Convert.ToByte(fCollection["selectDurum"]);
            int merkezId = Convert.ToInt32(fCollection["SelectMerkez"]);
            int galeriId = Convert.ToInt32(fCollection["selectGaleri"]);
            string hfEtiketler = fCollection["hfEtiketler"];
            #endregion

            okulRepository = new OkulRepository();
            var okul = okulRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif, (int)GeneralVariables.Durum.Pasif });
            merkezBaslik = okul.DilOkulu_Merkez.Baslik;
            okul.MerkezId = merkezId;
            okul.DilId = dilId;
            okul.UlkeId = ulkeId;
            okul.SehirId = sehirId;
            okul.Seo_Keywords = seo_Keywords;
            okul.Seo_Descriptions = seo_Descriptions;
            okul.OkulKapasitesi = okulKapasitesi;
            okul.SinifKapasitesi = sinifKapasitesi;
            okul.OkulOlanaklari = okulOlanaklari;
            okul.SosyalAktiviteler = sosyalAktiviteler;
            okul.OkulTipi = (chk_Okultipi != null && chk_Okultipi.Length > 0 ? string.Join(", ", chk_Okultipi) : "");
            okul.KonaklamaTurleri = (chk_KonaklamaTurleri != null && chk_KonaklamaTurleri.Length > 0 ? string.Join(", ", chk_KonaklamaTurleri) : "");
            okul.SunulanProgramlar = (chk_SunulanProgramlar != null && chk_SunulanProgramlar.Length > 0 ? string.Join(", ", chk_SunulanProgramlar) : "");
            okul.Baslik = baslik;
            okul.Icerik = icerik;
            okul.Harita = harita;
            okul.StreetView = streetView;
            okul.VideoUrl = videoUrl;
            okul.GaleriId = galeriId;
            okul.Url = Tools.ReplaceTitle(merkezBaslik + "-" + baslik.Trim());
            okul.Durumu = durumu;

            if (!string.IsNullOrEmpty(onecikanOkul))
            {
                okul.OnecikanOkul = true;
            }
            else
            {
                okul.OnecikanOkul = false;
            }

            okulGenericRepository = new GenericRepository<DilOkulu_Okullar>(okulRepository.DBContext);
            DilOkulu_Okullar RetOkul = okulGenericRepository.Update(okul);

            var etiketler = EtiketleriGetir();

            if (RetOkul != null)
            {
                //Eklenmiş etiket varsa ekleniyor.
                if (!string.IsNullOrEmpty(hfEtiketler))
                {
                    etiketler = EtiketleriKaydet(RetOkul.Id, hfEtiketler, etiketler);
                }

                ViewBag.Status = "ok";
            }
            else
            {
                ViewBag.Status = "err";
            }

            ViewBag.Merkezler = MerkezleriGetir();
            ViewBag.Etiketler = etiketler;
            ViewBag.IlislikiEtiketler = IliskiliEtiketleriGetir(etiketler, id);
            ViewBag.GaleriListesi = GaleriListesiGetir();
            ViewBag.SunulanProgramlar = SunulanProgramlariGetir();

            return View(okul);
        }

        private List<DilOkulu_Etiketler> EtiketleriKaydet(int okulId, string gelenEtiketler, List<DilOkulu_Etiketler> etiketler)
        {
            #region Tanimlamalar
            etiketIliskileriRepository = new EtiketIliskileriRepository();
            etiketlerGenericRepository = new GenericRepository<DilOkulu_Etiketler>();
            etiketIliskileriGenericRepository = new GenericRepository<DilOkulu_EtiketIliskileri>();
            #endregion

            #region Json
            string json = @"{""items"":" + gelenEtiketler + @"}";
            var serializer = new JavaScriptSerializer();
            JsonEtiketItems etiketItems = serializer.Deserialize<JsonEtiketItems>(json);
            #endregion

            foreach (var item in etiketItems.items)
            {
                var etiket = etiketler.Find(e => e.Baslik == item.etiket);

                if (item.deleted == false)
                {

                    //Json ile gelen etiket yok ise etiket tablosuna kayıt ediliyor
                    if (etiket == null)
                    {
                        DilOkulu_Etiketler newEtiket = new DilOkulu_Etiketler()
                        {
                            Baslik = item.etiket,
                            Durumu = (int)GeneralVariables.Durum.Aktif,
                            KayitTarihi = DateTime.Now
                        };

                        var retEtiket = etiketlerGenericRepository.Insert(newEtiket);

                        if (retEtiket != null && retEtiket.Id > 0)
                        {
                            //Okul Etiket ilişkisi ekleniyor
                            DilOkulu_EtiketIliskileri etiketIliski = new DilOkulu_EtiketIliskileri()
                            {
                                EtiketId = retEtiket.Id,
                                EtiketTipi = "okul",
                                IcerikId = okulId,
                                Durumu = (int)GeneralVariables.Durum.Aktif,
                                KayitTarihi = DateTime.Now
                            };
                            var iliskiVarMi = Convert.ToBoolean(etiketIliskileriRepository.IliskiMarMi(okulId, retEtiket.Id, "okul"));
                            if (!iliskiVarMi)
                            {
                                etiketIliskileriGenericRepository.Insert(etiketIliski);
                            }

                            //Okul Etiket ilişkisi ekleniyor son.
                        }
                    }
                    else
                    {
                        //Okul Etiket ilişkisi ekleniyor
                        DilOkulu_EtiketIliskileri etiketIliski = new DilOkulu_EtiketIliskileri()
                        {
                            EtiketId = etiket.Id,
                            EtiketTipi = "okul",
                            IcerikId = okulId,
                            Durumu = (int)GeneralVariables.Durum.Aktif,
                            KayitTarihi = DateTime.Now
                        };

                        var iliskiVarMi = Convert.ToBoolean(etiketIliskileriRepository.IliskiMarMi(okulId, etiket.Id, "okul"));

                        if (!iliskiVarMi)
                        {
                            etiketIliskileriGenericRepository.Insert(etiketIliski);
                        }

                        //Okul Etiket ilişkisi ekleniyor son.
                    }
                    //Json ile gelen etiket yok ise kayıt ediliyor son.
                }
                else
                {
                    if (etiket != null)
                    {
                        //Okula ait kayıtlı etiket var ise silinecek
                        var etiketIliski = etiketIliskileriRepository.Liste()
                            .Where(
                            ei =>
                                ei.EtiketId == etiket.Id &&
                                ei.IcerikId == okulId &&
                                ei.EtiketTipi == "okul"
                                )
                                .ToList();

                        if (etiketIliski != null && etiketIliski.Count > 0)
                        {
                            foreach (var itemIliski in etiketIliski)
                            {
                                itemIliski.Durumu = (int)GeneralVariables.Durum.Silindi;
                                etiketIliskileriGenericRepository.Update(itemIliski);
                            }
                        }
                        //Okula ait kayıtlı etiket var ise silinecek son.
                    }

                }
            }

            return EtiketleriGetir();
        }

        private List<DilOkulu_Merkez> MerkezleriGetir()
        {
            merkezRepository = new MerkezRepository();
            var merkezler = merkezRepository.Liste().Where(m => m.Durumu != (int)GeneralVariables.Durum.Silindi).OrderBy(m => m.Baslik).ToList();
            return merkezler;
        }

        private List<DilOkulu_Etiketler> EtiketleriGetir()
        {
            etiketRepository = new EtiketRepository();
            var etiketler = etiketRepository.Liste()
                .Where(e => e.Durumu == (int)GeneralVariables.Durum.Aktif)
                .OrderBy(e => e.Baslik)
                .ToList();

            return etiketler;
        }

        private string IliskiliEtiketleriGetir(List<DilOkulu_Etiketler> etiketler, int okulId)
        {
            if (etiketIliskileriRepository == null)
            {
                etiketIliskileriRepository = new EtiketIliskileriRepository();
            }
            string jsonResult = "";
            List<int> tanimliEtiketler = etiketIliskileriRepository.Liste()
                .Where(
                ei =>
                    ei.EtiketTipi == "okul" &&
                    ei.IcerikId == okulId &&
                    ei.Durumu == (int)GeneralVariables.Durum.Aktif
                    ).Select(ei => ei.EtiketId)
                    .ToList();

            if (tanimliEtiketler != null && tanimliEtiketler.Count > 0)
            {
                var bulunanEtiketler = etiketler.Where(e => tanimliEtiketler.Contains(e.Id));

                List<JsonEtiketItem> jsonItems = new List<JsonEtiketItem>();
                foreach (var item in bulunanEtiketler)
                {
                    jsonItems.Add(new JsonEtiketItem() { etiket = item.Baslik, deleted = false });
                }

                var serializer = new JavaScriptSerializer();

                jsonResult = serializer.Serialize(jsonItems);

            }
            return jsonResult;
        }

        private List<DilOkulu_Galeriler> GaleriListesiGetir()
        {
            galeriRepository = new GaleriRepository();
            List<DilOkulu_Galeriler> galeriler = galeriRepository.Liste().Where(g => g.Durumu == (int)GeneralVariables.Durum.Aktif).Select(g => g).OrderBy(g => g.Baslik).ToList();
            return galeriler;
        }

        private List<DilOkulu_SunulanProgramlar> SunulanProgramlariGetir()
        {
            sunulanProgramRepository = new SunulanProgramRepository();
            var liste = sunulanProgramRepository.Liste().Where(sp => sp.Durumu != (int)GeneralVariables.Durum.Silindi).ToList();
            return liste;
        }
    }
}
