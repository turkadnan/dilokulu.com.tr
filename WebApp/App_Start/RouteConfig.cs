using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Core;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            #region Makaleler
            routes.MapRoute(
                   "makaleler_default", // Route name
                   "makaleler/", // URL with parameters
                   new { controller = "makaleler", action = "index", id = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );

            routes.MapRoute(
                   "makaleler_detay", // Route name
                   "makaleler/{title}", // URL with parameters
                   new { controller = "makaleler", action = "detay", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Öğrenci Yorumları
            routes.MapRoute(
                   "ogrenci_yorumlari_default", // Route name
                   "ogrenci-yorumlari/", // URL with parameters
                   new { controller = "OgrenciYorumlari", action = "index", id = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            routes.MapRoute(
                   "ogrenci_yorumlari_detay", // Route name
                   "ogrenci-yorumlari/{title}", // URL with parameters
                   new { controller = "OgrenciYorumlari", action = "detay", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Etiket
            routes.MapRoute(
                   "etiket", // Route name
                   "yurtdisi_dil_egitimi/{baslik}", // URL with parameters
                   new { controller = "etiket", action = "index", baslik = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region ProgramTipi
            routes.MapRoute(
                   "programTipi", // Route name
                   "program-tipi/{baslik}", // URL with parameters
                   new { controller = "ProgramTipi", action = "index", baslik = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Haber
            routes.MapRoute(
                   "haber_Detay", // Route name
                   "haber/{title}", // URL with parameters
                   new { controller = "haberler", action = "detay", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Ülke Rehberi
            routes.MapRoute(
                   "ulke_rehberi", // Route name
                   "{title}-ulke-rehberi", // URL with parameters
                   new { controller = "ulkeRehberi", action = "detay", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Vize Rehberi
            routes.MapRoute(
                   "vize_rehberi", // Route name
                   "{title}-vize-rehberi", // URL with parameters
                   new { controller = "vizeRehberi", action = "detay", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Bilgi istek formu
            routes.MapRoute(
                   "BilgiIstekFormu", // Route name
                   "bilgi-istek-formu", // URL with parameters
                   new { controller = "BilgiIstekFormu", action = "Index", id = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Vize Rehberi
            routes.MapRoute(
                   "fitay_listes", // Route name
                   "fiyat-listesi", // URL with parameters
                   new { controller = "FiyatListesi", action = "index", title = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

            #region Okullar
            routes.MapRoute(
                   "okullar_Ulke", // Route name
                   "{url}-dil-okullari", // URL with parameters
                   new { controller = "Okullar", action = "UlkeSehir", url = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );

            routes.MapRoute(
                   "okullar_DilListe", // Route name
                   "yurtdisi-{url}-dil-egitimi", // URL with parameters
                   new { controller = "Okullar", action = "Dil", url = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );

            routes.MapRoute(
                   "okullar_merkez_detay", // Route name
                   "{url}-dil-okulu", // URL with parameters
                   new { controller = "Okullar", action = "MerkezDetay", url = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );

            routes.MapRoute(
                   "okullar_detay", // Route name
                   "{merkez}-{url}", // URL with parameters
                   new { controller = "Okullar", action = "Detay", merkez = UrlParameter.Optional, url = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );


            #endregion

            #region Anasayfa
            routes.MapRoute(
                   "", // Route name
                   "{controller}/{action}/{id}", // URL with parameters
                   new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                   new string[] { "WebApp.Controllers" }// Parameter defaults
               );
            #endregion

        }
    }
}