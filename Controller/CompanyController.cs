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
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompany _companyService;

        public CompanyController (ICompany companyService, ApplicationDbContext context)
        {
            _context = context;
            _companyService = companyService;
        }

        [HttpGet("companies")]
        public async Task<ActionResult<IEnumerable<object>>> GetCompanyAsync()
        {
            var company = await _companyService.GetCompanyAsync();

            return Ok(company);
        }
        [HttpPost]
        public async Task<IActionResult> AddCompanyAsync(Company company)
        {
            if(company==null)
                return BadRequest("Item cannot be null");

            var result = await _companyService.AddCompanyAsync(company);
                if(!result.Success)
                    return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompanyAsync(int id, Company company)
        {
            if(id !=company.Id)
                return BadRequest("ID Mismatch");

            if(company==null)
                return BadRequest("This field cannot be null");

            var result = await _companyService.UpdateCompanyAsync(company);

            if(!result.Success)
                return BadRequest(result.Message);   

            return Ok(result);
        }
        [HttpDelete]

        public async Task<IActionResult> DeleteCompanyAsync (int id)
        {
            var comp = await _context.Companies.FindAsync(id);
            if(comp == null)
                return BadRequest("Not available");

            var result = await _companyService.DeleteCompanyAsync(id);
            if(!result.Success)
                return BadRequest(result.Message);
            
            return Ok($"{id} has been deleted");
        }
    }
}