using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using JobRecruitmentSystem.BLL.Services.Interfaces;

namespace JobRecruitmentSystem.BLL.Services.Implementations
{
    public class AiChatService : IAiChatService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AiChatService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<string> SendMessageAsync(string message)
        {
            var apiKey = _configuration["Gemini:ApiKey"];
            var model = _configuration["Gemini:Model"];

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var systemPrompt = "Sən Job Recruitment System platformasında işləyən köməkçi bir AI-san. İstifadəçilərə iş axtarışı, CV yazma, müsahibəyə hazırlıq və platformadan istifadə mövzularında Azərbaycan dilində kömək et. Qısa və faydalı cavablar ver.";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = systemPrompt + "\n\nİstifadəçi sualı: " + message }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("AI cavab verə bilmədi: " + responseBody);
            }

            using var doc = JsonDocument.Parse(responseBody);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text;
        }
    }
}