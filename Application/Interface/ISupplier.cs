using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;

namespace jvPo.Application.Interface
{
    public interface ISupplier
    {
        Task<IEnumerable<object>> GetSuppliersAsync();
        Task<(bool Success, string Message)> AddSupplierAsync(Suppliers supplier);
        Task<(bool Success, string Message)> UpdateSupplierAsync(Suppliers supplier);
        Task<(bool Success, string Message)> DeleteSupplierAsync(int id);

    }
}