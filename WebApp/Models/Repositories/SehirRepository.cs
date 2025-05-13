using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface ISehirRepository
    {
        IQueryable<DilOkulu_Sehirler> Liste();
        DilOkulu_Sehirler Detay(int Id, int[] durum);
        bool? SehirMarMi(string Baslik, int UlkeId);
        DilOkuluEntities DBContext { get; }

    }

    public class SehirRepository : ISehirRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public SehirRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public SehirRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Sehirler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Sehirler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Sehirler Detay(int Id, int[] durum)
        {
            try
            {
                var dil = dbContext.DilOkulu_Sehirler.Single(s => s.Id == Id && durum.Contains(s.Durumu));
                return dil;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Sehirler DetayByNoneProxy(int Id)
        {
            try
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                var dil = dbContext.DilOkulu_Sehirler.Single(s => s.Id == Id && s.Durumu != 3);
                return dil;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? SehirMarMi(string Baslik, int UlkeId)
        {
            try
            {
                int count = dbContext.DilOkulu_Sehirler
                    .Where(
                    s =>
                        s.Baslik.ToLower() == Baslik.ToLower() &&
                        s.UlkeId == UlkeId &&
                        s.Durumu != (int)GeneralVariables.Durum.Silindi
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