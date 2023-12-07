using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class DeletePostHandler : IRequestHandler<DeletePostCommand, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly ILogger _logger;

    public DeletePostHandler(IPostService postService,
        ILogger<DeletePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        var post = await _postService.DeletePost(request.Id);

        if (post is null)
        {
            _logger.LogError("Post object with id {DeletePostId} doesn't exist", request.Id);
        }

        return post;
    }
}