using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Infrastructure.Services.Interfaces;

namespace Blog.Infrastructure.Services;

//TODO:Implement methods
public sealed class PostService : IPostService
{
    public IEnumerable<Post> GetPosts()
    {
        throw new NotImplementedException();
    }

    public Task<Post?> GetPostById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Post> CreatePost(Post createPost)
    {
        throw new NotImplementedException();
    }

    public Task<Post?> UpdatePost(Post updatePost)
    {
        throw new NotImplementedException();
    }

    public Task<Post?> DeletePost(Guid id)
    {
        throw new NotImplementedException();
    }
}