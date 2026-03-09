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
        public async Task<ActionResult<IEnumerable<object>>> GetDeliveryAddressAsync()
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
    }
}