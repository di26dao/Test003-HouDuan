using Microsoft.AspNetCore.Mvc;
using Test003.Models;
using Test003.Services;
using System.Threading.Tasks;

namespace Test003.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var (success, token, userId, roleName) = await _userService.Login(user.Username, user.Password);
            if (!success)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token, UserId = userId, RoleName = roleName });
        }

    }

    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Welcome to the application!");
        }
    }
}