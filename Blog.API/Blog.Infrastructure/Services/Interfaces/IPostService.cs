using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Infrastructure.Services.Interfaces;

public interface IPostService
{
    IEnumerable<Post> GetPosts();
    Task<Post?> GetPostById(Guid id);
    Task<Post> CreatePost(Post createPost);
    Task<Post?> UpdatePost(Post updatePost);
    Task<Post?> DeletePost(Guid id);
}