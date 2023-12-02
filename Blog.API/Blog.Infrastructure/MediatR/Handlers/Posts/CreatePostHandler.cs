using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly IValidator<CreatePostRequest> _createPostValidator;
    private readonly ILogger _logger;

    public CreatePostHandler(IPostService postService,
        IValidator<CreatePostRequest> createPostValidator,
        ILogger<CreatePostHandler> logger)
    {
        _postService = postService;
        _createPostValidator = createPostValidator;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var result = _createPostValidator.Validate(request.Post);
        if (!result.IsValid)
        {
            _logger.LogError("CreatePostRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        var responsePost = await _postService.CreatePost(request.Post);
        return responsePost;
    }
}