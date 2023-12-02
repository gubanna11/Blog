using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Contracts.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetPosts();
    Task<PostResponse?> GetPostById(Guid id);
    Task<PostResponse?> CreatePost(CreatePostRequest createPost);
    Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost);
    Task<PostResponse?> DeletePost(Guid id);
}