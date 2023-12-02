using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly IValidator<CreateCommentRequest> _createCommentValidator;
    private readonly ILogger _logger;

    public CreateCommentHandler(ICommentService commentService,
        IValidator<CreateCommentRequest> createCommentValidator,
        ILogger<CreateCommentHandler> logger)
    {
        _commentService = commentService;
        _createCommentValidator = createCommentValidator;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var result = _createCommentValidator.Validate(request.Comment);
        if (!result.IsValid)
        {
            _logger.LogError("CreateCommentRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        var responseComment = await _commentService.CreateComment(request.Comment);
        return responseComment;
    }
}