namespace StockManager.API.Entities.DTOs.UserDTOs
{
    public record LoginDTO(string Email, string Password);

    public record AuthResponseDTO(
        string Token,
        DateTime ExpiresAt,
        string Role
    );
}
