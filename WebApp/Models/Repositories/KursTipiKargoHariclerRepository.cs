using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IKursTipiKargoHariclerRepository
    {
        IQueryable<DilOkulu_KursTipiKargoHaricler> Liste();
        DilOkulu_KursTipiKargoHaricler Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KursTipiKargoHariclerRepository : IKursTipiKargoHariclerRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursTipiKargoHariclerRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursTipiKargoHariclerRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursTipiKargoHaricler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursTipiKargoHaricler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursTipiKargoHaricler Detay(int Id, int[] durum)
        {
            try
            {
                var kursTipiKargoHaricler = dbContext.DilOkulu_KursTipiKargoHaricler.Single(d => d.Id == Id);
                return kursTipiKargoHaricler;
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