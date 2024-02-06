using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.MediatR.Queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Blog.API.Endpoints;

public static class PostsEndpoints
{
    public static IEndpointRouteBuilder MapPostsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/posts", GetPosts);
        app.MapGet("api/posts/{id:guid}", GetPostById);
        app.MapPost("api/posts", CreatePost);
        app.MapPut("api/posts", UpdatePost);
        app.MapDelete("api/posts/{id:guid}", DeletePost);

        return app;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetPosts(IMediator mediator, CancellationToken cancellationToken)
    {
        var posts = await mediator.Send(new GetPostsQuery(), cancellationToken);

        if (posts.Any()) return Results.Ok(posts);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetPostById(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new GetPostByIdQuery(id), cancellationToken);

        if (post is not null) return Results.Ok(post);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    private static async Task<IResult> CreatePost(IMediator mediator, [FromBody] CreatePostRequest createPost,
        CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new CreatePostCommand(createPost), cancellationToken);

        return Results.Ok(post);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> UpdatePost(IMediator mediator, [FromBody] UpdatePostRequest updatePost,
        CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new UpdatePostCommand(updatePost), cancellationToken);

        if (post is not null) return Results.Ok(post);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> DeletePost(IMediator mediator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var post = await mediator.Send(new DeletePostCommand(id), cancellationToken);

        if (post is not null) return Results.Ok(post);

        return Results.NotFound();
    }
}