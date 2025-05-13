using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface IOkulRepository
    {
        IQueryable<DilOkulu_Okullar> Liste();
        DilOkulu_Okullar Detay(int Id, int[] durum);
        DilOkulu_Okullar Detay(string url, int[] durum);
        bool? OkulVarMi(int merkezId, string baslik);
        DilOkuluEntities DBContext { get; }
        IEnumerable<KeyValuePair<int, string>> MerkezOkullarListesi();
        List<DilOkulu_Okullar> CMSOkullarListesi(int merkezId);

    }

    public class OkulRepository : IOkulRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public OkulRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public OkulRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Okullar> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Okullar.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Okullar Detay(int Id, int[] durum)
        {
            try
            {
                var okul = dbContext.DilOkulu_Okullar.Single(o => o.Id == Id && durum.Contains(o.Durumu));
                return okul;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Okullar Detay(string url, int[] durum)
        {
            try
            {
                var okul = dbContext.DilOkulu_Okullar.Single(o => o.Url == url && durum.Contains(o.Durumu));
                return okul;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? OkulVarMi(int merkezId, string baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Okullar
                    .Where(
                    d =>
                        d.Baslik.ToLower() == baslik.ToLower() &&
                        d.MerkezId == merkezId &&
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

        /*Bakılacak*/
        public IEnumerable<KeyValuePair<int, string>> MerkezOkullarListesi()
        {
            try
            {
                var okullar = dbContext.DilOkulu_Okullar.Where(o => o.Durumu == 1)
                    .Select(o => new { o.Baslik, o.Id, o.Durumu })
                    .OrderBy(o => o.Baslik).ToList().Select(o => new KeyValuePair<int, string>(o.Id, o.Baslik));

                return okullar;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<DilOkulu_Okullar> CMSOkullarListesi(int merkezId)
        {
            try
            {
                var okullar = dbContext.DilOkulu_Okullar.Where(o => 1 == 1
                && o.Durumu != 3
                && (merkezId > 0 ? o.MerkezId == merkezId : 1 == 1)
                ).OrderByDescending(o=>o.Durumu).ThenBy(o=>o.Baslik).ToList();

                return okullar;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}