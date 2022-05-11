using FakeMailRegistration.API.Models;
using Microsoft.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace FakeMailRegistration.API.Services
{
    public class GmailnatorService
    {
        private readonly HttpClient _httpClient;

        private readonly IConfiguration _configuration;

        public GmailnatorService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            _configuration = configuration;

            ConfigureGmailnator();
        }

        public async Task GenerateEmail(List<EmailVarieties> emailVarieties)
        {
            var options = emailVarieties.ConvertAll(value => (int)value);
            var optionsJson = new StringContent(JsonSerializer.Serialize(options), Encoding.UTF8, Application.Json);
            using var httpResponseMessage = await _httpClient.PostAsync("generate-email", optionsJson);
            httpResponseMessage.EnsureSuccessStatusCode();

            var contents = await httpResponseMessage.Content.ReadAsStringAsync();
        }

        private void ConfigureGmailnator()
        {
            _httpClient.BaseAddress = new Uri(_configuration["Gmailnator:BaseAddress"]);

            _httpClient.DefaultRequestHeaders.Add(
                _configuration["Gmailnator:RapidAPI:Host.Header.Name"], 
                _configuration["Gmailnator:RapidAPI:Host.Header.Value"]);

            _httpClient.DefaultRequestHeaders.Add(
                _configuration["Gmailnator:RapidAPI:Key.Header.Name"],
                _configuration["Gmailnator:RapidAPI:Key.Header.Value"]);
        }
    }
}
