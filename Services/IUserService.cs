using Test003.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Test003.Services
{
    public interface IUserService 
    {
        
        Task<(bool success, string token, int userId, string roleName)> Login(string username, string password);
    }
}
