using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartEMR.Domain.Enums;
using SmartEMR.Domain.DTOs;

namespace SmartEMR.Infrastructure;

public class DataStore
{
    private readonly HttpClient _client = new HttpClient();
    private readonly ITokenProvider _tokenProvider;

    public event EventHandler<DataStoreErrorEventArgs>? ErrorOccured;

    public string APIUrl { get; set; } = "http://127.0.0.1:8000/";

    // API 응답 상태를 저장하는 속성들
    public string? retMessage { get; set; }
    public int? retStatusCode { get; set; } // eResponseCode에 맞게 int로 변경 권장
    public int? retCount { get; set; }
    public bool retIsSuccess { get; set; }

    private readonly SemaphoreSlim _requestSemaphore = new(1, 1);

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };

    public DataStore(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    /// <summary>
    /// 단일 인스턴스를 반환하기 위한 함수
    /// </summary>
    public async Task<T?> GetItem<T>(eAPI path, object? paramItem = null) where T : class
    {
        var response = await PostAsync(GetAPIUrlByPath(path), paramItem);
        if (response == null) return null;

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DataResponse<T>>(_options).ConfigureAwait(false);
            if (result != null)
            {
                UpdateResponseStatus(result);
                // 상태 값 업데이트
                return result.Item;
            }
        }
        else
        {
            UpdateResponseStatusByFail();
        }

        return default;
    }

    /// <summary>
    /// 리스트(IQueryable)를 반환하기 위한 함수
    /// </summary>
    public async Task<IQueryable<T>> GetItems<T>(eAPI path, object? paramItem = null) where T : class
    {
        var response = await PostAsync(GetAPIUrlByPath(path), paramItem);

        if (response != null && response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<DataResponse<T>>(_options).ConfigureAwait(false);
            if (result != null && result.Items != null)
            {
                UpdateResponseStatus(result);
                return result.Items.AsQueryable();
            }
        }
        else
        {
            UpdateResponseStatusByFail();
        }

        return Enumerable.Empty<T>().AsQueryable();
    }

    /// <summary>
    /// 실제 HTTP POST 요청을 보내는 공통 함수
    /// </summary>
    public async Task<HttpResponseMessage?> PostAsync(string url, object? paramItem = null)
    {
        await _requestSemaphore.WaitAsync();

        HttpResponseMessage? response = null;

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(180));

        try
        {
            var token = _tokenProvider.GetToken();

            if (token == null)
            {
                return null; // 토큰이 없는 경우 null 반환
            }

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

            response = await _client.PostAsJsonAsync(url, paramItem ?? new { }, _options, cts.Token);
            
            if (!response.IsSuccessStatusCode)
            {

                try
                {
                    var responseContent = await response.Content.ReadFromJsonAsync<DataResponse>(_options);
                    
                    if (responseContent != null && responseContent.ResponseCode == eResponseCode.TOKEN_EXPIRED)
                    {
                        var refreshResult = await RefreshTokenAsync();

                        if (refreshResult is not null && refreshResult.IsSuccess)
                        {
                            var newToken = _tokenProvider.GetToken();
                            _client.DefaultRequestHeaders.Clear();
                            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken?.AccessToken);
                            
                            return await _client.PostAsJsonAsync(url, paramItem ?? new { }, _options, cts.Token);
                        };
                        
                        retMessage = "세션이 만료되었습니다. 다시 로그인 해주세요.";
                        retStatusCode = 401;
                        retIsSuccess = false;
                    }
                }
                catch (Exception)
                {
                    retMessage = "서버와 통신중 오류가 발생했습니다.";
                    retStatusCode = 500;
                    retIsSuccess = false;
                }
            }

            return response;
        }
        catch (OperationCanceledException)
        {
            retMessage = "서버 응답 시간이 초과하였습니다. 잠시 후 다시 시도해주세요.";
            retStatusCode = 408;
            retIsSuccess = false;
        }
        catch (Exception ex)
        {
            retMessage = "서버와 통신중 오류가 발생했습니다.";
            retStatusCode = 500;
            retIsSuccess = false;
        }
        finally
        {
            _requestSemaphore.Release();
        }

        if (!retIsSuccess)
        {
            ErrorOccured?.Invoke(this, new DataStoreErrorEventArgs(retMessage, retStatusCode.GetValueOrDefault(200)));
        }

        return response;
    }

    private async Task<DataResponse?> RefreshTokenAsync()
    {
        var result = new DataResponse { IsSuccess = false };

        try
        {
            var currentToken = _tokenProvider.GetToken();
            if (currentToken == null || string.IsNullOrWhiteSpace(currentToken.RefreshToken)) return result;

            string url = $"{APIUrl.TrimEnd('/')}/Auth/refresh_access_token";

            var request = new
            {
                MUR_Idx = currentToken.User?.MUR_Idx,
                TOKEN_VALUE = currentToken.RefreshToken
            };

            _client.DefaultRequestHeaders.Clear();

            var response = await _client.PostAsJsonAsync(url, request);
            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadFromJsonAsync<DataResponse>();
                if (responseContent is not null)
                {
                    result.Message = responseContent.Message;
                }

                return result;
            }

            var retToken = await response.Content.ReadFromJsonAsync<TokenResponse>();

            if (retToken == null || string.IsNullOrWhiteSpace(retToken.AccessToken))
            {
                return result;
            }

            _tokenProvider.SetToken(retToken);

            result.IsSuccess = true;

            return result;
        }
        catch
        {
            return null;
        }
    }

    private void UpdateResponseStatus<T>(DataResponse<T> result) where T : class
    {
        this.retMessage = result.Message;
        this.retIsSuccess = result.IsSuccess;
        this.retCount = result.TotalCount;
        this.retStatusCode = (int)result.ResponseCode;
    }

    private void UpdateResponseStatusByFail()
    {
        this.retMessage = "";
        this.retIsSuccess = false;
        this.retCount = 0;
        this.retStatusCode = 500;
    }

    private string GetAPIUrlByPath(eAPI path)
    {
        var parts = path.ToString().Split('_');
        if (parts.Length < 2) return APIUrl; // 기본 URL 반환

        string requestEntity = parts[0];
        string requestAction = parts[1];

        return $"{APIUrl.TrimEnd('/')}/{requestEntity}/{requestAction}";
    }

    public class DataStoreErrorEventArgs : EventArgs
    {
        public string? Message;
        public int StatusCode;

        public DataStoreErrorEventArgs(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}