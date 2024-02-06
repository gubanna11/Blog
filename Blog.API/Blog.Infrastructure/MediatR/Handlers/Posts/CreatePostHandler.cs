﻿using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Posts;

public sealed class CreatePostHandler : IRequestHandler<CreatePostCommand, PostResponse?>
{
    private readonly IPostService _postService;
    private readonly ILogger _logger;

    public CreatePostHandler(IPostService postService,
        ILogger<CreatePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<PostResponse?> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var responsePost = await _postService.CreatePost(request.Post, cancellationToken);

        if(responsePost is null)
        {
            _logger.LogPostWasNotCreated();
        }
        else
        {
            _logger.LogPostWasCreated(responsePost.PostId);
        }

        return responsePost;
    }
}