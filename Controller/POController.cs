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
    public class POController : ControllerBase
    {
        private readonly IPurchaseOrder _purchaseOrderService;
        private readonly ApplicationDbContext _context;

        public POController(IPurchaseOrder purchaseOrderService, ApplicationDbContext context)
        {
            _purchaseOrderService = purchaseOrderService;
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetPurchaseOrders()
        {
            var purchaseOrders = await _purchaseOrderService.GetPurchaseOrdersAsync();
            return Ok(purchaseOrders);
        }
    }
}