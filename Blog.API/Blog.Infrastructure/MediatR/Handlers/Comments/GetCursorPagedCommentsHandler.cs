using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCursorPagedCommentsHandler : IRequestHandler<GetCursorPagedCommentsQuery, CursorPagedResponse<CommentResponse>>
{
    private readonly ICommentService _commentService;

    public GetCursorPagedCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    public async Task<CursorPagedResponse<CommentResponse>> Handle(GetCursorPagedCommentsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _commentService.GetCursorPagedComments(request, cancellationToken);

        return categories;
    }
}