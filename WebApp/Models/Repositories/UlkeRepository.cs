using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface IUlkeRepository
    {
        IQueryable<DilOkulu_Ulkeler> Liste();
        DilOkulu_Ulkeler Detay(int Id, int[] durum);
        DilOkulu_Ulkeler Detay(string url, int[] durum);
        bool? UlkeMarMi(string Baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class UlkeRepository : IUlkeRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public UlkeRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public UlkeRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Ulkeler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Ulkeler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Ulkeler Detay(int Id, int[] durum)
        {
            try
            {
                var ulke = dbContext.DilOkulu_Ulkeler.Single(u => u.Id == Id && durum.Contains(u.Durumu));
                return ulke;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Ulkeler Detay(string url, int[] durum)
        {
            try
            {
                var ulke = dbContext.DilOkulu_Ulkeler.Single(u => u.Url== url && durum.Contains(u.Durumu));
                return ulke;
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

        public bool? UlkeMarMi(string Baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Ulkeler
                    .Where(
                    u => 
                        u.Baslik.ToLower() == Baslik.ToLower() &&
                        u.Durumu != (int)GeneralVariables.Durum.Silindi
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
    }
}