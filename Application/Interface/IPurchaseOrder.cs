using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
using jvPo.Models.DTO;
using jvPo.Report;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Application.Interface
{
    public interface IPurchaseOrder
    {
        // Task<IEnumerable<PODto>> GetPurchaseOrdersAsync(int pageNumber, int pageSize);

        Task<(IEnumerable<PODto> Data, int TotalRecords, int FilteredRecords)> GetPurchaseOrdersAsync
        (int pageNumber, int pageSize, string searchValue);
        Task<object?> GetPOByIdAsync(int id);
        Task<string> GeneratePONumberAsync();
        Task<(bool Success, string Message, string poNumber)> AddPurchaseOrderAsync(PODto dto);
        Task<(bool Success, string Message)> UpdatePurchaseOrderAsync(PODto dto);
        Task<(bool Success, string Message)> DeletePurchaseOrderAsync(int id);
        Task<IEnumerable<PODetailsDTO>> GetPODetailsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<PODetailsDTO>> GetPODetailsAsyncId(int id);
        //ViewPODetails PreviewPo(string poNumber);

            Task<(bool Success, string Message, ViewPODetails Report)> PreviewPo(string poNumber);
    }
}