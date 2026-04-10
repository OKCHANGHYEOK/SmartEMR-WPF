using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json;
using SmartEMR.Domain.Models; // DataResponse가 있는 곳

namespace SmartEMR.Infrastructure
{
    public class DataStore
    {
        private readonly HttpClient _client = new HttpClient();

        public string APIUrl { get; set; } = "http://127.0.0.1:8000/";

        // API 응답 상태를 저장하는 속성들
        public string? retMessage { get; set; }
        public int? retStatusCode { get; set; } // eResponseCode에 맞게 int로 변경 권장
        public int? retCount { get; set; }
        public bool? retIsSuccess { get; set; }

        private readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// 단일 인스턴스를 반환하기 위한 함수
        /// </summary>
        public async Task<T?> GetItem<T>(string _eAPI, object? paramItem = null) where T : class
        {
            var response = await PostAsync(_eAPI, paramItem);

            if (response != null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DataResponse<T>>(_options);
                if (result != null)
                {
                    // 상태 값 업데이트
                    UpdateResponseStatus(result);
                    return result.Item;
                }
            }
            return default;
        }

        /// <summary>
        /// 리스트(IQueryable)를 반환하기 위한 함수
        /// </summary>
        public async Task<IQueryable<T>> GetItems<T>(string _eAPI, object? paramItem = null) where T : class
        {
            var response = await PostAsync(_eAPI, paramItem);

            if (response != null && response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<DataResponse<T>>(_options);
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
        public async Task<HttpResponseMessage?> PostAsync(string _eAPI, object? paramItem = null)
        {
            var parts = _eAPI.Split('_');
            if (parts.Length < 2) return null;

            string requestEntity = parts[0];
            string requestAction = parts[1];
            string requestUrl = $"{APIUrl.TrimEnd('/')}/{requestEntity}/{requestAction}";

            try
            {
                // 토큰은 위에서 SetToken을 통해 DefaultRequestHeaders에 박혀있으므로 
                // 여기서 별도로 헤더를 건드릴 필요 없이 바로 전송합니다.
                return await _client.PostAsJsonAsync(requestUrl, paramItem ?? new { }, _options);
            }
            catch (Exception ex)
            {
                retIsSuccess = false;
                retMessage = $"통신 에러: {ex.Message}";
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
    }
}