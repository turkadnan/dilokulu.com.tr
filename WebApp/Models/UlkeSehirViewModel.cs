using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class UlkeSehirViewModel
    {
        public IEnumerable<KeyValuePair<int, string>> Ulkeler { get; set; }
        public DilOkulu_Sehirler Sehir { get; set; }
        public List<DilOkulu_Sehirler> Sehirler { get; set; }
    }
}