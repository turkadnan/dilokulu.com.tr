using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Helpers
{
    public static class CookieHelper
    {
        public static void WriteCookie(string okul)
        {
            HttpCookie dilokuluCookie = HttpContext.Current.Request.Cookies.Get("dilokuluGezinti");
            if (dilokuluCookie == null)
            {
                dilokuluCookie = new HttpCookie("dilokuluGezinti");
                dilokuluCookie.Value = okul;
                dilokuluCookie.Expires = DateTime.Now.AddDays(1);
                HttpContext.Current.Response.Cookies.Set(dilokuluCookie);
            }
            else
            {
                if (dilokuluCookie.Value.IndexOf(okul) == -1)
                {
                    dilokuluCookie.Value = okul + "|" + dilokuluCookie.Value;
                    dilokuluCookie.Expires = DateTime.Now.AddDays(1);
                    HttpContext.Current.Response.Cookies.Set(dilokuluCookie);
                }
            }
        }

        public static string[] ReadCookie()
        {
            HttpCookie dilokuluCookie = HttpContext.Current.Request.Cookies.Get("dilokuluGezinti");
            if (dilokuluCookie != null && !string.IsNullOrEmpty(dilokuluCookie.Value))
            {
                return dilokuluCookie.Value.Split('|');
            }
            else
            {
                return null;
            }
        }
    }
}