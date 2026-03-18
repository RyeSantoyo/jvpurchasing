using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models
{
    public class PO
    {
        public int Id { get; set; }
        public Company Company { get; set; } = null!;
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public DateTime PODate { get; set; } = DateTime.UtcNow;
        [ForeignKey("SupplierId")]
        public Suppliers Supplier { get; set; } = null!;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierAddress { get; set; } = string.Empty;

        //Farms and Offices
        [ForeignKey("DeliveryAddressID")]
        public DeliveryAddress Address { get; set; } = null!;
        public int DeliveryAddressID { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;

        [ForeignKey("TermsId")]
        public Terms Terms { get; set; } = null!;
        public int TermsId { get; set; }
        public string AgreedTerms { get; set; } = string.Empty;

        // [ForeignKey("UserId")]
        // public Users User { get; set; } = null!;
        // public int UserId { get; set; }


        public string RequestedBy { get; set; } = string.Empty; //Can be anyone
        public string OrderBy { get; set; } = string.Empty;
        public int RONumber { get; set; } //Request Order
        public DateTime RODate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;

        public ICollection<PODetails> PODetails { get; set; } = new List<PODetails>();
    }

}
