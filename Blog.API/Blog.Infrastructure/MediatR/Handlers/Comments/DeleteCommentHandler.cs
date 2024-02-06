using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly ILogger _logger;

    public DeleteCommentHandler(ICommentService commentService,
        ILogger<DeleteCommentHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.DeleteComment(request.Id, cancellationToken);

        if (comment is null)
        {
            _logger.LogCommentWasNotDeleted(request.Id);
        }
        else
        {
            _logger.LogCommentWasDeleted(comment.CommentId);
        }

        return comment;
    }
}