using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebAppV3.Core;

namespace WebAppV3.Models.Repositories
{
    public interface ISunulanProgramRepository
    {
        IQueryable<DilOkulu_SunulanProgramlar> Liste();
        DilOkulu_SunulanProgramlar Detay(int id, int[] durum);
        bool? ProgramKategoriVarMi(string baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class SunulanProgramRepository : ISunulanProgramRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public SunulanProgramRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public SunulanProgramRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_SunulanProgramlar> Liste()
        {
            try
            {
                return dbContext.DilOkulu_SunulanProgramlar.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public DilOkulu_SunulanProgramlar Detay(int id, int[] durum)
        {
            try
            {
                var program = dbContext.DilOkulu_SunulanProgramlar.Single(m => m.Id == id && durum.Contains(m.Durumu));
                return program;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? ProgramKategoriVarMi(string baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_SunulanProgramlar
                    .Where(
                    m =>
                        m.Baslik.ToLower() == baslik.ToLower() && 
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