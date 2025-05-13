using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IDilRepository
    {
        IQueryable<DilOkulu_Diller> Liste();
        DilOkulu_Diller Detay(int Id, int[] durum);
        DilOkulu_Diller Detay(string url, int[] durum);
        bool? DilMarMi(string Baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class DilRepository : IDilRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public DilRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public DilRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Diller> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Diller.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Diller Detay(int Id, int[] durum)
        {
            try
            {
                var dil = dbContext.DilOkulu_Diller.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return dil;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Diller Detay(string url, int[] durum)
        {
            try
            {
                var dil = dbContext.DilOkulu_Diller.Single(d => d.Url == url && durum.Contains(d.Durumu));
                return dil;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? DilMarMi(string Baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Diller
                    .Where(
                    d =>
                        d.Baslik.ToLower() == Baslik.ToLower() &&
                        d.Durumu != (int)GeneralVariables.Durum.Silindi
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