using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Queries.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly ILogger _logger;

    public GetPostByIdHandler(IPostService postService,
        ILogger<GetPostByIdHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = await _postService.GetPostById(request.Id);

        if(post is null)
        {
            _logger.LogPostWithIdDoesNotExist(request.Id);
        }

        return post;
    }
}