using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger _logger;

    public UpdateCategoryHandler(ICategoryService categoryService,
        ILogger<UpdateCategoryHandler> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        CategoryResponse? responseCategory = await _categoryService.UpdateCategory(request.Category);

        if (responseCategory is null)
        {
            _logger.LogError("Category wasn't updated with id {FailedUpdateCategoryId}", request.Category.CategoryId);
        }
        else
        {
            _logger.LogInformation("Category was updated with id {UpdatedCategoryId}", responseCategory.CategoryId);
        }

        return responseCategory;
    }
}