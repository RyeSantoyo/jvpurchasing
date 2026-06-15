using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class TermsService : ITerms
    {
        private readonly ApplicationDbContext _context;

        public TermsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Message)> AddTermsAsync(Terms terms)
        {
            if(terms == null)
                return (false, "Terms cannot be null.");
            var termExist = await _context.Terms.AnyAsync(t=> t.Term == terms.Term && t.Days == terms.Days);
            if(termExist)
                return (false, "Item already exists.");
            try
            {
                var newTerms = new Terms
                {
                    Term = terms.Term,
                    Days = terms.Days
                };
                _context.Terms.Add(newTerms);
                await _context.SaveChangesAsync();
                return (true, "Terms added successfully.");
            }
            catch(Exception ex)
            {
                return (false, $"An error occurred while adding the terms: {ex.Message}");
            }

        }

        public async Task<(bool Success, string Message)> DeleteTermsAsync(int id)
        {
            var terms = await _context.Terms.FindAsync(id);
            if(terms == null) return (false, "Not valid.");

            _context.Terms.Remove(terms);
            await _context.SaveChangesAsync();
            return (true, "Terms deleted successfully.");
        }

        public async Task<IEnumerable<object>> GetTermsAsync()
        {
            var terms = await _context.Terms.Select(t=> new{t.Id, t.Term, t.Days}).ToListAsync();

            return terms;
        }

        public async Task<(bool Success, string Message)> UpdateTermsAsync(Terms terms)
        {
            if(terms == null)
                return (false, "Terms cannot be null.");
            
            var term = await _context.Terms.FindAsync(terms.Id);
            if(term == null)
                return (false, "Terms not found.");
            
            term.Term = terms.Term;
            term.Days = terms.Days;

            _context.Terms.Update(term);
            await _context.SaveChangesAsync();
            return (true, "Terms updated successfully."); 
        }
    }
}