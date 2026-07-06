using System.Net.Http.Json;
using System.Text.Json;
using SmartEMR.Domain.Enums;
using SmartEMR.Domain.DTOs;
using System.Net.Http.Headers;

namespace SmartEMR.Infrastructure
{
    public class DataStore
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ITokenProvider _tokenProvider;

        public string APIUrl { get; set; } = "http://127.0.0.1:8000/";

        // API 응답 상태를 저장하는 속성들
        public string? retMessage { get; set; }
        public int? retStatusCode { get; set; } // eResponseCode에 맞게 int로 변경 권장
        public int? retCount { get; set; }
        public bool retIsSuccess { get; set; }

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
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

            return Enumerable.Empty<T>().AsQueryable();
        }

        /// <summary>
        /// 실제 HTTP POST 요청을 보내는 공통 함수
        /// </summary>
        public async Task<HttpResponseMessage?> PostAsync(string url, object? paramItem = null)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            try
            {
                var token = _tokenProvider.GetToken();

                if (token == null)
                {
                    return null; // 토큰이 없는 경우 null 반환
                }

                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _client.PostAsJsonAsync(url, paramItem ?? new { }, _options, cts.Token);
                
                if (!response.IsSuccessStatusCode)
                {
                    try
                    {
                        var responseContent = await response.Content.ReadFromJsonAsync<DataResponse>(_options);
                        
                        if (responseContent != null && responseContent.ResponseCode == eResponseCode.TOKEN_EXPIRED)
                        {
                            bool isRefresh = await RefreshTokenAsync();

                            if (isRefresh)
                            {
                                var newToken = _tokenProvider.GetToken();
                                _client.DefaultRequestHeaders.Clear();
                                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken?.AccessToken);
                                
                                return await _client.PostAsJsonAsync(url, paramItem ?? new { }, _options, cts.Token);
                            }
                        }
                        else
                        {
                            retIsSuccess = false;
                            retMessage = "세션이 만료되었습니다. 다시 로그인 해주세요";
                            return response;
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                retIsSuccess = false;
                retMessage = "서버 응답 시간이 초과하였습니다.";
                return null;
            }
            catch (Exception ex)
            {
                retIsSuccess = false;
                retMessage = $"통신 에러: {ex.Message}";
                return null;
            }
        }

        private async Task<bool> RefreshTokenAsync()
        {
            try
            {
                var currentToken = _tokenProvider.GetToken();
                if (currentToken == null || string.IsNullOrWhiteSpace(currentToken.RefreshToken)) return false;

                string url = $"{APIUrl.TrimEnd('/')}/Auth/refresh_access_token";

                var request = new
                {
                    MUR_Idx = currentToken.User?.MUR_Idx,
                    TOKEN_VALUE = currentToken.RefreshToken
                };

                _client.DefaultRequestHeaders.Clear();

                var response = await _client.PostAsJsonAsync(url, request);

                if (response.IsSuccessStatusCode)
                {
                    var retToken = await response.Content.ReadFromJsonAsync<TokenResponse>();

                    if (retToken == null || string.IsNullOrWhiteSpace(retToken.AccessToken))
                    {
                        return false;
                    }
                    
                    _tokenProvider.SetToken(retToken);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateResponseStatus<T>(DataResponse<T> result) where T : class
        {
            this.retMessage = result.Message;
            this.retIsSuccess = result.IsSuccess;
            this.retCount = result.TotalCount;
            this.retStatusCode = (int)result.ResponseCode;
        }

        private string GetAPIUrlByPath(eAPI path)
        {
            var parts = path.ToString().Split('_');
            if (parts.Length < 2) return APIUrl; // 기본 URL 반환

            string requestEntity = parts[0];
            string requestAction = parts[1];

            return $"{APIUrl.TrimEnd('/')}/{requestEntity}/{requestAction}";
        }
    }
}