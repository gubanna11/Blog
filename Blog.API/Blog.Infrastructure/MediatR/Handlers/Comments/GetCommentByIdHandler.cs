using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class GetCommentByIdHandler : IRequestHandler<GetCommentByIdQuery, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly ILogger _logger;

    public GetCommentByIdHandler(ICommentService commentService,
        ILogger<GetCommentByIdHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
    {
        var comment = await _commentService.GetCommentById(request.Id, cancellationToken);

        if(comment is null)
        {
            _logger.LogError("Comment object with id {GetByIdCommentId} doesn't exits", request.Id);
        }

        return comment;
    }
}