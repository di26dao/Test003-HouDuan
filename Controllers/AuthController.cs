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
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            var type = false;
            var type2 = false;
            if(user.Username!=null && user.Password!=null && user.Phone != null)
            {
              var count= await _userService.selectUserByUserName(user.Username);
                if (count == 0)
                {
                   type= await _userService.InsertUser(user.Username, user.Password, user.Phone);
                    if (type)
                    {
                     var UserId =  await _userService.selectUserId(user.Username);
                     type2=  await _userService.InsertUserRole(UserId);
                        if (type2)
                        {
                            return Ok(new { type2 });
                        }
                        
                    }
                    else
                    {
                        return Ok(new { type2 });
                    }
                }
                else
                {
                    return Ok(new { type2 });
                }
            }
            else
            {
                return Ok(new { type2 });
            }

            return Ok(new { type2 });
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