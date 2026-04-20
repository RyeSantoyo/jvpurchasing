using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Application.DatabaseMigration
{
    public class LegacyPODetails
    {
#pragma warning disable IDE1006 // Naming Styles
    public int id { get; set; }
    public int PONO { get; set; }
    public int qty { get; set; }
    public string unit {get; set;} = string.Empty;
    public string xdesc { get; set; } = string.Empty;
    public decimal price {get; set;}
    public decimal total {get;set;}
#pragma warning restore IDE1006 // Naming Styles

    }
}