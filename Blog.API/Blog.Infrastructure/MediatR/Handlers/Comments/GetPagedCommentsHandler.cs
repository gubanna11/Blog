using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetPagedCommentsHandler : IRequestHandler<GetPagedCommentsQuery, PagedResponse<CommentResponse>>
{
    private readonly ICommentService _commentService;

    public GetPagedCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    public async Task<PagedResponse<CommentResponse>> Handle(GetPagedCommentsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _commentService.GetPagedComments(request, cancellationToken);

        return categories;
    }
}