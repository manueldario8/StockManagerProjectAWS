using Microsoft.AspNetCore.Identity;
using StockManager.API.Data;
using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Entities.Models.Users;
using StockManager.API.Interfaces.AuthInterfaces;
using StockManager.API.Middlewares.DomainExceptions;
using Microsoft.EntityFrameworkCore;

namespace StockManager.API.Services.AuthServices
{
    public class UserService(DataBaseContext context) : IUserService
    {
        private readonly DataBaseContext _context = context;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public async Task<CreatedUserDTO> CreateUser(CreateUserDTO dto)
        {
            var email = dto.Email.Trim().ToLower();
            var name = dto.CompleteName;
            await ValidateUser(email, dto.Password);

            var user = new User
            {
                Email = email,
                CompleteName = name
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new CreatedUserDTO(user.Id, user.Email, user.CompleteName, user.Role, user.IsActive, user.CreatedAt);
        }


        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }



        private async Task ValidateUser(string email, string password)
        {

            if (string.IsNullOrEmpty(email)) throw new ValidationException("Se requiere un email para continuar");
            if (string.IsNullOrEmpty(password)) throw new ValidationException("La contraseña no puede estar vacía");
            if (password.Length < 6) throw new ValidationException("La contraseña no puede tener menos de 6 caracteres");

            var emailUsed = await _context.Users.AnyAsync(p => p.Email == email);
            if (emailUsed)
                throw new ConflictException($"El mail '{email}' ya está asignado a otro usuario.");
        }
    }
}
