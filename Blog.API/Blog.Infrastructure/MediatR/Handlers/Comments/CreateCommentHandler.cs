using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Comments;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Comment>
{
    private readonly ICommentService _commentService;

    public CreateCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Comment> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.CreateComment(request.Comment);

        return comment;
    }
}