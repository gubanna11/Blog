using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly ILogger _logger;

    public UpdateCommentHandler(ICommentService commentService,
        ILogger<UpdateCommentHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var responseComment = await _commentService.UpdateComment(request.Comment);

        if (responseComment is null)
        {
            _logger.LogError("Comment wasn't updated with id {FailedUpdateCommentId}", request.Comment.CommentId);
        }
        else
        {
            _logger.LogInformation("Comment was updated with id {UpdatedCommentId}", responseComment.CommentId);
        }

        return responseComment;
    }
}