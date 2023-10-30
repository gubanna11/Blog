using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly ICategoryService _categoryService;

    public GetCategoriesHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetCategories();

        return categories;
    }
}