using SmartEMR.Domain.Entities;
namespace SmartEMR.Domain.DTOs;

public class TokenResponse
{
    public string? AccessToken { get; set; }
    public string? TokenType { get; set; }
    public double? ExpireMinutes { get; set; }
    public MemberUser? User { get; set; }
}
