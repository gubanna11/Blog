using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly ILogger _logger;

    public UpdatePostHandler(IPostService postService,
        ILogger<UpdatePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var responsePost = await _postService.UpdatePost(request.Post, cancellationToken);

        if (responsePost is null)
        {
            _logger.LogError("Post wasn't updated with id {FailedUpdatePostId}", request.Post.PostId);
        }
        else
        {
            _logger.LogInformation("Post was updated with id {UpdatedPostId}", responsePost.PostId);
        }

        return responsePost;
    }
}