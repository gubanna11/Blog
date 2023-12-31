using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.MediatR.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Blog.API.Endpoints;

public static class CategoriesEndpoints
{
    public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/categories", GetCategories);
        app.MapGet("api/categories/{id:guid}", GetCategoryById);
        app.MapPost("api/categories", CreateCategory);
        app.MapPut("api/categories", UpdateCategory);
        app.MapDelete("api/categories/{id:guid}", DeleteCategory);

        return app;
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryResponse>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetCategories(IMediator mediator, CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(new GetCategoriesQuery(), cancellationToken);

        if (categories.Any()) return Results.Ok(categories);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> GetCategoryById(IMediator mediator, Guid id, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);

        if (category is not null) return Results.Ok(category);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    private static async Task<IResult> CreateCategory(IMediator mediator, [FromBody] CreateCategoryRequest createCategory,
        CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new CreateCategoryCommand(createCategory), cancellationToken);

        return Results.Ok(category);
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> UpdateCategory(IMediator mediator, [FromBody] UpdateCategoryRequest updateCategory,
        CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new UpdateCategoryCommand(updateCategory), cancellationToken);

        if (category is not null) return Results.Ok(category);

        return Results.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(NotFoundResult))]
    private static async Task<IResult> DeleteCategory(IMediator mediator, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var category = await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);

        if (category is not null) return Results.Ok(category);

        return Results.NotFound();
    }
}