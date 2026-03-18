using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string? CompanyCode { get; set; }
        public string Name { get; set; } = string.Empty;
        
    }
}