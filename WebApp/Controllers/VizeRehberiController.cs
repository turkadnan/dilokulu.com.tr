using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    public class VizeRehberiController : Controller
    {
        //
        // GET: /VizeRehberi/

        #region Degiskenler
        private IVizeRehberiRepository vizeRehberiRepository = null;
        #endregion

        public ActionResult detay(string title = "")
        {
            if (!string.IsNullOrEmpty(title))
            {
                vizeRehberiRepository = new VizeRehberiRepository();
                var rehber = vizeRehberiRepository.Detay(title, new int[] { (int)GeneralVariables.Durum.Aktif });
                return View(rehber);
            }
            else
            {
                return View();
            }

        }

    }
}
