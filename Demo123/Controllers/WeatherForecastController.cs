using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo123.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private IHttpClientFactory _httpClientFactory;
        private readonly UserManager<User> _userManager;


        public WeatherForecastController(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
        }



        [HttpGet("Anonymous")]
        [AllowAnonymous]
        public  string GetString()
        {
            return "Hello World!";
        }

        [HttpGet("/Google", Name = "Get Google Html")]
        [Authorize]
        public async Task<string> Get()
        {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("https://www.google.com/");
                var responseHtml = await response.Content.ReadAsStringAsync();
                return responseHtml;
        }

        [HttpGet("GetUserData", Name = "GetWeatherForecast2")]
        [Authorize]
        public async Task<IActionResult> Get2()
        {
            // Get user ID
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get user from database using ID
            var user = await _userManager.FindByIdAsync(userId);

            // Check if user is found
            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Return user information
            return Ok(new { UserId = user.Id, FirstName = user.FirstName, LastName = user.LastName });
        }
    }
}
