using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models.DTO
{
    public class PODto
    {
        public int CompanyId { get; set; }
        public int PONumber { get; set; }
        public DateTime PODate { get; set; } = DateTime.UtcNow;
        public int SupplierId { get; set; }
        

    }
}