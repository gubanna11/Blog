using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger _logger;

    public CreateCategoryHandler(ICategoryService categoryService,
        ILogger<CreateCategoryHandler> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var responseCategory = await _categoryService.CreateCategory(request.Category, cancellationToken);

        if(responseCategory is null)
        {
            _logger.LogError("Category wasn't created");
        }
        else
        {
            _logger.LogInformation("Category was created with id {CreatedCategoryId}", responseCategory.CategoryId);
        }

        return responseCategory;
    }
}