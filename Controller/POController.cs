using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Models;
using jvPo.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class POController : ControllerBase
    {
        private readonly IPurchaseOrder _purchaseOrderService;
        private readonly ApplicationDbContext _context;

        public POController(IPurchaseOrder purchaseOrderService, ApplicationDbContext context)
        {
            _purchaseOrderService = purchaseOrderService;
            _context = context;
        }
        [HttpGet("pos")]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var purchaseOrders = await _purchaseOrderService.GetPurchaseOrdersAsync();
            return Ok(purchaseOrders);
        }

        [HttpGet("po-deets")]
        public async Task<IActionResult> GetPODetails()
        {
            var poDetails = await _purchaseOrderService.GetPODetailsAsync();
            return Ok(poDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseOrderAsync(PODto dto)
        {
            if(dto == null)
                return BadRequest("PO is empty.");
            
            var result = await _purchaseOrderService.AddPurchaseOrderAsync(dto);
            if(!result.Success)
                return BadRequest(result.Message.ToString());
            
            return Ok(result.Message);
        }

        [HttpDelete]

        public async Task<IActionResult> DeletePurchaseOrderAsync(int id)
        {
            if(id<=0)
                return BadRequest("Not available");
            var result = await _purchaseOrderService.DeletePurchaseOrderAsync(id);

            if(!result.Success)
                return NotFound(result.Message);
            
            return Ok();
            
        }
    }
}