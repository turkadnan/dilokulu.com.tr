using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class XMLController : Controller
    {
        //
        // GET: /XML/

        #region Variables
        private List<IXMLSitemapItem> sitemapItems = null;
        private IDilRepository dilRepository = null;
        private IMakaleRepository makaleRepository = null;
        private IHaberRepository haberRepository = null;
        private IVizeRehberiRepository vizeRehberiRepository = null;
        private IUlkeRepository ulkeRepository = null;
        private ISehirRepository sehirRepository = null;
        private IOkulRepository okulRepository = null;
        private IMerkezRepository merkezRepository = null;
        private IStatikLinkRepository statikLinkRepository = null;
        private string siteDomain = ConfigurationManager.AppSettings["SiteDomain"].ToString();
        #endregion

        [HandleError]
        public XMLSitemapResult Sitemap()
        {
            #region Instance
            sitemapItems = new List<IXMLSitemapItem>();
            makaleRepository = new MakaleRepository();
            haberRepository = new HaberRepository();
            vizeRehberiRepository = new VizeRehberiRepository();
            ulkeRepository = new UlkeRepository();
            sehirRepository = new SehirRepository();
            dilRepository = new DilRepository();
            merkezRepository = new MerkezRepository();
            okulRepository = new OkulRepository();
            statikLinkRepository = new StatikLinkRepository();
            #endregion

            string retUrl = "";

            #region Merkez Listesi
            try
            {
                var okulMerkezleri = merkezRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik })
                    .ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

                foreach (var item in okulMerkezleri)
                {
                    retUrl = siteDomain + item.Url + "-dil-okulu";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }

            }
            catch (Exception)
            { }
            #endregion

            #region Okul Listesi
            try
            {
                var okulllar= okulRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik })
                    .ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

                foreach (var item in okulllar)
                {
                    retUrl = siteDomain + item.Url;
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }

            }
            catch (Exception)
            { }
            #endregion

            #region Yurt Dışı <Sehir> dil eğitimi
            try
            {
                var sehirler = sehirRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik })
                    .ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

                foreach (var item in sehirler)
                {
                    retUrl = siteDomain + item.Url + "-dil-okullari";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }

            }
            catch (Exception)
            { }
            #endregion

            #region Yurt Dışı <language> dil eğitimi
            try
            {
                var diller = dilRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik })
                    .ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

                foreach (var item in diller)
                {
                    retUrl = siteDomain + "yurtdisi-" + item.Url + "-dil-egitimi";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }

            }
            catch (Exception)
            { }
            #endregion

            #region Dil Okulları ve Ülke Rehberi
            try
            {
                var ulkeRehberi = ulkeRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik, r.Oncelik })
                    .OrderBy(s => s.Oncelik).ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();

                //Dil Okulları Rehberi
                foreach (var item in ulkeRehberi)
                {
                    retUrl = siteDomain + item.Url + "-dil-okullari";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }

                //Ülke Rehberi
                foreach (var item in ulkeRehberi)
                {
                    retUrl = siteDomain + item.Url + "-ulke-rehberi";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }
            #endregion

            #region Vize Rehberi
            try
            {
                var vizeRehber = vizeRehberiRepository.Liste().Where(r => r.Durumu == (int)GeneralVariables.Durum.Aktif)
                    .Select(r => new { r.Id, r.Url, r.Baslik, r.Oncelik })
                    .OrderBy(s => s.Oncelik).ToList().Select(r => new GeneralListObject { Id = r.Id, Url = r.Url, Baslik = r.Baslik }).ToList<GeneralListObject>();
                foreach (var item in vizeRehber)
                {
                    retUrl = siteDomain + item.Url + "-vize-rehberi";
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }
            #endregion

            #region Haberler
            try
            {
                var haberler = haberRepository.Liste().Where(h => h.Durumu == 1).OrderBy(h => h.Oncelik).ToList();
                foreach (var item in haberler)
                {
                    retUrl = siteDomain + "haber/" + item.Url;
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }
            #endregion

            #region Dilokulu Makaleler
            try
            {
                var ogrenciMakaleleri = makaleRepository.Liste(m => m.Durumu == (int)GeneralVariables.Durum.Aktif && m.KategoriId == (int)GeneralVariables.MakaleKategorileri.Danisman, m => m.KayitTarihi, m => m.DilOkulu_Yazarlar);
                foreach (var item in ogrenciMakaleleri)
                {
                    retUrl = siteDomain + "ogrenci-yorumlari/" + item.Url;
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }

            #endregion

            #region Öğrenci Makaleleri
            try
            {
                var dilokuluMakaleler = makaleRepository.Liste(m => m.Durumu == (int)GeneralVariables.Durum.Aktif && m.KategoriId == (int)GeneralVariables.MakaleKategorileri.Ogrenci, m => m.KayitTarihi, m => m.DilOkulu_Yazarlar);
                foreach (var item in dilokuluMakaleler)
                {
                    retUrl = siteDomain + "makaleler/" + item.Url;
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }

            #endregion

            #region Statik oluşturulan linker
            try
            {

                var linkler = statikLinkRepository.Liste().Where(l => l.Durumu == (int)GeneralVariables.Durum.Aktif).OrderBy(l => l.LinkTipi).ThenBy(l => l.Oncelik).ToList();
                foreach (var item in linkler)
                {
                    retUrl = (siteDomain + item.Link).Replace("//", "/").Replace("http:/d", "http://d");
                    sitemapItems.Add(new XMLSitemapItem(retUrl) { });
                }
            }
            catch (Exception)
            { }
            #endregion

            #region Statik Sayfalar
            sitemapItems.Add(new XMLSitemapItem(siteDomain + "fiyat-listesi") { });
            sitemapItems.Add(new XMLSitemapItem(siteDomain + "iletisim") { });
            sitemapItems.Add(new XMLSitemapItem(siteDomain + "bilgi-istek-formu") { });
            sitemapItems.Add(new XMLSitemapItem(siteDomain + "hakkimizda") { });
            #endregion

            return new XMLSitemapResult(sitemapItems);
        }

    }
}
