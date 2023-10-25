using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Infrastructure.Services.Interfaces;

//TODO: replace with actual methods and implement them in the class
public interface IUserService
{
    IEnumerable<User> GetUsers();
    Task<User?> GetUserById(string id);
    Task<User> CreateUser(User createUser);
    Task<User?> UpdateUser(User updateUser);
    Task<User?> DeleteUser(string id);
}