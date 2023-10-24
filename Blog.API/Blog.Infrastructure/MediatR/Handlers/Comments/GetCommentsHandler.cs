using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Comments;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCommentsHandler : IRequestHandler<GetCommentsQuery, IEnumerable<Comment>>
{
    private readonly ICommentService _commentService;

    public GetCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public Task<IEnumerable<Comment>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = _commentService.GetComments();

        return Task.FromResult(comments);
    }
}