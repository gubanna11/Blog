using Blog.Core.MediatR.Commands.Comments;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;

    public UpdateCommentHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentResponse?> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var responseComment = await _commentService.UpdateComment(request.Comment);

        return responseComment;
    }
}