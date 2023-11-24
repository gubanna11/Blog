using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.MediatR.Commands.Comments;
using Blog.Core.MediatR.Queries.Comments;
using Blog.Core.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CommentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CommentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetComments(CancellationToken cancellationToken)
    {
        var comments = await _mediator.Send(new GetCommentsQuery(), cancellationToken);

        if (comments.Any()) return Ok(comments);

        return NotFound();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetCommentById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(new GetCommentByIdQuery(id), cancellationToken);

        if (comment is not null) return Ok(comment);

        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest createComment,
        CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(new CreateCommentCommand(createComment), cancellationToken);

        return Ok(comment);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest updateComment,
        CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(new UpdateCommentCommand(updateComment), cancellationToken);

        if (comment is not null) return Ok(comment);

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> DeleteComment([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var comment = await _mediator.Send(new DeleteCommentCommand(id), cancellationToken);

        if (comment is not null) return Ok(comment);

        return NotFound();
    }
}