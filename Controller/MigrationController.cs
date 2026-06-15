using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MigrationController : ControllerBase
    {
        private readonly IDataMigration _dataMigration;

        public MigrationController(IDataMigration dataMigration)
        {
            _dataMigration = dataMigration;
        }

        [HttpPost("migrate-po")]
        public async Task<IActionResult> MigratePO()
        {
            try
            {
                var totalProcessed = await _dataMigration.MigratePOAsync();
                return Ok(new { message = "Migration Successful", totalProcessed });
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(500, new { message = "Migration Failed", error = ex.Message });
            }
        }
        [HttpPost("migrate-podetails")]

        public async Task<IActionResult> MigratePOAsync()
        {
            try
            {
                var totalProcessed = await _dataMigration.MigratePODetailsAsync();
                return Ok(new {message = "Migration Successful", totalProcessed});
            }
            catch(Exception ex)
            {
                return StatusCode(500, new {message = "Migration Failed", error = ex.Message});
            }
        }



    }
}