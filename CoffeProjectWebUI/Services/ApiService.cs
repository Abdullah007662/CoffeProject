using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CoffeProjectWebUI.Models;

namespace CoffeProjectWebUI.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<string> LoginAsync(LoginViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/login", content);
            if (!response.IsSuccessStatusCode)
                return null!;

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TokenResponse>(responseContent, _jsonOptions);
            return result!.Token;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel model)
        {
            var client = _httpClientFactory.CreateClient("API");
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/register", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<UserViewModel>> GetUsersAsync(string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/auth/users");
            if (!response.IsSuccessStatusCode)
                return new List<UserViewModel>();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserViewModel>>(responseContent, _jsonOptions)!;
        }

        public async Task<bool> AssignRoleAsync(string userId, string roleName, string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var model = new { UserId = userId, RoleName = roleName };
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/assign-role", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId, string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/auth/user-roles/{userId}");
            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(responseContent, _jsonOptions)!;
        }

        public async Task<List<UserActivityViewModel>> GetUserActivitiesAsync(string userId, string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/useractivity/user/{userId}");
            if (!response.IsSuccessStatusCode)
                return new List<UserActivityViewModel>();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserActivityViewModel>>(responseContent, _jsonOptions)!;
        }

        public async Task<List<UserActivityViewModel>> GetAllActivitiesAsync(string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/useractivity");
            if (!response.IsSuccessStatusCode)
                return new List<UserActivityViewModel>();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserActivityViewModel>>(responseContent, _jsonOptions)!;
        }

        public async Task<List<string>> GetUserTokensAsync(string userId, string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"api/auth/tokens/{userId}");
            if (!response.IsSuccessStatusCode)
                return new List<string>();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(responseContent, _jsonOptions)!;
        }

        public async Task<bool> InvalidateTokenAsync(int tokenId, string token)
        {
            var client = _httpClientFactory.CreateClient("API");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"api/auth/invalidate-token/{tokenId}", null);
            return response.IsSuccessStatusCode;
        }
    }

    public class TokenResponse
    {
        public string? Token { get; set; }
    }
}
