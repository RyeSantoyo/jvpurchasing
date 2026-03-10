using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Services;
using jvPo.Application.Interface;
using jvPo.Models;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        private readonly ISupplier _supplierService;
        public SuppliersController(ApplicationDbContext context, ISupplier supplierService)
        {
            _context = context;
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _supplierService.GetSuppliersAsync();
            return Ok(suppliers);
        }

        [HttpPost]
        public async Task<IActionResult> AddSupplierAsync(Suppliers supplier)
        {
            if(supplier == null)
                return BadRequest("Supplier cannot be null.");

            var result = await _supplierService.AddSupplierAsync(supplier);
            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplierAsync(int id, Suppliers supplier)
        {
            if(id != supplier.Id)
                return BadRequest("ID mismatch.");
            
            if(supplier == null)
                return BadRequest("Supplier cannot be null.");

            var result = await _supplierService.UpdateSupplierAsync(supplier);
            if(!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

    }
}