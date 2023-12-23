using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class GetPagedCategoriesHandler : IRequestHandler<GetPagedCategoriesQuery, PagedResponse<Category>>
{
    private readonly ICategoryService _categoryService;

    public GetPagedCategoriesHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    public async Task<PagedResponse<Category>> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetPagedCategories(request, cancellationToken);

        return categories;
    }
}