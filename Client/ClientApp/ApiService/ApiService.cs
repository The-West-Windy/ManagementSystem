using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ClientApp.Models;
using Microsoft.Maui.Storage;

namespace ClientApp.Services
{
    public class ApiService
    {
        private const string TokenPreferenceKey = "jwt_token";
        private const string LibrarianIdPreferenceKey = "librarian_id";
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
            if (string.IsNullOrWhiteSpace(authResponse?.Token) || authResponse.LibrarianId <= 0)
            {
                return (false, "The API did not return complete authentication data.");
            }

            Preferences.Set(TokenPreferenceKey, authResponse.Token);
            Preferences.Set(LibrarianIdPreferenceKey, authResponse.LibrarianId);
            SetAuthorizationHeader(authResponse.Token);
            return (true, null);
        }

        public async Task<IReadOnlyList<Book>> GetBooksAsync()
        {
            EnsureAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/items");
            await EnsureSuccessStatusCodeAsync(response);

            var books = await response.Content.ReadFromJsonAsync<List<Book>>();
            return books ?? new List<Book>();
        }

        public async Task CreateBorrowRequestAsync(BorrowRequest request)
        {
            EnsureAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/actions", request);
            await EnsureSuccessStatusCodeAsync(response);
        }

        public int? GetCurrentLibrarianId()
        {
            var id = Preferences.Get(LibrarianIdPreferenceKey, -1);
            return id > 0 ? id : null;
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
                Preferences.Remove(LibrarianIdPreferenceKey);
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