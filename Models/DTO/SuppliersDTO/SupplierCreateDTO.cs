using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models.DTO.SuppliersDTO
{
    public class SupplierCreateDTO
    {
        public string SupplierCode { get; set; } = string.Empty;
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierAddress { get; set; } = string.Empty;
        public string CityAddress { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string TelNo { get; set; } = string.Empty;
        public string FaxNo { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public int TermsId { get; set; }
    }
}