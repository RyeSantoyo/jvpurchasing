using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class PODetails
    {
    public int Id  { get; set; } 
    public int CompanyId { get; set; }
    public PO PurchaseOrder { get; set; } = null!;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } =string.Empty;
    public double Price { get; set; }
    public double Total { get; set; }  
    }
}