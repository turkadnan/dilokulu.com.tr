using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppV3.Core;
using WebAppV3.Models.Repositories;

namespace WebAppV3.Areas.Service.Controllers
{
    public class DilController : Controller
    {
        // GET: Service/Dil

        #region Variables
        private IDilRepository dilRepository = null;
        #endregion

        public JsonResult index()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu == (byte)GeneralVariables.Durum.Aktif)
                .Select(d => new {d.Id,d.Baslik})
                .OrderBy(d => d.Baslik).ToList() ;
            return Json(diller, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShortList()
        {
            dilRepository = new DilRepository();
            var diller = dilRepository.Liste().Where(d => d.Durumu == (byte)GeneralVariables.Durum.Aktif)
                .Select(d => new { d.Id, d.Baslik })
                .OrderBy(d => d.Baslik).ToList();
            return Json(diller, JsonRequestBehavior.AllowGet);
        }
    }
}