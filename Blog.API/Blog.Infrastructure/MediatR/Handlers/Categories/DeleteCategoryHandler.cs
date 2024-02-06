using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Logging;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger _logger;

    public DeleteCategoryHandler(ICategoryService categoryService,
        ILogger<DeleteCategoryHandler> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryService.DeleteCategory(request.Id, cancellationToken);

        if (category is null)
        {
            _logger.LogCategoryWasNotDeleted(request.Id);
        }
        else
        {
            _logger.LogCategoryWasDeleted(category.CategoryId);
        }

        return category;
    }
}