using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger _logger;

    public GetCategoryByIdHandler(ICategoryService categoryService,
        ILogger<GetCategoryByIdHandler> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        CategoryResponse? category = await _categoryService.GetCategoryById(request.Id, cancellationToken);

        if(category is null)
        {
            _logger.LogError("Category object with id {GetByIdCategoryId} doesn't exist", request.Id);
        }

        return category;
    }
}