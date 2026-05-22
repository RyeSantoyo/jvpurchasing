using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models.DTO
{
    public class PODetailsDTO
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public int POId { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string PONumber { get; set; } = string.Empty;
    }
}


