using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class jsonKargoHaricKursTipleri
    {
        public string okulId { get; set; }
        public string kursTipiId { get; set; }
        public string selected { get; set; }
    }
}