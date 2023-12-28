using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Logging;
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
        var category = await _categoryService.GetCategoryById(request.Id);

        if (category is null)
        {
            _logger.LogCategoryWithIdDoesNotExist(request.Id);
        }
        else
        {
            _logger.LogCategoryWithIdWasGotten(category.CategoryId);
        }

        return category;
    }
}
