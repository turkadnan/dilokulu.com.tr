using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{
    public interface IKursTipiFiyatlandirmalariRepository
    {
        IQueryable<DilOkulu_KursTipiFiyatlandirmalari> Liste();
        DilOkulu_KursTipiFiyatlandirmalari Detay(int Id, int[] durum);
        DilOkuluEntities DBContext { get; }

    }

    public class KursTipiFiyatlandirmalariRepository : IKursTipiFiyatlandirmalariRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public KursTipiFiyatlandirmalariRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public KursTipiFiyatlandirmalariRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_KursTipiFiyatlandirmalari> Liste()
        {
            try
            {
                return dbContext.DilOkulu_KursTipiFiyatlandirmalari.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_KursTipiFiyatlandirmalari Detay(int Id, int[] durum)
        {
            try
            {
                var kursTipiFiyati = dbContext.DilOkulu_KursTipiFiyatlandirmalari.Single(d => d.Id == Id && durum.Contains(d.Durumu));
                return kursTipiFiyati;
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