using DevExpress.Data.Filtering.Helpers;
using DevExpress.XtraReports;
using jvPo.Application.Interface;
using jvPo.Models;
using jvPo.Models.DTO;
using jvPo.Report;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                return (false, "Company does not exist.", "");
                
            if (terms == null)
                return (false, "Terms does not exist.", "");

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

                    DeliveryAddressID = dto.DeliveryAddressId,
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
            var poNumbers = await _context.POs
                                        .OrderByDescending(p => p.PODate)
                                        .ThenByDescending(p => p.Id)
                                        .Take(50)
                                        .Select(p => p.PONumber)
                                        .ToListAsync();

            int lastSequence = 0;

            foreach(var rawPo in poNumbers)
            {
            if(int.TryParse(rawPo?.Trim(), out int parsedNum)){
                    if(parsedNum > lastSequence)
                    {
                        lastSequence = parsedNum;
                        break;
                    }
                }
            }
            int nextNumber = lastSequence > 0 ? lastSequence + 1 :1001;

            return nextNumber.ToString();
        }

        public async Task<object?> GetPOByIdAsync(int id)
        {
            return await _context.POs
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
                                    .FirstOrDefaultAsync(po => po.Id == id);

        }

        public async Task<IEnumerable<PODetailsDTO>> GetPODetailsAsync(int pageNumber, int pageSize)
        {
            var pod = _context.PODetails
                .AsNoTracking()
                .GroupBy(d => new { d.POId, d.PONumber, d.CompanyId, d.CompanyCode, d.Description, d.Unit })
                .Select(g => new PODetailsDTO
                {
                    POId = g.Key.POId,
                    PONumber = g.Key.PONumber,
                    CompanyId = g.Key.CompanyId,
                    CompanyCode = g.Key.CompanyCode,
                    Total = g.Sum(x => x.Total),
                    Quantity = g.Sum(x => x.Quantity),
                    Unit = g.Key.Unit ?? "N/A",
                    Description = g.Key.Description,
                    Price = g.Average(x => x.Price)
                });

            var result = await pod.OrderByDescending(d => d.POId)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            foreach (var item in result)
            {
                item.Unit = item.Unit.Trim();
                item.Description = item.Description.Trim();
            }

            return result;
        }

        public async Task<IEnumerable<PODetailsDTO>> GetPODetailsAsyncId(int id)
        {
            throw new NotImplementedException();
        }
#region GetPO
        // public async Task<IEnumerable<PODto>> GetPurchaseOrdersAsync(int pageNumber, int pageSize)
        // {

        //     return await _context.POs
        //     .AsNoTracking()
        //     .OrderByDescending(po => po.PODate)
        //     .ThenByDescending(po => po.Id)
        //     .Skip((pageNumber - 1) * pageSize)
        //     .Take(pageSize)
        //     .Select(po => new PODto
        //     {
        //         POID = po.Id,
        //         CompanyId = po.CompanyId,
        //         CompanyCode = po.CompanyCode,
        //         PONumber = po.PONumber,
        //         PODate = po.PODate,
        //         SupplierId = po.SupplierId,
        //         DeliveryId = po.DeliveryAddressID,
        //         DeliveryAddress = string.IsNullOrEmpty(po.Address.Address) ? "N/A" : po.Address.Address.Trim(),
        //         TermsId = po.TermsId,
        //         AgreedTerms = string.IsNullOrEmpty(po.Terms.Term) ? "N/A" : po.Terms.Term.Trim(),
        //         RequestedBy = po.RequestedBy,
        //         OrderBy = po.OrderBy,
        //         RONumber = po.RONumber,
        //         RODate = po.RODate,
        //         TotalAmount = po.TotalAmount,
        //         Remarks = po.Remarks,

        //     })
        //     .ToListAsync();

        // }

        // public async Task<(IEnumerable<PODto> Data, int TotalRecord, int FilteredRecord)> GetPurchaseOrdersAsync(int skip, int take, string? searchValue, string? sortColumn, string? sortDirection)
        // {
        //     var query = _context.POs.AsNoTracking();

        //     int totalRecords = await query.CountAsync();

        //     if (!string.IsNullOrEmpty(searchValue))
        //     {
        //         searchValue = searchValue.ToLower();

        //         query = query.Where(po =>
        //         po.PONumber.ToLower().Contains(searchValue) ||
        //         po.RequestedBy.ToLower().Contains(searchValue) ||
        //         (po.Address != null && po.Address.Address.ToLower().Contains(searchValue))
        //         );

        //     }

        //     int recordsFiltered = await query.CountAsync();

        //     if (!string.IsNullOrWhiteSpace(sortColumn) && !string.IsNullOrWhiteSpace(sortDirection))
        //     {
        //         bool isDescending = sortDirection.ToLower() == "desc";
        //         query = sortColumn.ToLower() switch
        //         {
        //             "ponumber" => isDescending ? query.OrderByDescending(po => po.PONumber) : query.OrderBy(po => po.PONumber),
        //             "podate" => isDescending ? query.OrderByDescending(po => po.PODate) : query.OrderBy(po => po.PODate),
        //             "totalamount" => isDescending ? query.OrderByDescending(po => po.TotalAmount) : query.OrderBy(po => po.TotalAmount),
        //             _ => isDescending ? query.OrderByDescending(po => po.Id) : query.OrderBy(po => po.Id)
        //         };
        //     }
        //     else
        //     {
        //         query = query.OrderByDescending(po => po.Id);
        //     }

        //     var data = await query.Skip(skip).Take(take)
        //         .Select(po => new PODto
        //         {
        //             POID = po.Id,
        //             CompanyId = po.CompanyId,
        //             CompanyCode = po.CompanyCode,
        //             PONumber = po.PONumber,
        //             PODate = po.PODate,
        //             SupplierId = po.SupplierId,
        //             DeliveryId = po.DeliveryAddressID,
        //             DeliveryAddress = string.IsNullOrEmpty(po.Address.Address) ? "N/A" : po.Address.Address.Trim(),
        //             TermsId = po.TermsId,
        //             AgreedTerms = po.AgreedTerms,
        //             RequestedBy = po.RequestedBy,
        //             OrderBy = po.OrderBy,
        //             RONumber = po.RONumber,
        //             RODate = po.RODate,
        //             TotalAmount = po.TotalAmount,
        //             Remarks = po.Remarks
        //         }).ToListAsync();
        //     return (data, totalRecords, recordsFiltered);
        // }
#endregion

        public async Task<(IEnumerable<PODto> Data, int TotalRecords, int FilteredRecords)> GetPurchaseOrdersAsync(int pageNumber, int pageSize, string searchValue)
        {
            var query = _context.POs.AsNoTracking();

            int totalRecords = await query.CountAsync();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string search = searchValue.Trim().ToLower();
                query = query.Where(po =>
                    (po.PONumber != null && po.PONumber.ToLower().Contains(search)) ||
                    (po.RequestedBy != null && po.RequestedBy.ToLower().Contains(search)) ||
                    (po.Address != null && po.Address.Address != null && po.Address.Address.ToLower().Contains(search)) ||
                    (po.Supplier != null && po.Supplier.SupplierName != null && po.Supplier.SupplierName.ToLower().Contains(search))
                );
            }

            int filteredRecords = await query.CountAsync();

            var data = await query
                .OrderByDescending(po => po.PODate)
                .ThenByDescending(po => po.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(po => new PODto
                {
                    POID = po.Id,
                    CompanyId = po.CompanyId,
                    CompanyCode = po.CompanyCode,
                    PONumber = po.PONumber,
                    PODate = po.PODate,
                    SupplierId = po.SupplierId,
                    DeliveryAddressId = po.DeliveryAddressID,
                    DeliveryAddress = string.IsNullOrEmpty(po.Address.Address) ? "N/A" : po.Address.Address.Trim(),
                    TermsId = po.TermsId,
                    AgreedTerms = string.IsNullOrEmpty(po.Terms.Term) ? "N/A" : po.Terms.Term.Trim(),
                    RequestedBy = po.RequestedBy,
                    OrderBy = po.OrderBy,
                    RONumber = po.RONumber,
                    RODate = po.RODate,
                    TotalAmount = po.TotalAmount,
                    Remarks = po.Remarks,
                }).ToListAsync();
            return (data, totalRecords, filteredRecords);
        }



        public ViewPODetails PreviewPo(string poNumber)
        {
            var report = new ViewPODetails();
            report.Parameters["PONumber"].Value = poNumber;
            report.Parameters["PONumber"].Visible = false;

            report.RequestParameters = false;

            return report;
        }

        public async Task<(bool Success, string Message)> UpdatePurchaseOrderAsync(PODto dto)
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