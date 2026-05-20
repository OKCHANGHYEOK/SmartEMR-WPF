using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace SmartEMR.Infrastructure.Services;

public class AuthenticationService
{
    public static async Task<TokenResponse?> AuthenticateUserByLogin(MemberUser item)
    {
        TokenResponse retToken = new TokenResponse() { };

        var requestMUR = new MemberUser
        {
            MUR_Idx = item.MUR_Idx,
            MUR_Id = item.MUR_Id,
            MUR_PassWord = item.MUR_PassWord
        };

        try
        {
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/Login/login", requestMUR, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (response == null)
            {
                return null;
            }

            if (response.IsSuccessStatusCode == false)
            {
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        retToken.FailMessage = "존재하지 않는 사용자이거나 아이디,패스워드가 올바르지 않습니다.";
                        return retToken;
                }
            }

            retToken = await response.Content.ReadFromJsonAsync<TokenResponse>() ?? new TokenResponse();

            if (retToken == null || string.IsNullOrWhiteSpace(retToken.AccessToken))
            {
                return null;
            }

            return retToken;

        }
        catch (Exception ex)
        {
            if (ex.GetType() == typeof(HttpRequestException))
            {
                retToken.FailMessage = "서버가 작동중이지 않습니다. 잠시후 다시 시도해주세요.";
            }
            else
            {
                retToken.FailMessage = ex.Message;
            }
        }

        return retToken;
    }
}
