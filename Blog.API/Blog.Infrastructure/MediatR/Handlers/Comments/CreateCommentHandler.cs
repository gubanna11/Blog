﻿using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Comments;

public sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CommentResponse?>
{
    private readonly ICommentService _commentService;
    private readonly ILogger _logger;

    public CreateCommentHandler(ICommentService commentService,
        ILogger<CreateCommentHandler> logger)
    {
        _commentService = commentService;
        _logger = logger;
    }

    public async Task<CommentResponse?> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var responseComment = await _commentService.CreateComment(request.Comment);

        if(responseComment is null)
        {
            _logger.LogCommentWasNotCreated();
        }
        else
        {
            _logger.LogCommentWasCreated(responseComment.CommentId);
        }

        return responseComment;
    }
}