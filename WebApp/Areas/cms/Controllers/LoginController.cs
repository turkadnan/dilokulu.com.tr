using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /cms/Login/
        #region Degiskenler
        private IKullaniciRepository kullaniciRepository = null;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection fCollection)
        {
            string KullaniciAdi = fCollection["KullaniciAdi"];
            string Sifre = fCollection["Sifre"];

            if (!string.IsNullOrEmpty(KullaniciAdi) && !string.IsNullOrEmpty(Sifre))
            {
                kullaniciRepository = new KullaniciRepository();
                var kullanici = kullaniciRepository.Login(KullaniciAdi, Sifre);
                if (kullanici != null)
                {
                    Session["Login"] = kullanici;
                    return RedirectToAction("index", "okullar");
                }
                else
                {
                    ViewBag.AuthStatus = "Kullanıcı Adınızı ya da Şifrenizi kontrol edin!";
                }
            }
            else
            {
                ViewBag.AuthStatus = "Kullanıcı Adınızı ya da Şifrenizi kontrol edin!";
            }
            return View();
        }

        public ActionResult signout()
        {
            Session["Login"] = null;
            return RedirectToAction("index", "login");
        }

    }
}
