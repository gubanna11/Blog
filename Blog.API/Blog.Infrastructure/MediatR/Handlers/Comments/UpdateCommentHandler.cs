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

public sealed class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly IValidator<UpdateCommentRequest> _updateCommentValidator;
    private readonly ILogger _logger;

    public UpdateCommentHandler(ICommentService commentService,
        IValidator<UpdateCommentRequest> updateCommentValidator,
        ILogger<UpdateCommentHandler> logger)
    {
        _commentService = commentService;
        _updateCommentValidator = updateCommentValidator;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var result = _updateCommentValidator.Validate(request.Comment);
        if (!result.IsValid)
        {
            _logger.LogError("UpdateCommentRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        var responseComment = await _commentService.UpdateComment(request.Comment);
        return responseComment;
    }
}