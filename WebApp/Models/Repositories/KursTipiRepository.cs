using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IKursTipiRepository
    {
        IQueryable<DilOkulu_KursTipleri> Liste();
        DilOkulu_KursTipleri Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KursTipiRepository : IKursTipiRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursTipiRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursTipiRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursTipleri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursTipleri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursTipleri Detay(int Id, int[] durum)
        {
            try
            {
                var kursTipi = dbContext.DilOkulu_KursTipleri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kursTipi;
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