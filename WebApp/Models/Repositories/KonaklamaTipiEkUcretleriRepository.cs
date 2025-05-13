using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IKonaklamaTipiEkUcretleriRepository
    {
        IQueryable<DilOkulu_KonaklamaTipiEkUcretleri> Liste();
        DilOkulu_KonaklamaTipiEkUcretleri Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KonaklamaTipiEkUcretleriRepository : IKonaklamaTipiEkUcretleriRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KonaklamaTipiEkUcretleriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KonaklamaTipiEkUcretleriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KonaklamaTipiEkUcretleri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KonaklamaTipiEkUcretleri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KonaklamaTipiEkUcretleri Detay(int Id, int[] durum)
        {
            try
            {
                var konaklamaTipiEkUcret = dbContext.DilOkulu_KonaklamaTipiEkUcretleri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return konaklamaTipiEkUcret;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkuluEntities DBContext
        {
            get
            {
                if (dbContext == null) dbContext = new DilOkuluEntities();
                return dbContext;
            }
        }
    }
}