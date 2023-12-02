using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Contracts.ResponseDtos;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<UpdateCategoryRequest> _updateCategoryValidator;
    private readonly ILogger _logger;

    public UpdateCategoryHandler(ICategoryService categoryService,
        IValidator<UpdateCategoryRequest> updateCategoryValidator,
        ILogger<UpdateCategoryHandler> logger)
    {
        _categoryService = categoryService;
        _updateCategoryValidator = updateCategoryValidator;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _updateCategoryValidator.Validate(request.Category);
        if (!result.IsValid)
        {
            _logger.LogError("UpdateCategoryRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        CategoryResponse? responseCategory = await _categoryService.UpdateCategory(request.Category);
        return responseCategory;
    }
}