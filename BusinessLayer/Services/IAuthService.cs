// BusinessLayer/Services/IAuthService.cs
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUserAsync(AppUser user, string password, string role);
        Task<(string token, AppUser user)> LoginAsync(string username, string password); // Changed from AuthenticateAsync
        Task<List<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(string userId);
        Task<bool> AssignRoleToUserAsync(string userId, string roleName);
        Task<List<string>> GetUserRolesAsync(string userId);
        Task<bool> RemoveUserFromRoleAsync(string userId, string roleName);
        Task<string> GenerateJwtTokenAsync(AppUser user);
        Task<bool> StoreTokenAsync(string userId, string token);
        Task<List<string>> GetUserTokensAsync(string userId);
        Task<bool> InvalidateTokenAsync(int tokenId);
    }
}