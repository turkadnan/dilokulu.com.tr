using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{

    public interface IKursKategoriRepository
    {
        IQueryable<DilOkulu_KursKategorileri> Liste();
        DilOkulu_KursKategorileri Detay(int Id, int[] durum);
        bool? KursKategoriVarMi(string Baslik);
        DilOkuluEntities DBContext { get; }

    }

    public class KursKategoriRepository : IKursKategoriRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursKategoriRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursKategoriRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursKategorileri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursKategorileri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursKategorileri Detay(int Id, int[] durum)
        {
            try
            {
                var kurs = dbContext.DilOkulu_KursKategorileri.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kurs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? KursKategoriVarMi(string Baslik)
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