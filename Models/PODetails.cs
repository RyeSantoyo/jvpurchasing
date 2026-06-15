using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class PODetails
    {
    public int Id  { get; set; }

    public Company Company { get; set; } = null!;
    public int CompanyId { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public PO PurchaseOrder { get; set; } = null!;
    public int POId { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } =string.Empty;
    public decimal Price { get; set; }
    public decimal Total { get; set; }  
    }
}