using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Core;

namespace WebApp.Models.Repositories
{
    public interface ISliderRepository
    {
        IQueryable<DilOkulu_Slider> Liste();
        DilOkulu_Slider Detay(int id, int[] durum);
        bool? SliderMarMi(string baslik);
        DilOkuluEntities DBContext { get; }
    }

    public class SliderRepository : ISliderRepository
    {
        #region Variables
        public DilOkuluEntities dbContext = null;
        #endregion

        public SliderRepository()
        {
            dbContext = new DilOkuluEntities();
        }

        public SliderRepository(DilOkuluEntities _dbContext)
        {
            dbContext = _dbContext;
        }

        public IQueryable<DilOkulu_Slider> Liste()
        {
            try
            {
                return dbContext.DilOkulu_Slider.AsQueryable();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public DilOkulu_Slider Detay(int id, int[] durum)
        {
            try
            {
                var slider = dbContext.DilOkulu_Slider.Single(m => m.Id == id && durum.Contains(m.Durumu));
                return slider;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool? SliderMarMi(string baslik)
        {
            try
            {
                int count = dbContext.DilOkulu_Slider
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