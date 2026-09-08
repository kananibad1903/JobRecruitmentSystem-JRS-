using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace JobRecruitmentSystem.UI.Services
{
    public class ApiClient
    {
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _accessor;

        public ApiClient(HttpClient http, IHttpContextAccessor accessor)
        {
            _http = http;
            _accessor = accessor;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            AttachToken();
            var response = await _http.GetAsync(url);
            return await ProcessAsync<T>(response);
        }

        public async Task<T?> PostAsync<T>(string url, object? body = null)
        {
            AttachToken();
            var response = await _http.PostAsync(url, body is null ? null : JsonContent.Create(body));
            return await ProcessAsync<T>(response);
        }

        public async Task PostAsync(string url, object? body = null)
        {
            AttachToken();
            var response = await _http.PostAsync(url, body is null ? null : JsonContent.Create(body));
            await EnsureSuccessAsync(response);
        }

        public async Task<T?> PutAsync<T>(string url, object? body)
        {
            AttachToken();
            var response = await _http.PutAsync(url, JsonContent.Create(body));
            return await ProcessAsync<T>(response);
        }

        public async Task PutAsync(string url, object? body)
        {
            AttachToken();
            var response = await _http.PutAsync(url, JsonContent.Create(body));
            await EnsureSuccessAsync(response);
        }

        public async Task DeleteAsync(string url)
        {
            AttachToken();
            var response = await _http.DeleteAsync(url);
            await EnsureSuccessAsync(response);
        }

        public async Task<T?> PostMultipartAsync<T>(string url, MultipartFormDataContent content)
        {
            AttachToken();
            var response = await _http.PostAsync(url, content);
            return await ProcessAsync<T>(response);
        }

        private void AttachToken()
        {
            var token = _accessor.HttpContext?.User?.FindFirst("AccessToken")?.Value;
            _http.DefaultRequestHeaders.Authorization = token is null
                ? null
                : new AuthenticationHeaderValue("Bearer", token);
        }

        private static async Task<T?> ProcessAsync<T>(HttpResponseMessage response)
        {
            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(ExtractMessage(raw, response));
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(raw, JsonOptions);
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                throw new ApiException(ExtractMessage(raw, response));
            }
        }

        private static string ExtractMessage(string raw, HttpResponseMessage response)
        {
            if (!string.IsNullOrWhiteSpace(raw))
            {
                try
                {
                    using var doc = JsonDocument.Parse(raw);
                    if (doc.RootElement.TryGetProperty("message", out var messageProp))
                    {
                        return messageProp.GetString() ?? raw;
                    }
                }
                catch (JsonException)
                {
                    // fall through to generic message
                }
            }

            return $"Sorğu uğursuz oldu ({(int)response.StatusCode}).";
        }
    }
}
