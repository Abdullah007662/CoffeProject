using BusinessLayer.Services;
using CoffeProjectWebApi.Models;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoffeProjectWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserActivityService _userActivityService;
        private readonly ITokenService _tokenService;

        public AuthController(
            IAuthService authService,
            IUserActivityService userActivityService,
            ITokenService tokenService)
        {
            _authService = authService;
            _userActivityService = userActivityService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _authService.RegisterUserAsync(user, model.Password!, model.Role);
            if (!result)
                return BadRequest(new { message = "User registration failed" });

            await _userActivityService.LogActivityAsync(
                user.Id,
                "Register",
                "User",
                null,
                $"User {user.UserName} registered",
                null!,
                null!,
                HttpContext.Connection.RemoteIpAddress?.ToString()!);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (token, user) = await _authService.LoginAsync(model.UserName!, model.Password!);
            if (token == null || user == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            await _userActivityService.LogActivityAsync(
                user.Id,
                "Login",
                "User",
                null,
                $"User {user.UserName} logged in",
                null,
                null,
                HttpContext.Connection.RemoteIpAddress?.ToString());

            return Ok(new
            {
                token,
                user = new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    roles = await _authService.GetUserRolesAsync(user.Id)
                }
            });
        }
    }
}