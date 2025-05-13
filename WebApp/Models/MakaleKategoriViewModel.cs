using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class MakaleKategoriViewModel
    {
        public IEnumerable<KeyValuePair<int, string>> Kategoriler { get; set; }
        public IEnumerable<KeyValuePair<int, string>> Yazarlar { get; set; }  
        public List<DilOkulu_Makaleler> Makaleler { get; set; }
        public DilOkulu_Makaleler Makale { get; set; }
    }
}