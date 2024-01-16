using Blog.Core.Contracts.Controllers.Posts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.MediatR.Queries.Posts;

namespace Blog.Infrastructure.Services.Interfaces;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetPosts();
    Task<PagedResponse<PostResponse>> GetPagedPosts(GetPagedPostsQuery request, CancellationToken cancellationToken);
    Task<CursorPagedResponse<PostResponse>> GetCursorPagedPosts(GetCursorPagedPostsQuery request, CancellationToken cancellationToken);
    Task<PostResponse?> GetPostById(Guid id);
    Task<PostResponse?> CreatePost(CreatePostRequest createPost);
    Task<PostResponse?> UpdatePost(UpdatePostRequest updatePost);
    Task<PostResponse?> DeletePost(Guid id);
}