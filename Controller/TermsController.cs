using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class TermsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITerms _termsService;
        public TermsController(ApplicationDbContext context, ITerms termsService){
            _context = context;
            _termsService = termsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTermsAsync()
        {
            var terms = await _termsService.GetTermsAsync();
            return Ok(terms);
        }
        [HttpPost]
        public async Task<IActionResult> AddTermsAsync(Terms terms)
        {
            if (terms == null)
                return BadRequest("Terms cannot be null.");
            
            var result = await _termsService.AddTermsAsync(terms);
            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTermsAsync(int id, Terms terms)
        {
            if(id != terms.Id)
                return BadRequest("ID Mismatch");
            
            if(terms == null)
                return BadRequest("Terms cannot be null.");
            
            var result = await _termsService.UpdateTermsAsync(terms);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTermsAsync(int id)
        {
            if(id <= 0)
                return BadRequest("Invalid ID.");
            var result = await _termsService.DeleteTermsAsync(id);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
    }
}