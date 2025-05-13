using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SubeListViewModel
    {
        public IEnumerable<KeyValuePair<int, string>> SubelerShortList { get; set; }
        public int SubeSelectedValue { get; set; }
        public DilOkulu_Subeler Sube { get; set; }
    }
}