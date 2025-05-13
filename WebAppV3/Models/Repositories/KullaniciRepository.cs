using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{
    public interface IKullaniciRepository
    {
        DilOkulu_Kullanicilar Login(string KullaniciAdi, string Sifre);   
    }

    public class KullaniciRepository : IKullaniciRepository
    {
        #region Variables
        private DilOkuluEntities dbContext = null;
        #endregion

        public DilOkulu_Kullanicilar Login(string KullaniciAdi, string Sifre)
        {
            try
            {
                dbContext = new DilOkuluEntities();
                var kullanici = dbContext.DilOkulu_Kullanicilar.First(k => k.KullaniciAdi == KullaniciAdi && k.Sifre == Sifre);
                return kullanici;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}