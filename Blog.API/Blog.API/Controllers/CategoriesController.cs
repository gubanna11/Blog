using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.MediatR.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Pagination;

namespace Blog.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);

        if (categories.Any()) return Ok(categories);

        return NotFound();
    }

    [HttpGet("getPaged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<CategoryResponse>))]
    public async Task<IActionResult> GetPagedCategories([FromQuery] GetPagedRequest getPagedRequest,
        CancellationToken cancellationToken,
        bool isIncludePosts = true)
    {
        var categories = await _mediator.Send(
            new GetPagedCategoriesQuery(getPagedRequest.SearchTerm, getPagedRequest.SortColumn,
                getPagedRequest.SortOrder, getPagedRequest.Page, getPagedRequest.PageSize, isIncludePosts),
            cancellationToken);

        return Ok(categories);
    }

    [HttpGet("getCursorPaged")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CursorPagedResponse<CategoryResponse>))]
    public async Task<IActionResult> GetCursorPagedCategories([FromQuery] GetCursorPagedRequest getCursorPagedRequest,
        CancellationToken cancellationToken, bool isIncludePosts = true)
    {
        var categories = await _mediator.Send(
            new GetCursorPagedCategoriesQuery(getCursorPagedRequest.Cursor, getCursorPagedRequest.PageSize,
                getCursorPagedRequest.SearchTerm, getCursorPagedRequest.SortColumn, getCursorPagedRequest.SortOrder,
                isIncludePosts),
            cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        if (category is not null) return Ok(category);

        return NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest createCategory,
        CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(createCategory), cancellationToken);

        return Ok(category);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest updateCategory,
        CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new UpdateCategoryCommand(updateCategory), cancellationToken);

        if (category is not null) return Ok(category);

        return NotFound();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

        if (category is not null) return Ok(category);

        return NotFound();
    }
}