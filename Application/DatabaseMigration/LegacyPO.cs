using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Application.DatabaseMigration
{
    public class LegacyPO
    {
#pragma warning disable IDE1006 // Naming Styles

        public int id { get; set; }
        public int CompId { get; set; }
        public decimal PONO { get; set; }
        public DateTime date1 { get; set; }
        public decimal suppid { get; set; }
        public string suppname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string terms { get; set; } = string.Empty;
        public string requestedby { get; set; } = string.Empty;
        public double ronum { get; set; }
        public string delto { get; set; } = string.Empty;
        public DateTime date2 { get; set; }
        public decimal totalamount { get; set; }
        public string remarks { get; set; } = string.Empty;
        public string orderby { get; set; } = string.Empty;
        public string preparedby { get; set; } = string.Empty;
#pragma warning restore IDE1006 // Naming Styles
    }
}