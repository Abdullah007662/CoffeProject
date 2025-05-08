using CoffeProjectWebUI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeProjectWebUI.Controllers
{
    [Authorize]
    public class UserActivityLogController : Controller
    {
        private readonly IApiService _apiService;

        public UserActivityLogController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private string GetToken()
        {
            return User.FindFirst("Token")?.Value!;
        }

        private string GetUserId()
        {
            return User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!;
        }

        public async Task<IActionResult> MyActivities()
        {
            var token = GetToken();
            var userId = GetUserId();

            var activities = await _apiService.GetUserActivitiesAsync(userId, token);
            return View(activities);
        }
    }
}
