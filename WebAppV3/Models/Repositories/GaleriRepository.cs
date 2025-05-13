using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IGaleriRepository
    {
        IQueryable<DilOkulu_Galeriler> Liste();
        DilOkulu_Galeriler Detay(int Id, int[] durum);
        bool? GaleriVarMi(string Baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class GaleriRepository : IGaleriRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public GaleriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public GaleriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Galeriler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Galeriler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Galeriler Detay(int Id, int[] durum)
        {
            try
            {
                var galeri = dbContext.DilOkulu_Galeriler.Single(g => g.Id == Id && durum.Contains(g.Durumu));
                return galeri;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Galeriler DetayByNoneProxy(int Id)
        {
            try
            {
                dbContext.Configuration.ProxyCreationEnabled = false;
                var galeri = dbContext.DilOkulu_Galeriler.Single(g => g.Id == Id && g.Durumu != 3);
                return galeri;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? GaleriVarMi(string Baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Galeriler
                    .Where(
                    g =>
                        g.Baslik.ToLower() == Baslik.ToLower() &&
                        g.Durumu != (int)GeneralVariables.Durum.Silindi
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