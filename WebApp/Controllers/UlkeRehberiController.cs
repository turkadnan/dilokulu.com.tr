using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class UlkeRehberiController : Controller
    {
        //
        // GET: /UlkeRehberi/

        #region Degiskenler
        private IUlkeRepository ulkeRepository = null;
        private OkulRepository okulRepository = null;
        #endregion

        public ActionResult detay(string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                ulkeRepository = new UlkeRepository();
                okulRepository = new OkulRepository(ulkeRepository.DBContext);

                var ulke = ulkeRepository.Detay(title, new int[] { (int)GeneralVariables.Durum.Aktif });
                if (ulke != null)
                {
                    var okullar = okulRepository.Liste().Where(o => o.UlkeId == ulke.Id && o.Durumu == (int)GeneralVariables.Durum.Aktif).ToList();

                    ViewBag.Okullar = okullar;
                }


                return View(ulke);
            }
            else
            {
                return View();
            }

        }

    }
}
