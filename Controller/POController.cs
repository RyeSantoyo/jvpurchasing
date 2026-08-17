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


        [HttpGet("purchaseorder")] 
        public async Task<IActionResult> GetPurchaseOrders(
            [FromQuery] int draw,
            [FromQuery] int start,
            [FromQuery] int length,
            [FromQuery(Name = "search[value]")] string searchValue = "")
        {
            int pageSize = length > 0 ? length : 10;
            int pageNumber = (start / pageSize) + 1;

            var (data, totalRecords, filteredRecords) = await _purchaseOrderService.GetPurchaseOrdersAsync(
                pageNumber, pageSize, searchValue);

            var response = new DataTableResponse<PODto>
            {
                Draw = draw,
                RecordsTotal = totalRecords,
                RecordsFiltered = filteredRecords,
                Data = data.ToList()
            };

            return Ok(response);

        }
 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPurchaseOrderById(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPOByIdAsync(id);
            if (purchaseOrder == null)
                return NotFound("PO not found.");

            return Ok(purchaseOrder);
        }

        [HttpGet("po-details")]
        public async Task<IActionResult> GetPODetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 200)
        {
            if (pageSize > 100)
                pageSize = 100;
            var poDetails = await _purchaseOrderService.GetPODetailsAsync(pageNumber, pageSize);
            return Ok(poDetails);
        }
        [HttpGet("po-details/{id}")]
        public async Task<IActionResult> GetPODetailsById(int id)
        {
            var podeets = await _purchaseOrderService.GetPODetailsAsyncId(id);
            if (podeets == null) return NotFound("PO details not found.");
            return Ok(podeets);
        }
        
        [HttpGet("generatePo")]
        public async Task<IActionResult> GeneratePoNumber()
        {
            string nextPoNumber = await _purchaseOrderService.GeneratePONumberAsync();
            return new JsonResult (new {poNumber = nextPoNumber});
        }

        [HttpPost]
        public async Task<IActionResult> AddPurchaseOrderAsync(PODto dto)
        {
            if (dto == null)
                return BadRequest("PO is empty.");

            if(!ModelState.IsValid)
                return BadRequest();

            var result = await _purchaseOrderService.AddPurchaseOrderAsync(dto);
            if (!result.Success)
                return BadRequest(result.Message.ToString());

            return Ok(result.Message);
        }

        [HttpDelete]

        public async Task<IActionResult> DeletePurchaseOrderAsync(int id)
        {
            if (id <= 0)
                return BadRequest("Not available");
            var result = await _purchaseOrderService.DeletePurchaseOrderAsync(id);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok();

        }
    }
}