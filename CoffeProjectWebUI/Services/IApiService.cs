using CoffeProjectWebUI.Models;

namespace CoffeProjectWebUI.Services
{
    public interface IApiService
    {
        Task<string> LoginAsync(LoginViewModel model);
        Task<bool> RegisterAsync(RegisterViewModel model);
        Task<List<UserViewModel>> GetUsersAsync(string token);
        Task<bool> AssignRoleAsync(string userId, string roleName, string token);
        Task<List<string>> GetUserRolesAsync(string userId, string token);
        Task<List<UserActivityViewModel>> GetUserActivitiesAsync(string userId, string token);
        Task<List<UserActivityViewModel>> GetAllActivitiesAsync(string token);
        Task<List<string>> GetUserTokensAsync(string userId, string token);
        Task<bool> InvalidateTokenAsync(int tokenId, string token);
    }
}
