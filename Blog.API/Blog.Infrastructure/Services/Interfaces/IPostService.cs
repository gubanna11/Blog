using Blog.Core.Contracts.Controllers.Posts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetPosts(CancellationToken cancellationToken);
    Task<PostResponse?> GetPostById(Guid id, CancellationToken cancellationToken);
    Task<PostResponse?> CreatePost(CreatePostRequest createPost, CancellationToken cancellationToken);
    Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost, CancellationToken cancellationToken);
    Task<PostResponse?> DeletePost(Guid id, CancellationToken cancellationToken);
}