using StockManager.API.Entities.DTOs.UserDTOs;
using StockManager.API.Entities.Models.Users;

namespace StockManager.API.Interfaces.AuthInterfaces
{
    public interface IUserService
    {
        Task<CreatedUserDTO> CreateUser(CreateUserDTO dto);
        Task<User?> GetByEmailAsync(string email);
    }
}
