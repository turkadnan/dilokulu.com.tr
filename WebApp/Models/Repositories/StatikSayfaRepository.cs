using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IStatikSayfaRepository
    {
        IQueryable<DilOkulu_StatikSayfalar> Liste();
        DilOkulu_StatikSayfalar Detay(string baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class StatikSayfaRepository : IStatikSayfaRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public StatikSayfaRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public StatikSayfaRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_StatikSayfalar> Liste()
        {
            try
            {
                return dbContext.DilOkulu_StatikSayfalar.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_StatikSayfalar Detay(string baslik)
        {
            try
            {
                var sayfa = dbContext.DilOkulu_StatikSayfalar.Single(d => d.Baslik == baslik);
                return sayfa;
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