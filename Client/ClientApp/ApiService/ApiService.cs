using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClientApp.Models;
using Microsoft.Maui.Storage;

namespace ClientApp.Services
{
    public class ApiService
    {
        private const string TokenPreferenceKey = "jwt_token";
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            RestoreToken();
        }

        public bool HasToken => !string.IsNullOrWhiteSpace(Preferences.Get(TokenPreferenceKey, string.Empty));

        public async Task<(bool Success, string? ErrorMessage)> LoginAsync(string email, string password)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new { email, password });
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, GetErrorMessage(response.StatusCode, error));
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();
            if (string.IsNullOrWhiteSpace(authResponse?.Token))
            {
                return (false, "The API did not return a JWT token.");
            }

            Preferences.Set(TokenPreferenceKey, authResponse.Token);
            SetAuthorizationHeader(authResponse.Token);
            return (true, null);
        }

        public async Task<IReadOnlyList<TrainingClass>> GetClassesAsync()
        {
            EnsureAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/classes");
            await EnsureSuccessStatusCodeAsync(response);

            var classes = await response.Content.ReadFromJsonAsync<List<TrainingClass>>();
            return classes ?? new List<TrainingClass>();
        }

        public async Task CreateBookingAsync(Booking request)
        {
            EnsureAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/bookings", request);
            await EnsureSuccessStatusCodeAsync(response);
        }

        public int? GetCoachIdFromToken()
        {
            var token = Preferences.Get(TokenPreferenceKey, string.Empty);
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }

            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                return null;
            }

            var payload = parts[1].PadRight(parts[1].Length + (4 - parts[1].Length % 4) % 4, '=');
            var payloadBytes = Convert.FromBase64String(payload);
            var payloadJson = Encoding.UTF8.GetString(payloadBytes);

            using var document = JsonDocument.Parse(payloadJson);
            if (document.RootElement.TryGetProperty("sub", out var subElement) &&
                int.TryParse(subElement.GetString(), out var coachId))
            {
                return coachId;
            }

            return null;
        }

        private void RestoreToken()
        {
            var token = Preferences.Get(TokenPreferenceKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(token))
            {
                SetAuthorizationHeader(token);
            }
        }

        private void EnsureAuthorizationHeader()
        {
            var token = Preferences.Get(TokenPreferenceKey, string.Empty);
            if (string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return;
            }

            if (_httpClient.DefaultRequestHeaders.Authorization?.Parameter != token)
            {
                SetAuthorizationHeader(token);
            }
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            var error = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                Preferences.Remove(TokenPreferenceKey);
                _httpClient.DefaultRequestHeaders.Authorization = null;
                throw new UnauthorizedAccessException("Authorization failed. Please log in again.");
            }

            throw new HttpRequestException(GetErrorMessage(response.StatusCode, error));
        }

        private static string GetErrorMessage(HttpStatusCode statusCode, string? responseText)
        {
            if (!string.IsNullOrWhiteSpace(responseText))
            {
                return responseText.Trim();
            }

            return statusCode switch
            {
                HttpStatusCode.BadRequest => "The server rejected the request.",
                HttpStatusCode.NotFound => "The requested resource was not found.",
                HttpStatusCode.InternalServerError => "The server returned an error.",
                _ => $"Unexpected response from server: {(int)statusCode}."
            };
        }
    }
}