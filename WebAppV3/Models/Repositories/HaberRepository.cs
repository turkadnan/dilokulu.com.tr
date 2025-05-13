using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IHaberRepository
    {
        IQueryable<DilOkulu_Haberler> Liste();
        DilOkulu_Haberler Detay(int id, int[] durum);
        DilOkulu_Haberler Detay(string url, int[] durum);
        bool? HaberMarMi(string baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class HaberRepository : IHaberRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public HaberRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public HaberRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Haberler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Haberler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public DilOkulu_Haberler Detay(int id, int[] durum)
        {
            try
            {
                var haber = dbContext.DilOkulu_Haberler.Single(m => m.Id == id && durum.Contains(m.Durumu));
                return haber;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Haberler Detay(string url, int[] durum)
        {
            try
            {
                var haber = dbContext.DilOkulu_Haberler.Single(m => m.Url == url && durum.Contains(m.Durumu));
                return haber;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? HaberMarMi(string baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Haberler
                    .Where(
                    m =>
                        m.Baslik.ToLower() == baslik.ToLower() && 
                        m.Durumu != (int)GeneralVariables.Durum.Silindi
                    ).Count();
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

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