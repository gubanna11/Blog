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

public sealed class UpdatePostHandler : IRequestHandler<UpdatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly IValidator<UpdatePostRequest> _updatePostValidator;
    private readonly ILogger _logger;

    public UpdatePostHandler(IPostService postService,
        IValidator<UpdatePostRequest> updatePostValidator,
        ILogger<UpdatePostHandler> logger)
    {
        _postService = postService;
        _updatePostValidator = updatePostValidator;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        var result = _updatePostValidator.Validate(request.Post);
        if (!result.IsValid)
        {
            _logger.LogError("UpdatePostRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        var responsePost = await _postService.UpdatePost(request.Post);
        return responsePost;
    }
}