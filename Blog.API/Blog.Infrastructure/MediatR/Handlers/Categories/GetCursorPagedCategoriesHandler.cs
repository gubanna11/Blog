﻿using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class GetCursorPagedCategoriesHandler: IRequestHandler<GetCursorPagedCategoriesQuery, CursorPagedResponse<Category>>
{
    private readonly ICategoryService _categoryService;

    public GetCursorPagedCategoriesHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    public async Task<CursorPagedResponse<Category>> Handle(GetCursorPagedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCursorPagedCategories(request, cancellationToken);

        return categories;
    }
}