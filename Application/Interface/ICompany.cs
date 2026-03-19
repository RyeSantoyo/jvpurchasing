using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;

namespace jvPo.Application.Interface
{
    public interface ICompany
    {
        Task<IEnumerable<object>> GetCompanyAsync();
        Task<(bool Success, string Message)> AddCompanyAsync(Company company);
        Task<(bool Success, string Message)> UpdateCompanyAsync(Company company);
        Task<(bool Success, string Message)> DeleteCompanyAsync (int id);
    }
}