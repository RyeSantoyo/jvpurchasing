using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class Suppliers
    {
        public int Id { get; set; }
        public string SupplierCode { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierAddress { get; set; } = string.Empty;
        public string CityAddress { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string TelNo { get; set; } = string.Empty;
        public int FaxNo { get; set; } 
        public string MobileNo { get; set; } = string.Empty;
        public Terms Terms { get; set; } = null!;
        public int TermsId { get; set; }
    }
}