using System.Web.Mvc;

namespace WebApp.Areas.cms
{
    public class cmsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "cms";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            //context.Routes.IgnoreRoute("cms/galeri/{resource}.axd");

            context.Routes.IgnoreRoute("cms/galeri/detay/{resource}.axd/{*pathInfo}");

            context.MapRoute(
                "cms_default",
                "cms/{controller}/{action}/{id}",
                new {  controller = "Home",action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
