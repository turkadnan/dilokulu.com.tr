using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Controllers
{
    [ValidateInput(false)]
    public class EtiketController : Controller
    {
        //
        // GET: /Etiket/

        #region Degiskenler
        private IEtiketRepository etiketRepository = null;
        private IEtiketIliskileriRepository etiketIliskileriRepository = null;
        private OkulRepository okulRepository = null;
        private List<DilOkulu_Okullar> okullar = null;
        #endregion

        [ValidateInput(false)]
        public ActionResult Index(string baslik = "")
        {
            baslik = Server.UrlDecode(baslik);

            ViewBag.Baslik = baslik;

            if (!string.IsNullOrEmpty(baslik))
            {
                etiketRepository = new EtiketRepository();
                var etiket = etiketRepository.Detay(baslik, new int[] { (int)(int)GeneralVariables.Durum.Aktif });

                if (etiket != null)
                {
                    etiketIliskileriRepository = new EtiketIliskileriRepository(etiketRepository.DBContext);
                    List<int> okulIDList = etiketIliskileriRepository.Liste()
                        .Where(
                        ei =>
                            ei.EtiketId == etiket.Id &&
                            ei.Durumu == (int)GeneralVariables.Durum.Aktif &&
                            ei.EtiketTipi == "okul"
                            )
                            .Select(ei => ei.IcerikId)
                            .ToList();
                    if (okulIDList != null && okulIDList.Count > 0)
                    {
                        okulRepository = new OkulRepository();
                        okullar = okulRepository.Liste()
                            .Where(
                            o =>
                                okulIDList.Contains(o.Id) && o.Durumu == (int)GeneralVariables.Durum.Aktif
                                ).ToList();

                    }
                }
            }
            return View(okullar);
        }

    }
}
