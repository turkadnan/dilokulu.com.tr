using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class MerkezOkullarViewModel
    {
        public List<DilOkulu_Okullar> Okullar { get; set; }
        public DilOkulu_Merkez Merkez { get; set; }
    }
}