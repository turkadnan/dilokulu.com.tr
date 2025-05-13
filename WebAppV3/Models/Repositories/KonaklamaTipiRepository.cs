using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{
    public interface IKonaklamaTipiRepository
    {
        IQueryable<DilOkulu_KonaklamaTipleri> Liste();
        DilOkulu_KonaklamaTipleri Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KonaklamaTipiRepository : IKonaklamaTipiRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KonaklamaTipiRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KonaklamaTipiRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KonaklamaTipleri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KonaklamaTipleri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KonaklamaTipleri Detay(int Id, int[] durum)
        {
            try
            {
                var konaklamaTipi = dbContext.DilOkulu_KonaklamaTipleri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return konaklamaTipi;
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