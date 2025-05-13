using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IEtiketIliskileriRepository
    {
        bool? IliskiMarMi(int icerikID, int etiketId, string etiketTipi);
        IQueryable<DilOkulu_EtiketIliskileri> Liste();
        DilOkuluEntities DBContext { get; }
    }

    public class EtiketIliskileriRepository : IEtiketIliskileriRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public EtiketIliskileriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public EtiketIliskileriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_EtiketIliskileri> Liste()
        {
            try
            {
                dbContext.Configuration.LazyLoadingEnabled = false;
                dbContext.Configuration.ProxyCreationEnabled = false;
                return dbContext.DilOkulu_EtiketIliskileri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? IliskiMarMi(int icerikID, int etiketId, string etiketTipi)
        {
            try
            {
                int count = dbContext.DilOkulu_EtiketIliskileri
                    .Where(
                    ei =>
                        ei.IcerikId == icerikID &&
                        ei.EtiketId == etiketId &&
                        ei.EtiketTipi == etiketTipi &&
                        ei.Durumu != (int)GeneralVariables.Durum.Silindi
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