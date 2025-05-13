using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IMerkezRepository
    {
        IQueryable<DilOkulu_Merkez> Liste();
        DilOkulu_Merkez Detay(int Id, int[] durum);
        DilOkulu_Merkez Detay(string url, int[] durum);
        bool? MerkezVarMi(string Baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class MerkezRepository : IMerkezRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public MerkezRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public MerkezRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Merkez> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Merkez.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Merkez Detay(int Id, int[] durum)
        {
            try
            {
                var merkez = dbContext.DilOkulu_Merkez.Single(o => o.Id == Id && durum.Contains(o.Durumu));
                return merkez;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Merkez Detay(string url, int[] durum)
        {
            try
            {
                var merkez = dbContext.DilOkulu_Merkez.Single(o => o.Url == url && durum.Contains(o.Durumu));
                return merkez;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? MerkezVarMi(string Baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Merkez
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