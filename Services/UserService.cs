using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Test003.Models;
using Test003.Repositories;
using Test003.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Test003.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<(bool success, string token, int userId, string roleName)> Login(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAndPassword(username, password);
            if (user == null)
            {
                return (false, null, 0, null);
            }

            var userId = user.Id;
            var roleId = await _userRepository.GetRoleIdByUserId(userId);
            var roleName = await _userRepository.GetRoleNameByRoleId(roleId);

            var token = await GenerateJwtToken(user);
            return (true, token, userId, roleName);
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                // 这里可以添加更多基于角色的声明
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpirationMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
