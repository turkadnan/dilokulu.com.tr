using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class AramaViewModel
    {
        public List<DilOkulu_Diller> Diller { get; set; }
        public List <DilOkulu_Merkez> Merkezler { get; set; }
    }
}