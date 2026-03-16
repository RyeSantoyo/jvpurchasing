using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using jvPo.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class PurchaseOrderService : IPurchaseOrder
    {

        private readonly ApplicationDbContext _context;
        public PurchaseOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message, string poNumber)> AddPurchaseOrderAsync(PODto dto)
        {
            if(dto == null)
                return(false, "Invalid Data", "");

                var supplierExists = await _context.Suppliers.AnyAsync(s=> s.Id == dto.SupplierId);
                var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            if(!supplierExists)
                return (false, "Supplier Does not exist.", "");
            Console.WriteLine($"Received JSON:{JsonSerializer.Serialize(dto)} " );

            if(dto.PODetails == null || dto.PODetails.Count == 0 )
                return (false, "No data available.", "");

            try
            {
                var poNumber = await GeneratePONumberAsync();
                var newPo = new PO
                {
                    PONumber = poNumber,
                    SupplierId = dto.SupplierId,
                    SupplierName = supplier?.SupplierName
                    
                };
            }
            catch
            {
                
            }

            return (true, "","");
        }

        public async Task<string> GeneratePONumberAsync()
        {
            var lastPo = await _context.POs
            .OrderByDescending(po => po.Id)
            .FirstOrDefaultAsync();
            int lastNumber = 0;
            if (lastPo != null)
            {
             var parts = lastPo.PONumber.ToString().Split('-');

                if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                {
                    lastNumber = number;
                }
            }
                return $"PO-{lastNumber + 1 :D5}";
        }

        public async Task<IEnumerable<object>> GetPurchaseOrdersAsync()
        {
            var pos = await _context.POs
            .Include(po => po.Supplier)
            .Include(po => po.Terms)
            .Include(po => po.Address)
            .Include(po => po.PODetails)
            .Include(po => po.User)
            .Select(po => new
            {
                po.Id,
                po.PONumber,
                po.PODate,
                po.SupplierId,
                po.Supplier.SupplierName,
                po.Terms.Term,
                po.DeliveryAddressID,
                DeliveryAddress = po.Address.Address,
                po.UserId,
                UserName = po.User.Username,
                po.RequestedBy,
                po.OrderBy,
                po.RONumber,
                po.RODate,
                po.TotalAmount,
                po.Remarks,
                PODetails = po.PODetails.Select(detail => new
                {
                    detail.Id,
                    detail.Quantity,
                    detail.Unit,
                    detail.Description,
                    detail.CompanyId,
                    detail.POId,
                }).ToList()
            })
            .ToListAsync();

            return pos;
        }
    }
}