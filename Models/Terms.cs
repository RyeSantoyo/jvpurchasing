using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class Terms
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public int Days { get; set; }

        public ICollection<PO> POs { get; set; } = new List<PO>();
    }
}