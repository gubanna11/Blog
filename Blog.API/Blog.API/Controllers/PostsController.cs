using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Pagination;
using Blog.Core.Contracts.Controllers.Posts;
using Blog.Core.MediatR.Commands.Posts;
using Blog.Core.MediatR.Queries.Posts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
    {
        var posts = await _mediator.Send(new GetPostsQuery(), cancellationToken);

        if (posts.Any()) return Ok(posts);

        return NotFound();
    }

    [HttpGet("getPaged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<PostResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetPagedPosts([FromQuery] GetPagedRequest getPagedRequest,
        CancellationToken cancellationToken,
        bool isIncludePost = true, bool isIncludeUser = true)
    {
        var posts = await _mediator.Send(
            new GetPagedPostsQuery(getPagedRequest.SearchTerm, getPagedRequest.SortColumn,
                getPagedRequest.SortOrder, getPagedRequest.Page, getPagedRequest.PageSize, isIncludePost,
                isIncludeUser),
            cancellationToken);

        return Ok(posts);
    }

    [HttpGet("getCursorPaged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CursorPagedResponse<PostResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetCursorPagedPosts([FromQuery] GetCursorPagedRequest getCursorPagedRequest,
        CancellationToken cancellationToken, bool isIncludePost = true, bool isIncludeUser = true)
    {
        var posts = await _mediator.Send(
            new GetCursorPagedPostsQuery(getCursorPagedRequest.Cursor, getCursorPagedRequest.PageSize,
                getCursorPagedRequest.SearchTerm, getCursorPagedRequest.SortColumn, getCursorPagedRequest.SortOrder,
                isIncludePost, isIncludeUser),
            cancellationToken);

        return Ok(posts);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetPostById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var post = await _mediator.Send(new GetPostByIdQuery(id), cancellationToken);

        if (post is not null) return Ok(post);

        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest createPost,
        CancellationToken cancellationToken)
    {
        var post = await _mediator.Send(new CreatePostCommand(createPost), cancellationToken);

        return Ok(post);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest updatePost,
        CancellationToken cancellationToken)
    {
        var post = await _mediator.Send(new UpdatePostCommand(updatePost), cancellationToken);

        if (post is not null) return Ok(post);

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> DeletePost([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var post = await _mediator.Send(new DeletePostCommand(id), cancellationToken);

        if (post is not null) return Ok(post);

        return NotFound();
    }
}