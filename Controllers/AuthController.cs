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
            ApiResponse result;

            if (user.Username != null && user.Password != null && user.Phone != null)
            {
                var count = await _userService.selectUserByUserName(user.Username);
                if (count == 0)
                {
                    var insertUserSuccess = await _userService.InsertUser(user.Username, user.Password, user.Phone);
                    if (insertUserSuccess)
                    {
                        var userId = await _userService.selectUserId(user.Username);
                        var insertUserRoleSuccess = await _userService.InsertUserRole(userId);
                        if (insertUserRoleSuccess)
                        {
                            result = new ApiResponse(true, 200, "注册成功");
                        }
                        else
                        {
                            await _userService.DeleteUser(userId);
                            result = new ApiResponse(false, 500, "插入用户角色失败");
                        }
                    }
                    else
                    {
                        result = new ApiResponse(false, 500, "插入用户信息失败");
                    }
                }
                else
                {
                    result = new ApiResponse(false, 409, "用户名已存在");
                }
            }
            else
            {
                result = new ApiResponse(false, 400, "请求参数不完整");
            }

            return Ok(result);
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