using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models.DTO.PODetails
{
    public class PODetailsCreateDTO
    {
        public int CompanyId { get; set; }
        public int POId { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }
}