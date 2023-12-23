using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetPagedCommentsHandler
{
    private readonly ICommentService _commentService;

    public GetPagedCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }
    
    public async Task<PagedResponse<Comment>> Handle(GetPagedCommentsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _commentService.GetPagedComments(request, cancellationToken);

        return categories;
    }
}