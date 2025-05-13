using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    [Serializable]
    public class JsonEtiketItems
    {
        public List<JsonEtiketItem> items;
    }

    [Serializable]
    public class JsonEtiketItem
    {
        public string etiket { get; set; }
        public bool deleted { get; set; }
    }
}