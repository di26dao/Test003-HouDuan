using Test003.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Test003.Repositories
{
    public interface IUserRepository 
    {
        Task<User> GetUserByUsernameAndPassword(string username, string password);
        Task<int> GetRoleIdByUserId(int userId);
        Task<string> GetRoleNameByRoleId(int roleId);
        Task<int> selectUser();
        Task<int> selectUserByUserName(string username);
        Task<bool> InsertUser(string username,string password,string phone);
        Task<int> selectUserId(string username);
        Task<bool> InsertUserRole(int id);
    }
}
