using Microsoft.EntityFrameworkCore;
using Test003.Data;
using Test003.Models;
using Test003.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Test003.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            string sql = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username, Password = password });
            }
        }

        public async Task<int> GetRoleIdByUserId(int userId)
        {
            string sql = "SELECT RoleId FROM UserRoles WHERE UserId = @UserId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<int>(sql, new { UserId = userId });
            }
        }

        public async Task<string> GetRoleNameByRoleId(int roleId)
        {
            string sql = "SELECT Name FROM Roles WHERE Id = @RoleId";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { RoleId = roleId });
            }
        }

        async Task<int> IUserRepository.selectUser()
        {
            string sql = "SELECT COUNT(*) FROM Users";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<int>(sql);
            }
        }

       async Task<int> IUserRepository.selectUserByUserName(string username)
        {
            string sql = "SELECT COUNT(*) FROM Users WHERE UserName = @UserName";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<int>(sql);
            }
        }

       async Task<bool> IUserRepository.InsertUser(string username,string password,string phone)
        {
            string sql = "INSERT INTO Users (Username,Password,Phone) VALUES (@UserName,@Password,@Phone);";
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<bool>(sql);
            }
        }
    }
}