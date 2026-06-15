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
     Task<IEnumerable<PODto>> GetPurchaseOrdersAsync(int pageNumber, int pageSize);
     Task<object?> GetPOByIdAsync(int id);
     Task<string> GeneratePONumberAsync();
     Task<(bool Success, string Message, string poNumber)> AddPurchaseOrderAsync(PODto dto);
     Task<(bool Sucess, string Message)> UpdatePurchaseOrderAsync (PODto dto);
     Task<(bool Success, string Message)> DeletePurchaseOrderAsync (int id);
     Task<IEnumerable<object>> GetPODetailsAsync();
     Task<IEnumerable<object>> GetPODetailsAsyncId(int id);
     ViewPODetails PreviewPo(string poNumber);
    }
}