//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppV3.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DilOkulu_Galeriler
    {
        public DilOkulu_Galeriler()
        {
            this.DilOkulu_GaleriDosyalari = new HashSet<DilOkulu_GaleriDosyalari>();
        }
    
        public int Id { get; set; }
        public string Baslik { get; set; }
        public System.DateTime KayitTarihi { get; set; }
        public byte Durumu { get; set; }
    
        public virtual ICollection<DilOkulu_GaleriDosyalari> DilOkulu_GaleriDosyalari { get; set; }
    }
}
