using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Services;
using jvPo.Models;
using jvPo.Models.DTO.AuthDto;
using jvPo.Models.DTO.LoginDto;
using Microsoft.AspNetCore.Mvc;

namespace jvPo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterDTO reg)
        {
            try
            {
                await _loginService.RegisterUserAsycn(reg);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto login)
        {

            var user = await _loginService.LoginUserAsync(login);

            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
    }
}