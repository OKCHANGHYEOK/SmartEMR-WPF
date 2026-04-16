using SmartEMR.Domain.DTOs;
namespace SmartEMR.Infrastructure;

public interface ITokenProvider
{
    TokenResponse? GetToken();
}
