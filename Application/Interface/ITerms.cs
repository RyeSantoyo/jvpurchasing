using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Models;
namespace jvPo.Application.Interface
{
    public interface ITerms
    {
        Task<IEnumerable<object>> GetTermsAsync();
        Task<(bool Success, string Message)> AddTermsAsync(Terms terms);
        Task<(bool Success, string Message)> UpdateTermsAsync(Terms terms);
        Task<(bool Success, string Message)> DeleteTermsAsync(int id);
    }
}