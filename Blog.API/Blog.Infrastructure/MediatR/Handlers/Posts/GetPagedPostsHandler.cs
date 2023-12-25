using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPagedPostsHandler : IRequestHandler<GetPagedPostsQuery, PagedResponse<PostResponse>>
{
    private readonly IPostService _postService;

    public GetPagedPostsHandler(IPostService postService)
    {
        _postService = postService;
    }
    
    public async Task<PagedResponse<PostResponse>> Handle(GetPagedPostsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _postService.GetPagedPosts(request, cancellationToken);

        return categories;
    }
}