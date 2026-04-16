using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using System.Net.Http.Json;
using System.Text.Json;

namespace SmartEMR.Infrastructure.Services;

public class AuthenticationService
{

    public static async Task<TokenResponse?> AuthenticateUserByLogin(MemberUser item)
    {
        var requestMUR = new MemberUser
        {
            MUR_Id = item.MUR_Id,
            MUR_PassWord = item.MUR_PassWord
        };

        using var client = new HttpClient();
        var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/Login/login", requestMUR, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

        if (response == null)
        {
            return null;
        }

        if (response.IsSuccessStatusCode)
        {
            var retToken = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (retToken != null)
            {
                return retToken;
            }
        }

        return null;
    }
}
