using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Repositories;
using WebApp.Core;
using WebApp.Models;
using WebApp.Helpers;

namespace WebApp.Controllers
{
    public class PartialController : Controller
    {
        //
        // GET: /Partial/

        #region Degiskenler
        private DilRepository dilRepository = null;
        private IGaleriRepository galeriRepository = null;
        private IMakaleRepository makaleRepository = null;
        private ISliderRepository sliderRepository = null;
        private IVizeRehberiRepository vizeRehberiRepository = null;
        private IUlkeRepository ulkeRepository = null;
        private IEtiketRepository etiketRepository = null;
        private EtiketIliskileriRepository etiketIliskileriRepository = null;
        private OkulRepository okulRepository = null;
        private MerkezRepository merkezRepository = null;
        private AramaViewModel aramaViewModel = null;
        private IStatikLinkRepository statikLinkRepository = null;
        #endregion

        public ActionResult MainMenu()
        {
            #region Vize Rehberi
            vizeRehberiRepository = new VizeRehberiRepository();
            var rehber = vizeRehberiRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                .Select(r => new { r.Id, r.Url, r.Baslik, r.Oncelik })
                .OrderBy(s => s.Oncelik).ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

            ViewBag.VizeRehberi = rehber;
            #endregion

            #region Ülke Rehberi
            ulkeRepository = new UlkeRepository();
            var ulkeler = ulkeRepository.Liste().Where(u => u.Durumu == (int)GeneralVariables.Durum.Aktif)
                .Select(u => new { u.Id, u.Url, u.Baslik, u.Oncelik })
                .OrderBy(u => u.Oncelik).ToList().Select(u => new GeneralListObject { Id = u.Id, Url = u.Url, Baslik = u.Baslik }).ToList<GeneralListObject>();

            ViewBag.UlkeRehberi = ulkeler;
            #endregion

            return PartialView("_MainMenu");
        }

        [OutputCache(Duration = 300)]
        public ActionResult Arama()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu == 1).OrderBy(d => d.Baslik).ToList();

            merkezRepository = new MerkezRepository();
            var merkezler = merkezRepository.Liste().Where(o => o.Durumu == (int)GeneralVariables.Durum.Aktif).OrderBy(o => o.Baslik).ToList();

            aramaViewModel = new AramaViewModel();
            aramaViewModel.Diller = diller;
            aramaViewModel.Merkezler = merkezler;


            return PartialView("_Arama", aramaViewModel);
        }

        public ActionResult AnasayfaAcademixMakaleler()
        {
            makaleRepository = new MakaleRepository();
            var makaleler = makaleRepository.Liste(m => m.Durumu == (int)GeneralVariables.Durum.Aktif && m.KategoriId == (int)GeneralVariables.MakaleKategorileri.Danisman, m => m.KayitTarihi, m => m.DilOkulu_Yazarlar);
            return PartialView("_AnasayfaAcademixMakaleler", makaleler);
        }

        public ActionResult AnasayfaOgrenciMakaleler()
        {
            makaleRepository = new MakaleRepository();
            var makaleler = makaleRepository.Liste(m => m.Durumu == (int)GeneralVariables.Durum.Aktif && m.KategoriId == (int)GeneralVariables.MakaleKategorileri.Ogrenci, m => m.KayitTarihi, m => m.DilOkulu_Yazarlar);
            return PartialView("_AnasayfaOgrenciMakaleler", makaleler);
        }

        public ActionResult AnasayfaEnSonZiyaretEdilenOkullar()
        {
            return PartialView("_AnasayfaEnSonZiyaretEdilenOkullar", CookieHelper.ReadCookie());
        }

        public ActionResult AnasayfaSlider()
        {
            sliderRepository = new SliderRepository();
            var slider = sliderRepository.Liste().Where(m => m.Durumu == (int)GeneralVariables.Durum.Aktif).OrderBy(s => s.Oncelik).ToList();
            return PartialView("_AnasayfaSlider", slider);
        }

        public ActionResult Kategoriler(GeneralVariables.KategoriListeTipi kategoriListeTipi)
        {
            //kategoriRepository = new KategoriRepository();

            //var kategoriler = kategoriRepository.Liste().Where(k => k.Durumu == 1).ToList();
            //switch (kategoriListeTipi)
            //{
            //    case GeneralVariables.KategoriListeTipi.Anasayfa:
            //        return PartialView("_Kategoriler", kategoriler);
            //    case GeneralVariables.KategoriListeTipi.IcerikSayfalari:
            //        return PartialView("_KategorilerIcerik", kategoriler);
            //}
            //return PartialView("_Kategoriler", kategoriler);

            return PartialView("_Kategoriler");
        }

        public ActionResult Etiketler(int okulId = 0)
        {
            List<DilOkulu_Etiketler> etiketler = null;
            if (okulId > 0)
            {
                etiketRepository = new EtiketRepository();
                etiketIliskileriRepository = new EtiketIliskileriRepository(etiketRepository.DBContext);

                var etiketiliskileri = etiketIliskileriRepository.Liste().Where(ie => ie.EtiketTipi == "okul" && ie.IcerikId == okulId).ToList();
                if (etiketiliskileri != null && etiketiliskileri.Count > 0)
                {
                    List<int> iliskiIds = new List<int>();
                    for (int i = 0; i < etiketiliskileri.Count; i++)
                    {
                        iliskiIds.Add(etiketiliskileri[i].EtiketId);                        
                    }
                    if (iliskiIds.Count>0)
                    {
                        etiketler = etiketRepository.Liste().Where(e => e.Durumu == (int)GeneralVariables.Durum.Aktif && iliskiIds.Contains(e.Id)).OrderBy(e => Guid.NewGuid()).ToList();
                    }
                }
            }


            return PartialView("_Etiketler", etiketler);

        }

        [OutputCache(Duration = 300)]
        public ActionResult OnecikanOkullar()
        {
            okulRepository = new OkulRepository();
            var okullar = okulRepository.Liste().Where(o => o.OnecikanOkul == true && o.Durumu == (int)GeneralVariables.Durum.Aktif).ToList();
            return PartialView("_OnecikanOkullar", okullar);
        }

        public ActionResult DigerOkullar(int merkezId = 0)
        {
            merkezRepository = new MerkezRepository();
            var merkezler = merkezRepository.Liste().Where(m => m.Id != merkezId && m.Durumu == (int)GeneralVariables.Durum.Aktif).ToList();
            return PartialView("_DigerOkullar", merkezler);
        }

        public ActionResult ResimGalerisi(int id)
        {
            DilOkulu_Galeriler galeri = null;
            if (true)
            {

            }
            galeriRepository = new GaleriRepository();
            galeri = galeriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });
            return PartialView("_ResimGalerisi", galeri);
        }

        [OutputCache(Duration = 300)]
        public ActionResult StatikLinkler()
        {
            statikLinkRepository = new StatikLinkRepository();
            var linkler = statikLinkRepository.Liste().Where(l => l.Durumu == (int)GeneralVariables.Durum.Aktif).OrderBy(l => l.LinkTipi).ThenBy(l => l.Oncelik).ToList();
            return PartialView("_StatikLinkler", linkler);
        }

        public ActionResult EnSonZiyaretEdilenOkullar()
        {
            return PartialView("_EnSonZiyaretEdilenOkullar", CookieHelper.ReadCookie());
        }
    }
}
