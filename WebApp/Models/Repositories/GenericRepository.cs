using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Insert(TEntity T);
        TEntity Update(TEntity T);
        int Delete(TEntity T);
    }

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public GenericRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public GenericRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public TEntity Insert(TEntity T)
        {
            try
            {
                if (dbContext == null)
                    dbContext = new DilOkuluEntities();
                dbContext.Set<TEntity>().Add(T);
                dbContext.SaveChanges();
                return T;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public TEntity Update(TEntity T)
        {
            try
            {

                dbContext.Entry<TEntity>(T).State = System.Data.EntityState.Modified;
                dbContext.SaveChanges();
                return T;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public int Delete(TEntity T)
        {
            try
            {
                if (dbContext == null)
                    dbContext = new DilOkuluEntities();

                dbContext.Entry<TEntity>(T).State = System.Data.EntityState.Deleted;
                dbContext.Set<TEntity>().Remove(T);
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int Delete(List<TEntity> T)
        {
            try
            {
                if (dbContext == null)
                    dbContext = new DilOkuluEntities();

                foreach (var item in T)
                {
                    dbContext.Entry<TEntity>(item).State = System.Data.EntityState.Deleted;
                    dbContext.Set(typeof(TEntity)).Remove(item);
                }

                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}