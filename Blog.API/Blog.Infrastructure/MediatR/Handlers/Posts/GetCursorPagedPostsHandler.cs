using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetCursorPagedPostsHandler : IRequestHandler<GetCursorPagedPostsQuery, CursorPagedResponse<PostResponse>>
{
    private readonly IPostService _postService;

    public GetCursorPagedPostsHandler(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<CursorPagedResponse<PostResponse>> Handle(GetCursorPagedPostsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _postService.GetCursorPagedPosts(request, cancellationToken);

        return categories;
    }
}