using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{

    public interface IKonaklamaTipiPromosyonTanimlariRepository
    {
        IQueryable<DilOkulu_KonaklamaTipiPromosyonTanimlari> Liste();
        DilOkulu_KonaklamaTipiPromosyonTanimlari Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KonaklamaTipiPromosyonTanimlariRepository : IKonaklamaTipiPromosyonTanimlariRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KonaklamaTipiPromosyonTanimlariRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KonaklamaTipiPromosyonTanimlariRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KonaklamaTipiPromosyonTanimlari> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KonaklamaTipiPromosyonTanimlari.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KonaklamaTipiPromosyonTanimlari Detay(int Id, int[] durum)
        {
            try
            {
                var konaklamaTipiPromosyon = dbContext.DilOkulu_KonaklamaTipiPromosyonTanimlari.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return konaklamaTipiPromosyon;
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