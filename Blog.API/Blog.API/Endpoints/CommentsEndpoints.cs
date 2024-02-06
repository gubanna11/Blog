using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Core.MediatR.Queries.Comments;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Blog.API.Endpoints;

public static class CommentsEndpoints
{
    public static IEndpointRouteBuilder MapCommentsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/comments", GetComments);
        app.MapGet("api/comments/{id:guid}", GetCommentById);
        app.MapPost("api/comments", CreateComment);
        app.MapPut("api/comments", UpdateComment);
        app.MapDelete("api/comments/{id:guid}", DeleteComment);

        return app;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetComments(IMediator mediator, CancellationToken cancellationToken)
    {
        var comments = await mediator.Send(new GetCommentsQuery(), cancellationToken);

        if (comments.Any()) return Results.Ok(comments);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetCommentById(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var comment = await mediator.Send(new GetCommentByIdQuery(id), cancellationToken);

        if (comment is not null) return Results.Ok(comment);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    private static async Task<IResult> CreateComment(IMediator mediator, [FromBody] CreateCommentRequest createComment,
        CancellationToken cancellationToken)
    {
        var comment = await mediator.Send(new CreateCommentCommand(createComment), cancellationToken);

        return Results.Ok(comment);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> UpdateComment(IMediator mediator, [FromBody] UpdateCommentRequest updateComment,
        CancellationToken cancellationToken)
    {
        var comment = await mediator.Send(new UpdateCommentCommand(updateComment), cancellationToken);

        if (comment is not null) return Results.Ok(comment);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> DeleteComment(IMediator mediator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await mediator.Send(new DeleteCommentCommand(id), cancellationToken);

        if (comment is not null) return Results.Ok(comment);

        return Results.NotFound();
    }
}