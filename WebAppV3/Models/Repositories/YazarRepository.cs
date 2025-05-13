using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface IYazarRepository
    {
        IQueryable<DilOkulu_Yazarlar> Liste();
        DilOkulu_Yazarlar Detay(int Id, int[] durum);
        bool? YazarMarMi(string AdSoyad, string YazarTipi);
        DilOkuluEntities DBContext { get; }

    }

    public class YazarRepository : IYazarRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public YazarRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public YazarRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Yazarlar> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Yazarlar.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Yazarlar Detay(int Id, int[] durum)
        {
            try
            {
                var yazar = dbContext.DilOkulu_Yazarlar.Single(y => y.Id == Id && durum.Contains(y.Durumu));
                return yazar;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? YazarMarMi(string AdSoyad, string YazarTipi)
        {
            try
            {
                int count = dbContext.DilOkulu_Yazarlar
                    .Where(
                    y => 
                        y.AdSoyad.ToLower() == AdSoyad.ToLower() && 
                        y.YazarTipi == YazarTipi &&
                        y.Durumu != (int)GeneralVariables.Durum.Silindi
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