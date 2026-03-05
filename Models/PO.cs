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
        public int PONumber { get; set; }
        public DateTime PODate { get; set; } = DateTime.UtcNow;
        [ForeignKey("SupplierId")]
        public Suppliers Supplier { get; set; } = null!;
        public int SupplierId { get; set; }

        //Farms and Offices
        [ForeignKey("DeliveryAddressID")]
        public DeliveryAddress Address { get; set; } = null!;
        public int DeliveryAddressID { get; set; }

        [ForeignKey("TermsId")]
        public Terms Terms { get; set; } = null!;
        public int TermsId { get; set; }

        [ForeignKey("UserId")]
        public Users User { get; set; } = null!;
        public int UserId { get; set; }


        public string RequestedBy { get; set; } = string.Empty; // Link to User 
        public int RONumber { get; set; } //Request Order
        public DateTime RODate { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public string OrderBy { get; set; } = string.Empty;

    }

}
