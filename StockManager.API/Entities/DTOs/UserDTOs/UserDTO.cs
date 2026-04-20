using StockManager.API.Entities.Enums;

namespace StockManager.API.Entities.DTOs.UserDTOs
{
    public record CreateUserDTO(
        string Email,
        string CompleteName,
        string Password);

    public record CreatedUserDTO(
        Guid Id,
        string Email,
        string CompleteName,
        Roles Role,
        bool IsActive,
        DateTime DateTime);
}
