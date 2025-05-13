using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IStatikLinkRepository
    {
        IQueryable<DilOkulu_StatikLinkler> Liste();
        DilOkulu_StatikLinkler Detay(string baslik);
        DilOkulu_StatikLinkler Detay(int id);
        DilOkuluEntities DBContext { get; }

    }

    public class StatikLinkRepository : IStatikLinkRepository
    {

        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public StatikLinkRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public StatikLinkRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_StatikLinkler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_StatikLinkler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_StatikLinkler Detay(string baslik)
        {
            try
            {
                var link = dbContext.DilOkulu_StatikLinkler.Single(d => d.Baslik == baslik);
                return link;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_StatikLinkler Detay(int id)
        {
            try
            {
                var link = dbContext.DilOkulu_StatikLinkler.Single(d => d.Id == id);
                return link;
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