using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StockManager.API.Data;
using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Entities.Enums;
using StockManager.API.Entities.Models.Users;
using StockManager.API.Interfaces.AuthInterfaces;
using StockManager.API.Middlewares.DomainExceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StockManager.API.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly DataBaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(
            DataBaseContext context,
            IConfiguration configuration,
            IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO dto)
        {
            var email = dto.Email.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email)
                ?? throw new NotFoundException("Credenciales inválidas");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                throw new AccessDeniedException("Credenciales inválidas");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, ((Roles)user.Role).ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpiresMinutes"]!)
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseDTO(
                new JwtSecurityTokenHandler().WriteToken(token),
                expires, user.Role.ToString());
        }

    }
}
