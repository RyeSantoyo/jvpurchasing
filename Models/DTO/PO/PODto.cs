using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jvPo.Models.DTO
{
    public class PODto
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; } = string.Empty;
        public string PONumber { get; set; } = string.Empty;
        public DateTime PODate { get; set; } = DateTime.UtcNow;
        public int SupplierId { get; set; }
        public int DeliveryId { get; set; }
        public string DeliveryAddress { get; set; } =string.Empty;
        public int TermsId { get; set; }
        public string AgreedTerms { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string RequestedBy { get; set; } = string.Empty; //Can be anyone
        public string OrderBy { get; set; } = string.Empty;
        public int RONumber { get; set; } //Request Order
        public DateTime RODate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public List<PODetailsDTO> PODetails { get; set; } = new List<PODetailsDTO>();
    }
}