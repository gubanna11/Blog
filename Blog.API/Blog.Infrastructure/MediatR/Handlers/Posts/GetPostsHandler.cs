using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPostsHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostResponse>>
{
    private readonly IPostService _postService;

    public GetPostsHandler(IPostService postService)
    {
        _postService = postService;
    }

    public Task<IEnumerable<PostResponse>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var posts = _postService.GetPosts();

        return posts;
    }
}