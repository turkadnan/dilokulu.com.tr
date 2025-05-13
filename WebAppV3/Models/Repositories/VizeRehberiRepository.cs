using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IVizeRehberiRepository
    {
        IQueryable<DilOkulu_VizeRehberi> Liste();
        DilOkulu_VizeRehberi Detay(int id, int[] durum);
        DilOkulu_VizeRehberi Detay(string url, int[] durum);
        bool? VizeRehberiMarMi(string baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class VizeRehberiRepository : IVizeRehberiRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public VizeRehberiRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public VizeRehberiRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_VizeRehberi> Liste()
        {
            try
            {
                return dbContext.DilOkulu_VizeRehberi.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public DilOkulu_VizeRehberi Detay(int id, int[] durum)
        {
            try
            {
                var vize = dbContext.DilOkulu_VizeRehberi.Single(v => v.Id == id && durum.Contains(v.Durumu));
                return vize;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_VizeRehberi Detay(string url, int[] durum)
        {
            try
            {
                var vize = dbContext.DilOkulu_VizeRehberi.Single(v => v.Url == url && durum.Contains(v.Durumu));
                return vize;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? VizeRehberiMarMi(string baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_VizeRehberi
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