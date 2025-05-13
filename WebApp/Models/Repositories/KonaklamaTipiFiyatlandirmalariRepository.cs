using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IKonaklamaTipiFiyatlandirmalariRepository
    {
        IQueryable<DilOkulu_KonaklamaTipiFiyatlandirmalari> Liste();
        DilOkulu_KonaklamaTipiFiyatlandirmalari Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KonaklamaTipiFiyatlandirmalariRepository : IKonaklamaTipiFiyatlandirmalariRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KonaklamaTipiFiyatlandirmalariRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KonaklamaTipiFiyatlandirmalariRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KonaklamaTipiFiyatlandirmalari> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KonaklamaTipiFiyatlandirmalari.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KonaklamaTipiFiyatlandirmalari Detay(int Id, int[] durum)
        {
            try
            {
                var konaklamaTipiFiyati = dbContext.DilOkulu_KonaklamaTipiFiyatlandirmalari.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return konaklamaTipiFiyati;
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