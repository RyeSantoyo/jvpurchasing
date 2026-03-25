using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jvPo.Application.Interface;
using jvPo.Infrastructure;
using jvPo.Models;
using jvPo.Models.DTO.AuthDto;
using jvPo.Models.DTO.LoginDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace jvPo.Application.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public LoginService(ApplicationDbContext context, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Users>();
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task RegisterUserAsycn(RegisterDTO dto)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(x => x.CompanyCode == dto.CompanyCode);
            if (company == null)
                throw new Exception("Company does not exist");

            var userExist = await _context.Users.FirstOrDefaultAsync(x => x.Username == dto.Username && x.CompanyId == company.Id);
            if (userExist != null)
                throw new Exception("Username already exist for this company");

            var user = new Users
            {
                CompanyId = company.Id,
                CompanyCode = dto.CompanyCode,
                Username = dto.Username,
                FullName = dto.FullName,
                Role = dto.Role,
                IsActive = true
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> LoginUserAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(us => us.Username == dto.Username);

            if (user == null)
                throw new Exception("User does not exist");

            var result = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    dto.Password
            );

            if (result != PasswordVerificationResult.Success)
                throw new Exception("Invalid username or password");

            var token = _jwtTokenGenerator.GenerateToken(user);

            return token;
        }

    }
}