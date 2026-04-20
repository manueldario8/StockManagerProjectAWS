using StockManager.API.Entities.DTOs.UserDTOs;

namespace StockManager.API.Interfaces.AuthInterfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO dto);

    }
}
