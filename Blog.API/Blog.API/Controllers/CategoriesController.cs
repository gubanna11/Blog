using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Core.ResponseDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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