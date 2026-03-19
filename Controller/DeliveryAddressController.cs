using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Application.Services;
using jvPo.Models;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryAddressController : ControllerBase
    {
        private readonly IDeliveryAddress _deliveryAddressService;
        private readonly ApplicationDbContext _context;
        public DeliveryAddressController(IDeliveryAddress deliveryAddressService, ApplicationDbContext context)
        {
            _deliveryAddressService = deliveryAddressService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliveryAddressAsync()
        {
            var deliveryAddresses = await _deliveryAddressService.GetDeliveryAddressAsync();

            return Ok(deliveryAddresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddDeliveryAddressAsync(DeliveryAddress deliveryAddress)
        {
            if(deliveryAddress == null)
                return BadRequest("Delivery address cannot be null.");
            var result = await _deliveryAddressService.AddDeliveryAddressAsync(deliveryAddress);
            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveryAddressAsync(int id, DeliveryAddress deliveryAddress)
        {
            if(id != deliveryAddress.Id)
                return BadRequest("ID mismatch.");
                
            if(deliveryAddress == null)
                return BadRequest("Delivery address cannot be null.");

            var result = await _deliveryAddressService.UpdateDeliveryAddressAsync(deliveryAddress);

            if(!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryAddressAsync(int id)
        {
            if(id <= 0)
                return BadRequest("Invalid delivery address ID.");

            var result = await _deliveryAddressService.DeleteDeliveryAddressAsync(id);

            if(!result.Success)
                return NotFound(result.Message);
            
            return Ok(result);
        }
    }
}