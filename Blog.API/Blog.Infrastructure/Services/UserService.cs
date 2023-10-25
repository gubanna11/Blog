using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Infrastructure.Services.Interfaces;

namespace Blog.Infrastructure.Services;

//TODO:Implement methods
public sealed class UserService : IUserService
{
    public IEnumerable<User> GetUsers()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetUserById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<User> CreateUser(User createUser)
    {
        throw new NotImplementedException();
    }

    public Task<User?> UpdateUser(User updateUser)
    {
        throw new NotImplementedException();
    }

    public Task<User?> DeleteUser(string id)
    {
        throw new NotImplementedException();
    }
}