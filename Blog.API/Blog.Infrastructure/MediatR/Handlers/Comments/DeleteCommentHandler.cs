using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, Comment?>
{
    private readonly ICommentService _commentService;

    public DeleteCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Comment?> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.DeleteComment(request.Id);

        return comment;
    }
}