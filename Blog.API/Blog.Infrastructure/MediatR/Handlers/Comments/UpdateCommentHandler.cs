using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Comments;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, Comment?>
{
    private readonly ICommentService _commentService;

    public UpdateCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Comment?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.UpdateComment(request.Comment);

        return comment;
    }
}