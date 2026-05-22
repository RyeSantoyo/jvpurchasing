using DevExpress.XtraReports;
using jvPo.Application.Interface;
using jvPo.Models;
using jvPo.Models.DTO;
using jvPo.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
            if (dto == null)
                return (false, "Invalid Data", "");


            var supplier = await _context.Suppliers.FindAsync(dto.SupplierId);
            var terms = await _context.Terms.FindAsync(dto.TermsId);
            var company = await _context.Companies.FindAsync(dto.CompanyId);

            if (supplier == null)
                return (false, "Supplier Does not exist.", "");
            Console.WriteLine($"Received JSON:{JsonSerializer.Serialize(dto)} ");
            if (company == null)
                return (false, "It does not exist.", "");
            if (terms == null)
                return (false, "It does not exist.", "");

            if (dto.PODetails == null || dto.PODetails.Count == 0)
                return (false, "No data available.", "");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var poNumber = await GeneratePONumberAsync();
                var newPo = new PO
                {
                    CompanyId = dto.CompanyId,
                    CompanyCode = company?.CompanyCode ?? string.Empty,

                    PONumber = poNumber,
                    PODate = dto.PODate,

                    SupplierId = dto.SupplierId,
                    SupplierName = supplier?.SupplierName ?? string.Empty,
                    SupplierAddress = supplier?.SupplierAddress ?? string.Empty,

                    DeliveryAddressID = dto.DeliveryId,
                    DeliveryAddress = dto.DeliveryAddress,
                    TermsId = dto.TermsId,
                    AgreedTerms = terms?.Term ?? string.Empty,
                    OrderBy = dto.OrderBy,
                    RequestedBy = dto.RequestedBy,
                    RONumber = dto.RONumber,
                    RODate = dto.RODate,
                    TotalAmount = dto.PODetails.Sum(d => d.Quantity * d.Price),
                    Remarks = dto.Remarks,
                    PODetails = dto.PODetails.Select(d => new PODetails
                    {
                        CompanyId = d.CompanyId,
                        CompanyCode = d.CompanyCode,
                        PONumber = poNumber,
                        Quantity = d.Quantity,
                        Price = d.Price,
                        Unit = d.Unit,
                        Description = d.Description,
                        Total = d.Quantity * d.Price
                    }).ToList()
                };
                Console.WriteLine(dto.PODetails.Count());
                await _context.POs.AddAsync(newPo);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"Order submitted", poNumber);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                await transaction.RollbackAsync();
                return (false, $"Error occurred {ex.Message}", "");
            }

        }

        public async Task<(bool Success, string Message)> DeletePurchaseOrderAsync(int id)
        {
            var po = await _context.POs.FindAsync(id);

            if (po == null)
                return (false, "Cannot be found");

            _context.POs.Remove(po);
            await _context.SaveChangesAsync();
            return (true, "PO Successfully deleted");
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
            return $"PO-{lastNumber + 1:D5}";
        }

        public async Task<object?> GetPOByIdAsync(int id)
        {
            return await _context.POs
                                    .Where(po => po.Id == id)
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
                                        po.RequestedBy,
                                        po.OrderBy,
                                        po.RONumber,
                                        po.RODate,
                                        po.TotalAmount,
                                        po.Remarks,
                                        PODetails = po.PODetails.Select(d => new
                                        {
                                            d.Id,
                                            d.POId,
                                            d.PONumber,
                                            d.CompanyId,
                                            d.CompanyCode,
                                            d.Quantity,
                                            d.Unit,
                                            d.Description,
                                            d.Price,
                                            d.Total
                                        })
                                    })
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(p => p.Id == id);



        }

        public async Task<IEnumerable<object>> GetPODetailsAsync()
        {

            var pod = await _context.PODetails.Select(d => new
            {
                d.Id,
                d.POId,
                d.PONumber,
                d.CompanyId,
                d.CompanyCode,
                d.Quantity,
                d.Unit,
                d.Description,
                d.Price,
                d.Total
            }).OrderBy(d => d.PONumber).ToListAsync();
            return pod;

        }

        public Task<IEnumerable<object>> GetPODetailsAsyncId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PODto>> GetPurchaseOrdersAsync()
        {
            return await _context.POs
            .Select(po => new PODto
            {
                POID= po.Id,
                CompanyId = po.CompanyId,
                CompanyCode = po.CompanyCode,
                PONumber = po.PONumber,
                PODate = po.PODate,
                SupplierId = po.SupplierId,
                DeliveryId = po.DeliveryAddressID,
                DeliveryAddress = po.DeliveryAddress,
                TermsId = po.TermsId,
                AgreedTerms = po.AgreedTerms,
                RequestedBy = po.RequestedBy,
                OrderBy = po.OrderBy,
                RONumber = po.RONumber,
                RODate = po.RODate,
                TotalAmount = po.TotalAmount,
                Remarks = po.Remarks,
                PODetails = po.PODetails.Select(d=> new PODetailsDTO
                {
                     POId = d.POId,
                     CompanyId = d.CompanyId,
                     CompanyCode = d.CompanyCode,
                     Quantity = d.Quantity,
                     Unit = d.Unit,
                     Description = d.Description,
                     Price = d.Price,
                     Total = d.Total,
                     PONumber = d.PONumber
                }).ToList()
            }).ToListAsync();
        }

                //         PODetails = po.PODetails.Select(detail => new
                // {
                //     detail.Id,
                //     detail.Quantity,
                //     detail.Unit,
                //     detail.Description,
                //     detail.CompanyId,
                //     detail.POId,
                //     detail.CompanyCode,
                //     detail.Price,
                //     detail.Total,
                //     detail.PONumber
                // }).ToList()

        public ViewPODetails PreviewPo(string poNumber)
        {
            var report = new ViewPODetails();
            report.Parameters["PONumber"].Value = poNumber;
            report.Parameters["PONumber"].Visible = false;

            report.RequestParameters = false;

            return report;
        }

        public async Task<(bool Sucess, string Message)> UpdatePurchaseOrderAsync(PODto dto)
        {
            if (dto == null)
                return (false, "Not available");

            var pod = await _context.POs.FindAsync(dto.POID);
            if (pod == null)
                return (false, "Not available");

            return (true, "Success");
        }


    }
}