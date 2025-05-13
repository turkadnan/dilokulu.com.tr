using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface ISubeRepository
    {
        IQueryable<DilOkulu_Subeler> Liste();
        DilOkulu_Subeler Detay(int Id, int[] durum);
        DilOkulu_Subeler Detay(string url, int[] durum);
        bool? SubeMarMi(string Baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class SubeRepository : ISubeRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public SubeRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public SubeRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Subeler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Subeler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Subeler Detay(int Id, int[] durum)
        {
            try
            {
                var sube = dbContext.DilOkulu_Subeler.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return sube;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Subeler Detay(string url, int[] durum)
        {
            try
            {
                var sube = dbContext.DilOkulu_Subeler.Single(d => d.Url == url && durum.Contains(d.Durumu));
                return sube;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? SubeMarMi(string Baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Subeler
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