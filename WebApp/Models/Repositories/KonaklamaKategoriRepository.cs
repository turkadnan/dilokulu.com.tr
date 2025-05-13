using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface IKonaklamaKategoriRepository
    {
        IQueryable<DilOkulu_KonaklamaKategorileri> Liste();
        DilOkulu_KonaklamaKategorileri Detay(int Id, int[] durum);
        bool? KonaklamaKategoriVarMi(string Baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class KonaklamaKategoriRepository : IKonaklamaKategoriRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KonaklamaKategoriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KonaklamaKategoriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KonaklamaKategorileri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KonaklamaKategorileri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KonaklamaKategorileri Detay(int Id, int[] durum)
        {
            try
            {
                var kategori = dbContext.DilOkulu_KonaklamaKategorileri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kategori;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? KonaklamaKategoriVarMi(string Baslik)
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