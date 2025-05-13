using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class SubeController : BaseController
    {
        //
        // GET: /cms/Sube/

        #region Degiskenler
        private ISubeRepository subeRepository = null;
        private GenericRepository<DilOkulu_Subeler> subeGenericRepository = null;
        private SubeListViewModel subeListViewModel = null;
        #endregion

        public ActionResult Index()
        {
            subeRepository = new SubeRepository();

            subeListViewModel = new SubeListViewModel();

            subeListViewModel.SubelerShortList = subeRepository.Liste().Where(s => s.Durumu != 3)
                .Select(s => new { s.Id, s.Baslik, s.Oncelik }).OrderBy(s => s.Oncelik).ToList()
                .Select(s => new KeyValuePair<int, string>(s.Id, s.Baslik));

            subeListViewModel.Sube = new DilOkulu_Subeler();
            subeListViewModel.Sube.Oncelik = 0;
            subeListViewModel.Sube.Durumu = 5;//Sallama bir sayı :)

            return View(subeListViewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(FormCollection fColl)
        {
            #region Form Collection
            int id = Convert.ToInt32("0" + fColl["SubeId"]);
            #endregion

            KayitGuncellemeIslemleri(fColl);

            #region GetAllList
            subeRepository = new SubeRepository();

            subeListViewModel = new SubeListViewModel();

            subeListViewModel.SubelerShortList = subeRepository.Liste().Where(s => s.Durumu != 3)
                .Select(s => new { s.Id, s.Baslik, s.Oncelik }).OrderBy(s => s.Oncelik).ToList()
                .Select(s => new KeyValuePair<int, string>(s.Id, s.Baslik));

            subeListViewModel.SubeSelectedValue = id;

            subeListViewModel.Sube = new DilOkulu_Subeler();
            if (id == 0)
            {

                subeListViewModel.Sube.Oncelik = 0;
                subeListViewModel.Sube.Durumu = 5;//Sallama bir sayı :)    
            }
            else
            {
                var sube = subeRepository.Detay(id, new int[] { 1, 2 });
                if (sube != null)
                {
                    subeListViewModel.Sube = sube;
                }

            }
            #endregion

            return View(subeListViewModel);
        }

        private void KayitGuncellemeIslemleri(FormCollection fColl)
        {

            #region Form Collection
            int id = Convert.ToInt32("0" + fColl["SubeId"]);
            string baslik = fColl["Baslik"].ToString();
            string ePosta = fColl["EPosta"].ToString();
            string telefon = fColl["Telefon"].ToString();
            string fax = fColl["Fax"].ToString();
            string adres = fColl["Adres"].ToString();
            string icerik = fColl["hfIcerik"];
            string krokiLink = fColl["KrokiLink"].ToString();
            string islem = fColl["Islem"].ToString();
            int oncelik = 0;
            int.TryParse(fColl["Oncelik"].ToString(), out oncelik);
            byte durumu = Convert.ToByte("0" + fColl["selectDurum"]);
            #endregion

            subeRepository = new SubeRepository();
            subeGenericRepository = new GenericRepository<DilOkulu_Subeler>(subeRepository.DBContext);

            if (!string.IsNullOrEmpty(islem))
            {
                switch (islem)
                {
                    case "new":
                        DilOkulu_Subeler yeniSube = new DilOkulu_Subeler();
                        yeniSube.Baslik = baslik;
                        yeniSube.EPosta = ePosta;
                        yeniSube.Telefon = telefon;
                        yeniSube.Fax = fax;
                        yeniSube.Adres = adres;
                        yeniSube.Icerik = icerik;
                        yeniSube.KrokiLink = krokiLink;
                        yeniSube.KayitTarihi = DateTime.Now;
                        yeniSube.Oncelik = oncelik;
                        yeniSube.Durumu = durumu;

                        var retInsert = subeGenericRepository.Insert(yeniSube);

                        if (retInsert != null)
                        {
                            ViewBag.Status = "ok";
                        }
                        else
                        {
                            ViewBag.Status = "err";
                        }

                        break;
                    case "update":
                        var sube = subeRepository.Detay(id, new int[] { 1, 2 });

                        sube.Baslik = baslik;
                        sube.EPosta = ePosta;
                        sube.Telefon = telefon;
                        sube.Fax = fax;
                        sube.Adres = adres;
                        sube.KrokiLink = krokiLink;
                        sube.Icerik = icerik;
                        sube.Oncelik = oncelik;
                        sube.Durumu = durumu;

                        var retUpdate = subeGenericRepository.Update(sube);
                        if (retUpdate != null)
                        {
                            ViewBag.Status = "ok";
                        }
                        else
                        {
                            ViewBag.Status = "err";
                        }

                        break;
                }
            }
        }

        public ActionResult Sayfalar()
        {


            return View();
        }
    }
}
