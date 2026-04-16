using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Core;

public class ApplicationSession : ITokenProvider
{
    public MemberUser MemberUser = new();
    public TokenResponse token = new();

    public TokenResponse GetToken()
    {
        return token;
    }

    public void SetToken(TokenResponse token)
    {
        this.token.AccessToken = token.AccessToken;
        this.token.TokenType = token.TokenType;
        this.token.ExpireMinutes = token.ExpireMinutes;
    }
}