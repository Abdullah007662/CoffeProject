using CoffeProjectWebUI.Models;
using CoffeProjectWebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeProjectWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IApiService _apiService;

        public AdminController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private string GetToken()
        {
            return User.FindFirst("Token")?.Value;
        }

        public async Task<IActionResult> Users()
        {
            var token = GetToken();
            var users = await _apiService.GetUsersAsync(token);
            return View(users);
        }

        public async Task<IActionResult> AssignRole(string userId)
        {
            var token = GetToken();
            var users = await _apiService.GetUsersAsync(token);
            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
                return NotFound();

            var roles = await _apiService.GetUserRolesAsync(userId, token);

            var model = new RoleAssignmentViewModel
            {
                UserId = userId,
                UserName = user.UserName,
                CurrentRoles = roles
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = GetToken();
            var result = await _apiService.AssignRoleAsync(model.UserId, model.NewRole, token);

            if (!result)
            {
                ModelState.AddModelError("", "Rol atama işlemi başarısız oldu");
                return View(model);
            }

            return RedirectToAction("Users");
        }

        public async Task<IActionResult> UserActivities()
        {
            var token = GetToken();
            var activities = await _apiService.GetAllActivitiesAsync(token);
            return View(activities);
        }

        public async Task<IActionResult> UserTokens(string userId)
        {
            var token = GetToken();
            var tokens = await _apiService.GetUserTokensAsync(userId, token);

            // View modelini hazırla
            // Not: API'dan dönen token listesini TokenViewModel'e çevirmeniz gerekecek

            return View(tokens);
        }

        [HttpPost]
        public async Task<IActionResult> InvalidateToken(int tokenId)
        {
            var token = GetToken();
            var result = await _apiService.InvalidateTokenAsync(tokenId, token);

            if (!result)
                return Json(new { success = false });

            return Json(new { success = true });
        }
    }
}
