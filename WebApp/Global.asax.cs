using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using WebApp.Core;

namespace WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private string siteDomain = ConfigurationManager.AppSettings["SiteDomain"].ToString();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            string PageUrl = Request.RawUrl;

            //Security control begin
            if (PageUrl.ToLower().Contains("="))
            {

                string TempUrl = Server.UrlDecode(PageUrl);
                string[] dennyChars = new string[] { "'", "\"", "<", ">" };
                foreach (var item in dennyChars)
                {
                    if (TempUrl.Contains(item))
                    {
                        Response.Redirect("http://dilokulu.com.tr/");
                    }
                }
            }
            //Security control end
            
            string TargetPage = RedirectPagesWith301(HttpContext.Current.Request.Url.ToString().ToLower());

            if (TargetPage.Length > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.StatusCode = 301;
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", TargetPage);
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_Error()
        {

            //var exception = Server.GetLastError();
            //var httpException = exception as HttpException;
            //Response.Clear();
            //Server.ClearError();
            //var routeData = new RouteData();
            //routeData.Values["controller"] = "pagenotfound";
            //routeData.Values["action"] = "index";
            //routeData.Values["exception"] = exception;
            //Response.StatusCode = 500;

            //if (httpException != null)
            //{
            //    Response.StatusCode = httpException.GetHttpCode();
            //    switch (Response.StatusCode)
            //    {
            //        case 403:
            //            routeData.Values["action"] = "index";
            //            break;
            //        case 404:
            //            routeData.Values["action"] = "index";
            //            break;
            //    }
            //}


            //var errorMessage = exception.Message.ToLower();
            //Response.Redirect("~/pagenotfound");
             

        }

        private string RedirectPagesWith301(string LocalPath)
        {
            System.Collections.Generic.Dictionary<string, string> dicUrl = new System.Collections.Generic.Dictionary<string, string>();
            /**/
            dicUrl.Add(siteDomain + "vize-rehberi/hollanda-vizesi", siteDomain);
            dicUrl.Add(siteDomain + "ulkeler-rehberi", siteDomain);
            dicUrl.Add(siteDomain + "dil-testi", siteDomain + "DilTesti");
            dicUrl.Add(siteDomain + "vize-rehberi", siteDomain + "ingiltere-vize-rehberi");
            dicUrl.Add(siteDomain + "dil-testi/en", siteDomain + "DilTesti");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/avusturalya", siteDomain + "avustralya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/yeni-zelanda", siteDomain + "yeni-zelanda-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/sili", siteDomain + "sili-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/rusya", siteDomain + "rusya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/portekiz", siteDomain + "portekiz-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/peru", siteDomain + "peru-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/meksika", siteDomain + "meksika-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/malta", siteDomain + "malta-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/kuba", siteDomain + "kuba-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/kosta-rika", siteDomain + "kosta-rika-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/kanada", siteDomain + "kanada-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/japonya", siteDomain + "japonya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/italya", siteDomain + "italya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/ispanya", siteDomain + "ispanya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/iskocya", siteDomain + "iskocya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/irlanda", siteDomain + "irlanda-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/ingiltere", siteDomain + "ingiltere-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/guney-afrika", siteDomain + "guney-afrika-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/fransa", siteDomain + "fransa-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/fas", siteDomain + "fas-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/cin", siteDomain + "cin-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/brezilya", siteDomain + "brezilya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/arjantin", siteDomain + "arjantin-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/amerika", siteDomain + "amerika-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/almanya", siteDomain + "almanya-ulke-rehberi");
            dicUrl.Add(siteDomain + "ulkeler-rehberi/isvicre", siteDomain + "isvicre-ulke-rehberi");
            dicUrl.Add(siteDomain + "vize-rehberi/italya-vizesi", siteDomain + "italya-vize-rehberi");
            dicUrl.Add(siteDomain + "vize-rehberi/ingiltere-vizesi", siteDomain + "ingiltere-vize-rehberi");
            dicUrl.Add(siteDomain + "vize-rehberi/amerika-vizesi", siteDomain + "amerika-vize-rehberi");
            dicUrl.Add(siteDomain + "hemen-bilgi-al", siteDomain + "bilgi-istek-formu");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/alicante", siteDomain + "alicante-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa/amboise", siteDomain + "amboise-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/guatemala/antigua", siteDomain + "antigua-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/arkansas", siteDomain + "arkansas-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/yeni-zelanda/auckland", siteDomain + "auckland-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/barcelona", siteDomain + "barcelona-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/arjantin/bariloche", siteDomain + "bariloche-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/bath", siteDomain + "bath-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/belfast", siteDomain + "belfast-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/berlin", siteDomain + "berlin-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/boca-raton", siteDomain + "boca-raton-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/boston", siteDomain + "boston-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/bournemouth", siteDomain + "bournemouth-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/brighton", siteDomain + "brighton-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/brisbane", siteDomain + "brisbane-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/bristol", siteDomain + "bristol-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/arjantin/buenos-aires", siteDomain + "buones-aires-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/byron-bay", siteDomain + "byron-bay-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/cadiz", siteDomain + "cadiz-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/cairns", siteDomain + "cairns-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/calgary", siteDomain + "calgary-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/california", siteDomain + "california-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/cambridge", siteDomain + "cambridge-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/canterbury", siteDomain + "canterbury-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/guney-afrika/cape-town", siteDomain + "cape-town-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/isvicre/cenova", siteDomain + "cenova-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/chester", siteDomain + "chester-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/chicago", siteDomain + "chicago-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/chichester", siteDomain + "chichester-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/yeni-zelanda/christchurch", siteDomain + "christchurch-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/colorado", siteDomain + "colorado-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/connecticut", siteDomain + "connecticut-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/arjantin/cordoba", siteDomain + "cordoba-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/costa-rica/coronado", siteDomain + "coronado-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/peru/cusco", siteDomain + "cusco-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/devon", siteDomain + "devon-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/district-of-columbia", siteDomain + "district-of-columbia-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/irlanda/dublin", siteDomain + "dublin-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/dusseldorf", siteDomain + "dusseldorf-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/eastbourne", siteDomain + "eastbourne-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/iskocya/edinburg", siteDomain + "edinburgh-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/edinburgh", siteDomain + "edinburgh-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/exeter", siteDomain + "exeter-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/costa-rica/flamingo-beach", siteDomain + "flamingo-beach-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/florence", siteDomain + "florence-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/florida", siteDomain + "florida-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/fort-lauderdale", siteDomain + "fort-lauderdale-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/frankfurt", siteDomain + "frankfurt-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa", siteDomain + "fransa-dil-okullari");
            dicUrl.Add(siteDomain + "genel", siteDomain);
            dicUrl.Add(siteDomain + "ingilizce/amerika/georgia", siteDomain + "georgia-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/malta/gozzo", siteDomain + "gozzo-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/granada", siteDomain + "granada-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/meksika/guanajuato", siteDomain + "guanajuato-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/halifax", siteDomain + "halifax-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/hamburg", siteDomain + "hamburg-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/hampstead", siteDomain + "hampstead-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/hastings", siteDomain + "hastings-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/kuba/havanna", siteDomain + "havanna-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/hawaii", siteDomain + "hawaii-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/costa-rica/heredia", siteDomain + "heredia-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/idaho", siteDomain + "idaho-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/illinois", siteDomain + "illinois-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/indiana", siteDomain + "indiana-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce", siteDomain);
            dicUrl.Add(siteDomain + "ingilizce/ingiltere", siteDomain + "ingiltere-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/irvine", siteDomain + "irvine-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya", siteDomain + "italya-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca", siteDomain + "yurtdisi-italyanca-dil-egitimi");
            dicUrl.Add(siteDomain + "ingilizce/guney-afrika/johannesburg", siteDomain + "johannesburg-dil-okullari");
            dicUrl.Add(siteDomain + "arapca/misir/kahire", siteDomain + "kahire-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/kalabrien", siteDomain + "kalabrien-dil-okullari");
            dicUrl.Add(siteDomain + "japonca/japonya/kanazawa", siteDomain + "kanazawa-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/kent", siteDomain + "kent-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/kingston", siteDomain + "kingston-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa/la-rochella", siteDomain + "la-rochella-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/las-vegas", siteDomain + "las-vegas-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/leeds", siteDomain + "leeds-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/lignano", siteDomain + "lignano-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/liverpool", siteDomain + "liverpool-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/livorno", siteDomain + "livorno-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/lleide-pyreness", siteDomain + "lleide-pyreness-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/londra", siteDomain + "londra-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/los-angeles", siteDomain + "los-angeles-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/louisiana", siteDomain + "louisiana-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/isvicre/lozan", siteDomain + "lozan-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/madrid", siteDomain + "madrid-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/malaga", siteDomain + "malaga-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/mallorca", siteDomain + "mallorca-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/manchester", siteDomain + "manchester-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/marbella", siteDomain + "marbella-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/massachusetts", siteDomain + "massachusetts-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/melbourne", siteDomain + "melbourne-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/arjantin/mendoza", siteDomain + "mendozza-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/miami", siteDomain + "miami-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/michigan", siteDomain + "michigan-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/milan", siteDomain + "milan-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/minnesota", siteDomain + "minnesota-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/missouri", siteDomain + "missouri-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/costa-rica/monteverde", siteDomain + "monteverde-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/montreal", siteDomain + "montreal-dil-okullari");
            dicUrl.Add(siteDomain + "rusca/rusya/moskova", siteDomain + "moskova-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/munich", siteDomain + "munich-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/nevada", siteDomain + "nevada-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/new-castle", siteDomain + "newcastle-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/new-hampshire", siteDomain + "new-hampshire-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/new-jersey", siteDomain + "new-jersey-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/new-york", siteDomain + "new-york-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa/nice", siteDomain + "nice-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/noosa", siteDomain + "noosa-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/north-carolina", siteDomain + "north-carolina-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/north-dakota", siteDomain + "north-dakota-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/norwich", siteDomain + "norwich-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/meksika/oaxaca", siteDomain + "oaxaca-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/ohio", siteDomain + "ohio-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/oklahoma", siteDomain + "oklahama-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/oregon", siteDomain + "oregon-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/ottawa", siteDomain + "ottowa-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/oxford", siteDomain + "oxford-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/pamplona", siteDomain + "pamplona-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa/paris", siteDomain + "paris-dil-okullari");
            dicUrl.Add(siteDomain + "cince/cin/pekin", siteDomain + "pekin-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/pennslyvania", siteDomain + "pennslyvania-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/perth", siteDomain + "perth-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/philadelphia", siteDomain + "philadelphia-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/meksika/playa-del-carmen", siteDomain + "playa-del-carmen-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/costa-rica/playa-jaco", siteDomain + "playa-jaco-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/portland", siteDomain + "portland-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/portsmouth", siteDomain + "portsmouth-dil-okullari");
            dicUrl.Add(siteDomain + "promosyonlar", siteDomain + "fiyat-listesi");
            dicUrl.Add(siteDomain + "ispanyolca/meksika/puerto-vallarta", siteDomain + "puerto-vallarta-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ekvator/quito", siteDomain + "quito-dil-okullari");
            dicUrl.Add(siteDomain + "arapca/fas/rabat", siteDomain + "rabat-dil-okullari");
            dicUrl.Add(siteDomain + "almanca/almanya/radolfzell", siteDomain + "radolfzell-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/rhode-island", siteDomain + "rhode-island-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/roma", siteDomain + "roma-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/salamanca", siteDomain + "salamanca-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/salisbury", siteDomain + "salisbury-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/san-diego", siteDomain + "san-diego-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/san-francisco", siteDomain + "san-francisco-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/kosta-rika/san-jose", siteDomain + "san-jose-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/santa-barbara", siteDomain + "santa-barbara-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/dominik-cumhuriyeti/santa-domingo", siteDomain + "santa-domingo-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/kuba/santiago", siteDomain + "santiago-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/sili/santiago-de-chile", siteDomain + "santiago-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/seattle", siteDomain + "seattle-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/sevilla", siteDomain + "sevilla-dil-okullari");
            dicUrl.Add(siteDomain + "italyanca/italya/siena", siteDomain + "siena-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/malta/sliema", siteDomain + "sliema-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/dominik-cumhuriyeti/sosua", siteDomain + "sosua-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/south-carolina", siteDomain + "south-carolina-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/malta/st-julians", siteDomain + "st-julians-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/malta/st-pauls-bay", siteDomain + "st-pauls-bay-dil-okullari");
            dicUrl.Add(siteDomain + "rusca/rusya/st-petersburg", siteDomain + "st-petersburg-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/bolivia/sucre", siteDomain + "sucre-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/surfers-paradise", siteDomain + "surfers-paradise-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/surrey", siteDomain + "surrey-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/avustralya/sydney", siteDomain + "sydney-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/taunton", siteDomain + "taunton-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/tavistock", siteDomain + "tavistock-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/tenerife", siteDomain + "tenerife-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/tennessee", siteDomain + "tennessee-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/texas", siteDomain + "texas-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/torbay", siteDomain + "torbay-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/toronto", siteDomain + "toronto-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/torquay", siteDomain + "torquay-dil-okullari");
            dicUrl.Add(siteDomain + "ispanyolca/kuba/trinidad", siteDomain + "trinidad-dil-okullari");
            dicUrl.Add(siteDomain + "uncategorized", siteDomain);
            dicUrl.Add(siteDomain + "ispanyolca/ispanya/valencia", siteDomain + "valencia-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/vancouver", siteDomain + "vancouver-dil-okullari");
            dicUrl.Add(siteDomain + "fransizca/fransa/vichy", siteDomain + "vichy-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/kanada/victoria", siteDomain + "victoria-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/virginia", siteDomain + "virginia-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/washington-d-c", siteDomain + "washington-dc-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/winchester", siteDomain + "winchester-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/wisconsin", siteDomain + "wisconsin-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/amerika/wyoming", siteDomain + "wyoming-dil-okullari");
            dicUrl.Add(siteDomain + "ingilizce/ingiltere/york", siteDomain + "york-dil-okullari");
            dicUrl.Add(siteDomain + "haberler", siteDomain);
            dicUrl.Add(siteDomain + "fransizca/isvicre/zurih", siteDomain + "zurih-dil-okullari");
            dicUrl.Add(siteDomain + "makaleler/butceye-uygun-dil-okulu-nasil-secilir", siteDomain);
            string Temp = "";
            foreach (var item in dicUrl)
            {
                if (LocalPath.Contains(item.Key.ToLower()))
                {
                    Temp = item.Value;
                    break;
                }
            }

            return Temp;
        }
    }
}