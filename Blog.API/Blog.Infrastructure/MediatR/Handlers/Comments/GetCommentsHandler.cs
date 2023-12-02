using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCommentsHandler : IRequestHandler<GetCommentsQuery, IEnumerable<CommentResponse>>
{
    private readonly ICommentService _commentService;

    public GetCommentsHandler(ICommentService commentService)
    {
        _commentService = commentService;
    }

    public async Task<IEnumerable<CommentResponse>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _commentService.GetComments();

        return comments;
    }
}