using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class GaleriController : BaseController
    {
        //
        // GET: /cms/Galeri/
        #region Degiskenler
        private IGaleriRepository galeriRepository = null;
        private GenericRepository<DilOkulu_Galeriler> galeriGenericRepository = new GenericRepository<DilOkulu_Galeriler>();
        private GenericRepository<DilOkulu_GaleriDosyalari> galeriDosyalariGenericRepository = new GenericRepository<DilOkulu_GaleriDosyalari>();
        #endregion

        public ActionResult Index()
        {
            galeriRepository = new GaleriRepository();
            var galeriler = galeriRepository.Liste().Where(g => g.Durumu == (int)GeneralVariables.Durum.Aktif).ToList().OrderBy(g => g.Baslik).ToList();
            return View(galeriler);
        }

        public ActionResult yeni()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult yeni(FormCollection fColl)
        {
            #region Form Collection
            string baslik = fColl["Baslik"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion

            if (!string.IsNullOrEmpty(baslik))
            {
                DilOkulu_Galeriler galeri = new DilOkulu_Galeriler()
                {
                    Baslik = baslik.Trim(),
                    Durumu = durumu,
                    KayitTarihi = DateTime.Now
                };

                galeriGenericRepository = new GenericRepository<DilOkulu_Galeriler>();
                var retGaleri = galeriGenericRepository.Insert(galeri);
                if (retGaleri != null)
                {
                    ViewBag.Status = "ok";
                    return RedirectToAction("detay", "galeri", new { id = retGaleri.Id });
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            else
            {
                ViewBag.Status = "err";
            }

            return View();
        }

        public ActionResult Detay(int id = 0)
        {
            string resimler = "";
            DilOkulu_Galeriler galeri = null;
            if (id > 0)
            {
                galeriRepository = new GaleriRepository();
                galeri = galeriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });

                if (galeri.DilOkulu_GaleriDosyalari != null && galeri.DilOkulu_GaleriDosyalari.Count > 0)
                {
                    var galeriDosyalari = galeri.DilOkulu_GaleriDosyalari.Where(gd => gd.Durumu == (int)GeneralVariables.Durum.Aktif);
                    foreach (var dosyalar in galeriDosyalari)
                    {
                        resimler += Path.GetFileName(dosyalar.DosyaPath) + ";";
                    }

                    resimler = resimler.TrimEnd(';');
                }

            }

            ViewBag.PhotoNames = resimler;
            return View(galeri);
        }

        [HttpPost]
        public ActionResult Detay(FormCollection fColl)
        {
            #region Form Collection
            int id = Convert.ToInt32(fColl["Id"]);
            string baslik = fColl["Baslik"];
            string hfPhotoNames = fColl["hfPhotoNames"].TrimEnd(';').TrimStart(';');
            string hfRemovePhotoNames = fColl["hfRemovePhotoNames"].TrimEnd(';').TrimStart(';');
            string hfGuncellemeDurum = fColl["hfGuncellemeDurum"];
            byte durumu = Convert.ToByte(fColl["selectDurum"]);
            #endregion
            galeriRepository = new GaleriRepository();
            galeriDosyalariGenericRepository = new GenericRepository<DilOkulu_GaleriDosyalari>(galeriRepository.DBContext);
            galeriGenericRepository = new GenericRepository<DilOkulu_Galeriler>(galeriRepository.DBContext);

            var galeri = galeriRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Aktif });
            if (galeri != null)
            {
                galeri.Baslik = baslik;
                galeri.Durumu = durumu;
                var retGaleri = galeriGenericRepository.Update(galeri);

                if (retGaleri != null)
                {
                    #region Resim İşlemleri
                    if (galeri.DilOkulu_GaleriDosyalari != null && galeri.DilOkulu_GaleriDosyalari.Count > 0)
                    {
                        foreach (var item in galeri.DilOkulu_GaleriDosyalari)
                        {
                            item.Durumu = (int)GeneralVariables.Durum.Silindi;
                            galeriDosyalariGenericRepository.Update(item);
                        }

                    }

                    int Counter = 0;
                    string Find = "";
                    string[] arrFile = hfPhotoNames.Split(';');
                    string[] arrRemoveFile = hfRemovePhotoNames.Split(';');
                    string NewFileList = "";

                    for (var i = 0; i < arrFile.Length; i++)
                    {

                        if (arrRemoveFile.Length > 0)
                        {
                            for (var r = 0; r < arrRemoveFile.Length; r++)
                            {
                                if (arrFile[i] == arrRemoveFile[r])
                                {
                                    Find = arrRemoveFile[r];
                                }
                            }
                        }

                        if (Find == arrFile[i])
                        {
                            Find = "";
                        }
                        else
                        {
                            Counter++;
                            DilOkulu_GaleriDosyalari galeriDosyalari = new DilOkulu_GaleriDosyalari();
                            galeriDosyalari.GaleriId = id;
                            galeriDosyalari.DosyaPath = "content/galeri/" + arrFile[i];
                            galeriDosyalari.KayitTarihi = DateTime.Now;
                            galeriDosyalari.Durumu = (int)GeneralVariables.Durum.Aktif;

                            var retDosya = galeriDosyalariGenericRepository.Insert(galeriDosyalari);
                            galeriDosyalari = null;
                            NewFileList += arrFile[i] + ";";
                        }
                    }
                    #endregion

                    ViewBag.PhotoNames = NewFileList;
                    ViewBag.GuncellemeDurum = "true";
                    ViewBag.Status = "ok";
                }
                else
                {
                    ViewBag.Status = "err";
                }
            }
            return View(galeri);
        }

    }
}
