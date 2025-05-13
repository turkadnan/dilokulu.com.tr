using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{

    public interface IKursTipiPromosyonTanimlariRepository
    {
        IQueryable<DilOkulu_KursTipiPromosyonTanimlari> Liste();
        DilOkulu_KursTipiPromosyonTanimlari Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KursTipiPromosyonTanimlariRepository : IKursTipiPromosyonTanimlariRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursTipiPromosyonTanimlariRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursTipiPromosyonTanimlariRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursTipiPromosyonTanimlari> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursTipiPromosyonTanimlari.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursTipiPromosyonTanimlari Detay(int Id, int[] durum)
        {
            try
            {
                var kursTipiPromosyon = dbContext.DilOkulu_KursTipiPromosyonTanimlari.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kursTipiPromosyon;
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