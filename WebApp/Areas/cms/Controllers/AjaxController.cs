using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Core;
using WebApp.Models;
using WebApp.Models.Repositories;

namespace WebApp.Areas.cms.Controllers
{
    public class AjaxController : Controller
    {
        //
        // GET: /cms/Ajax/

        #region Degiskernler
        private IDilRepository dilRepository = null;
        private IUlkeRepository ulkeRepository = null;
        private ISehirRepository sehirRepository = null;
        private IMerkezRepository merkezRepository = null;
        private IOkulRepository okulRepository = null;
        private IYazarRepository yazarRepository = null;
        private IMakaleRepository makaleRepository = null;
        private IHaberRepository haberRepository = null;
        private ISliderRepository sliderRepository = null;
        private ISunulanProgramRepository sunulanProgramRepository = null;
        private IGaleriRepository galeriRepository = null;
        private IStatikLinkRepository statikLinkRepository = null;
        private GenericRepository<DilOkulu_StatikLinkler> statikLinklerGenericRepository = null;
        private GenericRepository<DilOkulu_Slider> sliderGenericRepository = null;
        #endregion

        [HttpPost]
        public ActionResult DilVarmi()
        {
            var Baslik = Request["Baslik"];
            string Mesaj = "";
            string Status = "";
            if (!String.IsNullOrEmpty(Baslik.Trim()))
            {
                dilRepository = new DilRepository();

                bool? durum = dilRepository.DilMarMi(Baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Dil daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Bir dil girmelisiniz!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UlkeVarMi()
        {
            var Baslik = Request["Baslik"];
            var DilId = Convert.ToInt32("0" + Request["DilId"]);
            var ParaBirimleri = Tools.IsInteger(Request["ParaBirimleri"]) ? Tools.ParseNullableInt(Request["ParaBirimleri"]) : null;
            var VizeUcreti1 = Tools.IsDecimal(Request["VizeUcreti1"]) ? Convert.ToDecimal(Request["VizeUcreti1"]) : Convert.ToDecimal(0);
            var KacAyaKadar1 = Tools.IsInteger(Request["KacAyaKadar1"]) ? Tools.ParseNullableInt(Request["KacAyaKadar1"]) : null;

            string Mesaj = "";
            string Status = "";
            if (!String.IsNullOrEmpty(Baslik.Trim()) && DilId != 0 && ParaBirimleri != null && VizeUcreti1 > 0 && KacAyaKadar1 > 0)
            {
                ulkeRepository = new UlkeRepository();

                bool? durum = ulkeRepository.UlkeMarMi(Baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Ülke daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Bir ülke adı, dil, Para birimi, Vize ücreti 1 ve Kac aya kadar 1 alanlarını kontrol edin!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SehirVarMi()
        {
            var UlkeId = Convert.ToInt32(Request["UlkeId"]);
            var Baslik = Request["Baslik"];

            string Mesaj = "";
            string Status = "";
            if (!String.IsNullOrEmpty(Baslik.Trim()) && UlkeId > 0)
            {
                sehirRepository = new SehirRepository();

                bool? durum = sehirRepository.SehirMarMi(Baslik, UlkeId);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Şehir daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Ülke ya da şehir alanlarını kontrol edin!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UlkeListesi(int DilId = 0)
        {
            ulkeRepository = new UlkeRepository();
            IEnumerable<KeyValuePair<int, string>> ulkeler =
                ulkeRepository.Liste().Where(u => u.DilId == DilId).Select(u => new { u.Baslik, u.Id, u.Durumu }).OrderBy(u => u.Baslik).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik));

            return Json(new { ulkeler = ulkeler }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SehirlerListesi(int UlkeId = 0)
        {
            sehirRepository = new SehirRepository();
            IEnumerable<KeyValuePair<int, string>> sehirler =
                sehirRepository.Liste().Where(u => u.UlkeId == UlkeId).Select(u => new { u.Baslik, u.Id, u.Durumu }).OrderBy(u => u.Baslik).ToList()
                .Select(u => new KeyValuePair<int, string>(u.Id, u.Baslik));

            return Json(new { sehirler = sehirler }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult MerkezVarMi()
        {
            var baslik = Request["baslik"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";

            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Merkez adını alanını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {
                merkezRepository = new MerkezRepository();

                bool? durum = merkezRepository.MerkezVarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                    Mesaj = "";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Merkez daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MerkezVarMiUpdate()
        {
            var Baslik = Request["Baslik"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";

            if (String.IsNullOrEmpty(Baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Merkez alanını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {

                Status = "ok";
                Mesaj = "";

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult OkulVarMi()
        {
            var baslik = Request["baslik"];
            var merkezId = Convert.ToInt32("0" + Request["merkezId"]);
            var dilId = Request["dilId"];
            var ulkeId = Request["ulkeId"];
            var sehirId = Request["sehirId"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";

            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Okul adı alanını kontrol edin!<br/>";
            }
            if (merkezId <= 0)
            {
                HataSayisi++;
                Mesaj += "Okul merkezi alanını kontrol edin!<br/>";
            }


            if (dilId == "0")
            {
                HataSayisi++;
                Mesaj += "Dil alanını kontrol edin!<br/>";
            }

            if (ulkeId == "0")
            {
                HataSayisi++;
                Mesaj += "Ülke alanını kontrol edin!<br/>";
            }

            if (sehirId == "0")
            {
                HataSayisi++;
                Mesaj += "Şehir alanını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {
                okulRepository = new OkulRepository();

                bool? durum = okulRepository.OkulVarMi(merkezId, baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                    Mesaj = "";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Okul daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult OkulVarMiUpdate()
        {
            var merkezId = Convert.ToInt32(Request["merkezId"]);
            var baslik = Request["baslik"];
            var SehirId = Convert.ToInt32(Request["SehirId"]);

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";

            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Okul adı alanını kontrol edin!<br/>";
            }
            if (merkezId <= 0)
            {
                HataSayisi++;
                Mesaj += "Merkez alanını kontrol edin!<br/>";
            }

            if (SehirId <= 0)
            {
                HataSayisi++;
                Mesaj += "Dil, ülke ya da şehir alanlarını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {

                Status = "ok";
                Mesaj = "";

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult YazarVarmi()
        {
            var adSoyad = Request["AdSoyad"];
            var yazarTipi = Request["YazarTipi"];
            string Mesaj = "";
            string Status = "";
            if (!String.IsNullOrEmpty(adSoyad.Trim()) && !String.IsNullOrEmpty(yazarTipi.Trim()))
            {
                yazarRepository = new YazarRepository();

                bool? durum = yazarRepository.YazarMarMi(adSoyad, yazarTipi);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Yazar daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Yazar ad-soyad alanı ile yazar tipi alanını kontrol ediniz!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MakaleMarMi()
        {
            var baslik = Request["Baslik"];
            var yazarId = Convert.ToInt32("0" + Request["YazarId"].ToString());
            var ozet = Request["Ozet"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }
            if (yazarId <= 0)
            {
                HataSayisi++;
                Mesaj += "Yazar alanını kontrol edin!!<br/>";
            }
            if (String.IsNullOrEmpty(ozet.Trim()))
            {
                HataSayisi++;
                Mesaj += "Özet alanını kontrol edin!<br/>";
            }


            if (HataSayisi == 0)
            {
                makaleRepository = new MakaleRepository();

                bool? durum = makaleRepository.MakaleMarMi(baslik, yazarId);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Makale daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HaberVarMi()
        {
            var baslik = Request["Baslik"];
            var ozet = Request["Ozet"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }
            if (String.IsNullOrEmpty(ozet.Trim()))
            {
                HataSayisi++;
                Mesaj += "Özet alanını kontrol edin!<br/>";
            }


            if (HataSayisi == 0)
            {
                haberRepository = new HaberRepository();

                bool? durum = haberRepository.HaberMarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Haber daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SliderVarMi()
        {
            var baslik = Request["Baslik"];
            var ozet = Request["Ozet"];
            var pageLink = Request["PageLink"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }
            if (String.IsNullOrEmpty(ozet.Trim()))
            {
                HataSayisi++;
                Mesaj += "Özet alanını kontrol edin!<br/>";
            }

            if (String.IsNullOrEmpty(pageLink.Trim()))
            {
                HataSayisi++;
                Mesaj += "Sayfa url alanını kontrol edin!<br/>";
            }


            if (HataSayisi == 0)
            {
                sliderRepository = new SliderRepository();

                bool? durum = sliderRepository.SliderMarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Slider daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SliderSil()
        {
            var id = Convert.ToInt32(Request["id"]);

            string Mesaj = "ok";
            string Status = "error";

            if (id > 0)
            {
                sliderRepository = new SliderRepository();
                var slider = sliderRepository.Detay(id, new int[] { (int)GeneralVariables.Durum.Pasif, (int)GeneralVariables.Durum.Aktif });
                if (slider != null)
                {
                    slider.Durumu = (int)GeneralVariables.Durum.Silindi;
                    sliderGenericRepository = new GenericRepository<DilOkulu_Slider>();
                    var retVal = sliderGenericRepository.Update(slider);
                    Status = "ok";
                    Mesaj = "Slider silindi.";

                }
            }


            return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GaleriVarMi()
        {
            var baslik = Request["Baslik"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {
                galeriRepository = new GaleriRepository();

                bool? durum = galeriRepository.GaleriVarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Galeri daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult VizeRehberiMarMi()
        {
            var baslik = Request["Baslik"];

            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }

            if (HataSayisi == 0)
            {
                haberRepository = new HaberRepository();

                bool? durum = haberRepository.HaberMarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Vize Rehberi daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult StatikLinkSil()
        {
            var id = Convert.ToInt32("0" + Request["id"]);

            string Mesaj = "";
            string Status = "";
            if (id > 0)
            {
                statikLinkRepository = new StatikLinkRepository();
                var link = statikLinkRepository.Detay(id);
                if (link != null)
                {
                    link.Durumu = (int)GeneralVariables.Durum.Silindi;

                    statikLinklerGenericRepository = new GenericRepository<DilOkulu_StatikLinkler>(statikLinkRepository.DBContext);
                    var retLink = statikLinklerGenericRepository.Update(link);
                    if (retLink != null)
                    {
                        Status = "ok";
                    }
                    else
                    {
                        Status = "err";
                        Mesaj = "Silme işlemi sırasında bir problem oluştu!";

                    }
                }
                else
                {
                    Status = "err";
                    Mesaj = "Silme işlemi sırasında bir problem oluştu!";
                }


                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Silme işlemi sırasında bir problem oluştu!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult StatikLinkSirala()
        {
            var orderList = Request["orderList"];


            string Mesaj = "";
            string Status = "";
            if (orderList.Length > 0)
            {
                try
                {
                    string[] arryList = orderList.Split(';');
                    int id = 0;
                    int order = 0;

                    statikLinkRepository = new StatikLinkRepository();
                    statikLinklerGenericRepository = new GenericRepository<DilOkulu_StatikLinkler>(statikLinkRepository.DBContext);

                    for (int i = 0; i < arryList.Length; i++)
                    {
                        id = Convert.ToInt32(arryList[i].Split('|')[0]);
                        order = Convert.ToInt32(arryList[i].Split('|')[1]);

                        var link = statikLinkRepository.Detay(id);

                        if (link != null)
                        {
                            link.Oncelik = order;

                            var retLink = statikLinklerGenericRepository.Update(link);
                        }
                    }

                    Status = "ok";

                }
                catch (Exception)
                {
                    Status = "err";
                    Mesaj = "Sıralama işlemi sırasında bir problem oldu!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                Mesaj = "Sıralama işlemi sırasında bir problem oldu!";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ProgramKategoriVarMi()
        {
            var baslik = Request["Baslik"];


            int HataSayisi = 0;
            string Mesaj = "";
            string Status = "";


            if (String.IsNullOrEmpty(baslik.Trim()))
            {
                HataSayisi++;
                Mesaj += "Başlık alanını kontrol edin!<br/>";
            }


            if (HataSayisi == 0)
            {
                sunulanProgramRepository = new SunulanProgramRepository();

                bool? durum = sunulanProgramRepository.ProgramKategoriVarMi(baslik);
                if (durum != null && durum == false)
                {
                    Status = "ok";
                }
                else
                {
                    Status = "err";
                    Mesaj = "Kategori daha önce tanımlanmış!";
                }

                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Status = "err";
                return Json(new { status = Status, mesaj = Mesaj }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
