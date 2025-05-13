using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppV3.Models.Repositories
{

    public interface IParaBirimiRepository
    {
        IQueryable<DilOkulu_ParaBirimleri> Liste();        
        DilOkuluEntities DBContext { get; }
    }

    public class ParaBirimiRepository:IParaBirimiRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public ParaBirimiRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public IQueryable<DilOkulu_ParaBirimleri> Liste()
        {
            try
            {
                return dbContext.DilOkulu_ParaBirimleri.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DilOkuluEntities DBContext
        {
            get { throw new NotImplementedException(); }
        }
    }
}