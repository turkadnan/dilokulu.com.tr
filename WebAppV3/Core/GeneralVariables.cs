using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Core
{
    public static class GeneralVariables
    {
        public enum OrtakTip
        {
            ogrenci,
            academix,
            danisman
        }

        public enum Durum
        {
            Aktif = 1,
            Pasif = 0,
            Silindi = 3
        }

        public enum KategoriListeTipi
        {
            Anasayfa,
            IcerikSayfalari
        }

        public enum MakaleKategorileri
        {
            Academix=0,
            Danisman=1,
            Ogrenci=2
        }
        public enum StatikLinkler
        {
            PopulerUlkeler,
            PopulerSehirler,
            PopulerDilOkullari
        }

        public enum UploadType
        {
            Yazar,
            Makale,
            Haber,
            Slider,
            VizeRehberi,
            Dil,
            Okul,
            Sehir,
            Ulke,
            banner300x350
        }

        public static string UploadFilePath(UploadType uploadType)
        {
            string tmp = "";
            switch (uploadType)
            {
                case UploadType.Yazar:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\yazarlar\\";
                    break;
                case UploadType.Makale:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Makaleler\\";
                    break;
                case UploadType.Haber:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Haberler\\";
                    break;
                case UploadType.Slider:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Slider\\";
                    break;
                case UploadType.VizeRehberi:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\VizeRehberi\\";
                    break;
                case UploadType.Dil:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Dil\\";
                    break;
                case UploadType.Okul:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Okul\\";
                    break;
                case UploadType.Sehir:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Sehir\\";
                    break;
                case UploadType.Ulke:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "content\\images\\Ulke\\";
                    break;
                case UploadType.banner300x350:
                    tmp = AppDomain.CurrentDomain.BaseDirectory + "Assets\\Banners\\";
                    break;
            }

            return tmp;
        }
    }
}