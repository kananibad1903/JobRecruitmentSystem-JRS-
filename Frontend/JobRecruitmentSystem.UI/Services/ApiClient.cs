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
        private readonly ILogger<ApiClient> _logger;
        private readonly bool _useTrialSiteAuth;

        public ApiClient(HttpClient http, IHttpContextAccessor accessor, ILogger<ApiClient> logger, IConfiguration configuration)
        {
            _http = http;
            _accessor = accessor;
            _logger = logger;
            // TEMPORARY — see Program.cs (both projects) for the full explanation.
            // While SmarterASP.NET's trial Basic Auth gate is on, the standard
            // Authorization header is reserved for it (set once, at HttpClient
            // creation) and our own JWT travels via "X-Access-Token" instead.
            _useTrialSiteAuth = configuration.GetValue<bool>("TrialSiteAuth:Enabled");
            _logger.LogWarning(
                "ApiClient: TrialSiteAuth resolved to Enabled={Enabled} (raw config value='{Raw}'), HttpClient default Authorization header is currently '{AuthHeader}'.",
                _useTrialSiteAuth,
                configuration["TrialSiteAuth:Enabled"] ?? "<null>",
                _http.DefaultRequestHeaders.Authorization?.Scheme ?? "<none>");
        }

        public async Task<T?> GetAsync<T>(string url, string? overrideToken = null)
        {
            AttachToken(overrideToken);
            LogRequest(HttpMethod.Get, url);
            var response = await _http.GetAsync(url);
            return await ProcessAsync<T>(response);
        }

        public async Task<T?> PostAsync<T>(string url, object? body = null)
        {
            AttachToken();
            LogRequest(HttpMethod.Post, url);
            var response = await _http.PostAsync(url, body is null ? null : JsonContent.Create(body));
            return await ProcessAsync<T>(response);
        }

        public async Task PostAsync(string url, object? body = null)
        {
            AttachToken();
            LogRequest(HttpMethod.Post, url);
            var response = await _http.PostAsync(url, body is null ? null : JsonContent.Create(body));
            await EnsureSuccessAsync(response);
        }

        public async Task<T?> PutAsync<T>(string url, object? body)
        {
            AttachToken();
            LogRequest(HttpMethod.Put, url);
            var response = await _http.PutAsync(url, JsonContent.Create(body));
            return await ProcessAsync<T>(response);
        }

        public async Task PutAsync(string url, object? body)
        {
            AttachToken();
            LogRequest(HttpMethod.Put, url);
            var response = await _http.PutAsync(url, JsonContent.Create(body));
            await EnsureSuccessAsync(response);
        }

        public async Task DeleteAsync(string url)
        {
            AttachToken();
            LogRequest(HttpMethod.Delete, url);
            var response = await _http.DeleteAsync(url);
            await EnsureSuccessAsync(response);
        }

        public async Task<T?> PostMultipartAsync<T>(string url, MultipartFormDataContent content)
        {
            AttachToken();
            LogRequest(HttpMethod.Post, url);
            var response = await _http.PostAsync(url, content);
            return await ProcessAsync<T>(response);
        }

        private void LogRequest(HttpMethod method, string relativeUrl)
        {
            var fullUrl = _http.BaseAddress is null
                ? relativeUrl
                : new Uri(_http.BaseAddress, relativeUrl).ToString();
            _logger.LogDebug("ApiClient: {Method} {Url} (BaseAddress={BaseAddress})", method, fullUrl, _http.BaseAddress);
        }

        private void AttachToken(string? overrideToken = null)
        {
            // overrideToken lets a caller (e.g. right after sign-in, before the
            // current request's HttpContext.User reflects the new cookie) supply
            // the JWT directly instead of relying on the authenticated principal.
            var token = overrideToken;

            if (string.IsNullOrEmpty(token))
            {
                var user = _accessor.HttpContext?.User;
                token = user is { Identity.IsAuthenticated: true }
                    ? user.FindFirst("AccessToken")?.Value
                    : null;
            }

            if (_useTrialSiteAuth)
            {
                // Authorization is reserved for the trial site's Basic Auth (set
                // once on the HttpClient in Program.cs) — never touch it here.
                // Our own JWT rides along in a custom header instead.
                _http.DefaultRequestHeaders.Remove("X-Access-Token");
                if (!string.IsNullOrEmpty(token))
                {
                    _http.DefaultRequestHeaders.Add("X-Access-Token", token);
                }

                _logger.LogDebug(
                    string.IsNullOrEmpty(token)
                        ? "ApiClient: no access token available; X-Access-Token omitted (trial-site Basic Auth still applied)."
                        : "ApiClient: attaching JWT via X-Access-Token ({Length} chars).",
                    token?.Length ?? 0);
                return;
            }

            if (string.IsNullOrEmpty(token))
            {
                // Explicitly clear it two ways: assigning null is normally enough,
                // but on a pooled/reused HttpClient we don't want to take any
                // chances leaving a stale header behind for anonymous requests.
                _http.DefaultRequestHeaders.Authorization = null;
                _http.DefaultRequestHeaders.Remove("Authorization");
                _logger.LogDebug("ApiClient: no access token available; sending request without an Authorization header.");
            }
            else
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                _logger.LogDebug(
                    "ApiClient: attaching Bearer token ({Length} chars, starts with '{Prefix}').",
                    token.Length,
                    token.Length > 8 ? token[..8] : token);
            }
        }

        private async Task<T?> ProcessAsync<T>(HttpResponseMessage response)
        {
            var raw = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                LogFailure(response, raw);
                throw new ApiException(ExtractMessage(raw, response));
            }

            if (string.IsNullOrWhiteSpace(raw))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(raw, JsonOptions);
        }

        private async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                LogFailure(response, raw);
                throw new ApiException(ExtractMessage(raw, response));
            }
        }

        private void LogFailure(HttpResponseMessage response, string raw)
        {
            var headers = string.Join(" | ", response.Headers
                .Concat(response.Content.Headers)
                .Select(h => $"{h.Key}={string.Join(",", h.Value)}"));

            _logger.LogWarning(
                "ApiClient: {StatusCode} from {Url}. Headers: [{Headers}]. Body: {Body}",
                (int)response.StatusCode,
                response.RequestMessage?.RequestUri,
                headers,
                raw.Length > 500 ? raw[..500] : raw);
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
