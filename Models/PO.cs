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
        public int CompId { get; set; } //Company Id
        public int PONumber { get; set; }
        public DateTime Date1 { get; set; }
        [ForeignKey("SupplierId")]
        public Suppliers Supplier { get; set; } = null!;
        public int SupplierId { get; set; }

        public DeliveryAddress Address { get; set; } = null!;
        public int DelAddId { get; set; }

        public Terms Terms { get; set; } = null!;
        public int TermsId { get; set; }

        public Users Users { get; set; } = null!;
        public int UsersId { get; set; }
        public string RequestedBy { get; set; } = string.Empty; 
        public int RONumber { get; set; }

    }
}