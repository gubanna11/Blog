using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;

    public DeleteCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentResponse?> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.DeleteComment(request.Id);

        return comment;
    }
}