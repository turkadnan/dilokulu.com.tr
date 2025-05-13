using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface IMakaleRepository
    {
        IQueryable<DilOkulu_Makaleler> Liste();
        IList<DilOkulu_Makaleler> Liste(Expression<Func<DilOkulu_Makaleler, bool>> predicate, Expression<Func<DilOkulu_Makaleler, DateTime>> orderByDescending, params Expression<Func<DilOkulu_Makaleler, object>>[] includes);
        IList<DilOkulu_Makaleler> Liste(Expression<Func<DilOkulu_Makaleler, bool>> predicate, Expression<Func<DilOkulu_Makaleler, DateTime>> orderByDescending, int TotalRecord, params Expression<Func<DilOkulu_Makaleler, object>>[] includes);
        DilOkulu_Makaleler Detay(int id, int[] durum);
        DilOkulu_Makaleler Detay(string url, int[] durum);
        bool? MakaleMarMi(string baslik, int yazarId);
        DilOkuluEntities DBContext { get; }
    }

    public class MakaleRepository : IMakaleRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public MakaleRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public MakaleRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Makaleler> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Makaleler.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public IList<DilOkulu_Makaleler> Liste(params Expression<Func<DilOkulu_Makaleler, object>>[] includes)
        //{
        //    List<DilOkulu_Makaleler> list;
        //    try
        //    {
        //        List<string> includeList = new List<string>();

        //        System.Data.Entity.DbSet<DilOkulu_Makaleler> dbQuery = dbContext.Set<DilOkulu_Makaleler>();
        //        System.Data.Entity.Infrastructure.DbQuery<DilOkulu_Makaleler> dbIncludes = null;

        //        foreach (var item in includes)
        //        {
        //            MemberExpression body = item.Body as MemberExpression;
        //            if (body == null)
        //                throw new ArgumentException("The body must be a member expression");
        //            includeList.Add(body.Member.Name);
        //        }

        //        if (includeList.Count > 0)
        //        {
        //            foreach (var include in includeList)
        //                dbIncludes = dbQuery.Include(include);

        //            return list = dbIncludes.AsNoTracking().ToList();
        //        }
        //        else
        //        {
        //            return list = dbQuery.AsNoTracking().ToList();
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public IList<DilOkulu_Makaleler> Liste(Expression<Func<DilOkulu_Makaleler, bool>> predicate, Expression<Func<DilOkulu_Makaleler, DateTime>> orderByDescending, params Expression<Func<DilOkulu_Makaleler, object>>[] includes)
        {
            List<DilOkulu_Makaleler> list;
            try
            {                
                List<string> includeList = new List<string>();

                System.Data.Entity.DbSet<DilOkulu_Makaleler> dbQuery = dbContext.Set<DilOkulu_Makaleler>();
                System.Data.Entity.Infrastructure.DbQuery<DilOkulu_Makaleler> dbIncludes = null;


                foreach (var item in includes)
                {
                    MemberExpression body = item.Body as MemberExpression;
                    if (body == null)
                        throw new ArgumentException("The body must be a member expression");
                    includeList.Add(body.Member.Name);
                }

                if (includeList.Count > 0)
                {
                    foreach (var include in includeList)
                        dbIncludes = dbQuery.Include(include);

                    if (orderByDescending!=null)
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).OrderByDescending(orderByDescending).ToList();
                    }
                    else
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).ToList();
                    }

                    
                }
                else
                {
                    if (orderByDescending != null)
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).OrderByDescending(orderByDescending).ToList();
                    }
                    else
                    {

                        return list = dbQuery.AsNoTracking().Where(predicate).ToList();
                    }

                }

            }
            catch (Exception)
            {
                return null;
            }           
        }

        public IList<DilOkulu_Makaleler> Liste(Expression<Func<DilOkulu_Makaleler, bool>> predicate, Expression<Func<DilOkulu_Makaleler, DateTime>> orderByDescending, int TotalRecord = 10, params Expression<Func<DilOkulu_Makaleler, object>>[] includes)
        {
            List<DilOkulu_Makaleler> list;
            try
            {
                List<string> includeList = new List<string>();

                System.Data.Entity.DbSet<DilOkulu_Makaleler> dbQuery = dbContext.Set<DilOkulu_Makaleler>();
                System.Data.Entity.Infrastructure.DbQuery<DilOkulu_Makaleler> dbIncludes = null;


                foreach (var item in includes)
                {
                    MemberExpression body = item.Body as MemberExpression;
                    if (body == null)
                        throw new ArgumentException("The body must be a member expression");
                    includeList.Add(body.Member.Name);
                }

                if (includeList.Count > 0)
                {
                    foreach (var include in includeList)
                        dbIncludes = dbQuery.Include(include);

                    if (orderByDescending != null)
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).OrderByDescending(orderByDescending).Take(TotalRecord).ToList();
                    }
                    else
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).Take(TotalRecord).ToList();
                    }


                }
                else
                {
                    if (orderByDescending != null)
                    {
                        return list = dbIncludes.AsNoTracking().Where(predicate).OrderByDescending(orderByDescending).Take(TotalRecord).ToList();
                    }
                    else
                    {

                        return list = dbQuery.AsNoTracking().Where(predicate).Take(TotalRecord).ToList();
                    }

                }

            }
            catch (Exception)
            {
                return null;
            }  
        }

        public DilOkulu_Makaleler Detay(int id, int[] durum)
        {
            try
            {
                var makale = dbContext.DilOkulu_Makaleler.Single(m => m.Id == id && durum.Contains(m.Durumu));
                return makale;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkulu_Makaleler Detay(string url, int[] durum)
        {
            try
            {
                var makale = dbContext.DilOkulu_Makaleler.Single(m => m.Url == url && durum.Contains(m.Durumu));
                return makale;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? MakaleMarMi(string baslik, int yazarId)
        {
            try
            {
                int count = dbContext.DilOkulu_Makaleler
                    .Where(
                    m =>
                        m.Baslik.ToLower() == baslik.ToLower() &&
                        m.YazarId == yazarId &&
                        m.Durumu != (int)GeneralVariables.Durum.Silindi
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