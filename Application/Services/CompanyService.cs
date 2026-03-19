using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class CompanyService : ICompany
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> AddCompanyAsync(Company company)
        {
            if(company == null)
                return (false, "Not available");
            
            var companyExist = await _context.Companies.AnyAsync(x=> x.CompanyCode == company.CompanyCode);
            if(companyExist)
                return (false, "Item already exists");
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newCompany = new Company
                {
                    Id = company.Id,
                    CompanyCode = company.CompanyCode,
                    Name = company.Name
                };

                await _context.AddAsync(newCompany);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, $"Item added successfully");             
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool Success, string Message)> DeleteCompanyAsync(int id)
        {
            var compExist = await _context.Companies.FindAsync(id);
                if(compExist == null)
                    return (false, "Does not exist");
            
            _context.Companies.Remove(compExist);
            await _context.SaveChangesAsync();
            return (true, "Company deleted");
        }

        public async Task<IEnumerable<object>> GetCompanyAsync()
        {
            var comp = await _context.Companies
                                     .Select(x=> new {x.Id, x.CompanyCode, x.Name}).ToListAsync();
            return comp;
        }

        public async Task<(bool Success, string Message)> UpdateCompanyAsync(Company company)
        {
            if(company == null)
                return (false, "Item does not exist");

            var exist = await _context.Companies.FindAsync(company.Id);
            if(exist == null)
                return (false, "This field cannot return null");

            exist.CompanyCode = company.CompanyCode;
            exist.Name = company.Name;

           await _context.SaveChangesAsync();
            return (true, "Field updated");
        }
    }
}