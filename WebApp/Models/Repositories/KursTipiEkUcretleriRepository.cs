using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IKursTipiEkUcretleriRepository
    {
        IQueryable<DilOkulu_KursTipiEkUcretleri> Liste();
        DilOkulu_KursTipiEkUcretleri Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KursTipiEkUcretleriRepository : IKursTipiEkUcretleriRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursTipiEkUcretleriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursTipiEkUcretleriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursTipiEkUcretleri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursTipiEkUcretleri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursTipiEkUcretleri Detay(int Id, int[] durum)
        {
            try
            {
                var kursTipiEkUcret = dbContext.DilOkulu_KursTipiEkUcretleri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kursTipiEkUcret;
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