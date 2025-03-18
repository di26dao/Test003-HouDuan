using Microsoft.AspNetCore.Mvc;
using Test003.Models;
using Test003.Services;
using System.Threading.Tasks;
using SqlSugar;

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
        public async Task<IActionResult> login([FromBody] User user)
        {
            var (success, token, userId, roleName) = await _userService.Login(user.Username, user.Password);
            if (!success)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token, UserId = userId, RoleName = roleName });
        }
        [HttpPost("select")]
        public async Task<IActionResult> select()
        {

            var count = await _userService.selectUser();

            return Ok(count);
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