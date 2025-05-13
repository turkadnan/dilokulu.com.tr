using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface ISubeSayfaRepository
    {
        IQueryable<DilOkulu_SubeSayfalar> Liste();
        DilOkulu_SubeSayfalar Detay(int Id, int[] durum);
        DilOkulu_SubeSayfalar Detay(string url, int[] durum);        
        DilOkuluEntities DBContext { get; }

    }

    public class SubeSayfaRepository : ISubeSayfaRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public SubeSayfaRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public SubeSayfaRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_SubeSayfalar> Liste()
        {
            try
            {
                return dbContext.DilOkulu_SubeSayfalar.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_SubeSayfalar Detay(int Id, int[] durum)
        {
            try
            {
                var subeSayfa = dbContext.DilOkulu_SubeSayfalar.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return subeSayfa;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_SubeSayfalar Detay(string url, int[] durum)
        {
            try
            {
                var subeSayfa = dbContext.DilOkulu_SubeSayfalar.Single(d => d.Url == url && durum.Contains(d.Durumu));
                return subeSayfa;
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