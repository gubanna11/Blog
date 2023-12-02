using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;

    public CreateCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentResponse?> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var responseComment = await _commentService.CreateComment(request.Comment);
        return responseComment;
    }
}