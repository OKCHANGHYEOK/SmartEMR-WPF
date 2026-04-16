using SmartEMR.Domain.DTOs;
using SmartEMR.Domain.Entities;
using System.Net.Http.Json;

namespace SmartEMR.Infrastructure.Services;

public class AuthenticationService
{
    private static readonly DataStore dataStore = new ();


    public static async Task<DataResponse<TokenResponse>> AuthenticateUserByLogin(MemberUser item)
    {
        var retResponse = new DataResponse<TokenResponse>() { IsSuccess = false };

        var requestMUR = new MemberUser
        {
            MUR_Id = item.MUR_Id,
            MUR_PassWord = item.MUR_PassWord
        };

        var response = await dataStore.PostAsync(dataStore.APIUrl + "/Login/login", requestMUR);

        if (response == null) {
            return retResponse;
        }

        if (response.IsSuccessStatusCode)
        {
            var retToken = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (retToken != null)
            {
                retResponse.Item = retToken;
                retResponse.IsSuccess = true;
            }
        }

        return retResponse;
    }
}
