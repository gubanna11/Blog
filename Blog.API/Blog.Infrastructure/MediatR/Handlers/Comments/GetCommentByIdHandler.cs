using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Comments;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, Comment?>
{
    private readonly ICommentService _commentService;

    public GetCommentByIdHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<Comment?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.GetCommentById(request.Id);

        return comment;
    }
}