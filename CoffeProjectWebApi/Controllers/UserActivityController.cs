using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeProjectWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _userActivityService;

        public UserActivityController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActivities()
        {
            var activities = await _userActivityService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserActivities(string userId)
        {
            var activities = await _userActivityService.GetUserActivitiesAsync(userId);
            return Ok(activities);
        }

        [HttpGet("type/{activityType}")]
        public async Task<IActionResult> GetActivitiesByType(string activityType)
        {
            var activities = await _userActivityService.GetActivitiesByTypeAsync(activityType);
            return Ok(activities);
        }

        [HttpGet("entity/{entityName}")]
        public async Task<IActionResult> GetActivitiesByEntity(string entityName)
        {
            var activities = await _userActivityService.GetActivitiesByEntityAsync(entityName);
            return Ok(activities);
        }
    }
}
