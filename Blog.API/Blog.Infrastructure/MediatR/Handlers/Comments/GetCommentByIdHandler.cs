using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentResponse?>
{
    private readonly ICommentService _commentService;

    public GetCommentByIdHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<CommentResponse?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.GetCommentById(request.Id);

        return comment;
    }
}