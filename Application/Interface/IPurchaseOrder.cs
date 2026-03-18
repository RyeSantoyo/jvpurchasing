using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
using jvPo.Models.DTO;

namespace jvPo.Application.Interface
{
    public interface IPurchaseOrder
    {
     Task<IEnumerable<object>> GetPurchaseOrdersAsync();
     Task<string> GeneratePONumberAsync();
     Task<(bool Success, string Message, string poNumber)> AddPurchaseOrderAsync(PODto dto);
     Task<(bool Sucess, string Message)> UpdatePurchaseOrderAsync (PODto dto);
     Task<(bool Success, string Message)> DeletePurchaseOrderAsync (PODto dto);
    }
}