using DataAccessLayer.Concrete;
using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> RegisterUserAsync(AppUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return false;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                var appRole = new AppRole
                {
                    Name = role,
                    Description = $"{role} role",
                    NormalizedName = role.ToUpper()
                };
                await _roleManager.CreateAsync(appRole);
            }

            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<(string token, AppUser user)> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                return (null, null);

            var token = await GenerateJwtTokenAsync(user);
            await StoreTokenAsync(user.Id, token);

            return (token, user);
        }

        public async Task<List<AppUser>> GetUsersAsync()
        {
            return _userManager.Users.ToList();
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId)!;
        }

        public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var role = new AppRole
                {
                    Name = roleName,
                    Description = $"{roleName} role",
                    NormalizedName = roleName.ToUpper()
                };
                await _roleManager.CreateAsync(role);
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<List<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> StoreTokenAsync(string userId, string token)
        {
            var tokenInfo = new TokenInfo
            {
                UserId = userId,
                Token = token,
                ExpiryDate = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"])),
                IsActive = true
            };

            await _unitOfWork.TokenInfos.AddAsync(tokenInfo);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetUserTokensAsync(string userId)
        {
            var tokens = await _unitOfWork.TokenInfos.FindAsync(t => t.UserId == userId && t.IsActive);
            return tokens.Select(t => t.Token).ToList()!;
        }

        public async Task<bool> InvalidateTokenAsync(int tokenId)
        {
            var token = await _unitOfWork.TokenInfos.GetByIdAsync(tokenId);
            if (token == null)
                return false;

            token.IsActive = false;
            await _unitOfWork.TokenInfos.UpdateAsync(token);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
