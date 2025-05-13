using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{
    public interface IEtiketRepository
    {
        IQueryable<DilOkulu_Etiketler> Liste();
        DilOkulu_Etiketler Detay(int Id, int[] durum);
        DilOkulu_Etiketler Detay(string baslik, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class EtiketRepository : IEtiketRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public EtiketRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public EtiketRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Etiketler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Etiketler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Etiketler Detay(int Id, int[] durum)
        {
            try
            {
                var etiket = dbContext.DilOkulu_Etiketler.Single(e => e.Id == Id && durum.Contains(e.Durumu));
                return etiket;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Etiketler Detay(string baslik, int[] durum)
        {
            try
            {
                var etiket = dbContext.DilOkulu_Etiketler.Single(e => e.Baslik == baslik && durum.Contains(e.Durumu));
                return etiket;
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