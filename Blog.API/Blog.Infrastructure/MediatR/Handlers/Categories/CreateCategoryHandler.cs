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

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;
    private readonly IValidator<CreateCategoryRequest> _createCategoryValidator;
    private readonly ILogger _logger;

    public CreateCategoryHandler(ICategoryService categoryService,
        IValidator<CreateCategoryRequest> createCategoryValidator,
        ILogger<CreateCategoryHandler> logger)
    {
        _categoryService = categoryService;
        _createCategoryValidator = createCategoryValidator;
        _logger = logger;
    }

    public async Task<CategoryResponse?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = _createCategoryValidator.Validate(request.Category);
        if (!result.IsValid)
        {
            _logger.LogError("CreateCategoryRequest object errors:\n{errors}", result.Errors.Select(e => e.ErrorMessage));
            return null;
        }

        var responseCategory = await _categoryService.CreateCategory(request.Category);
        return responseCategory;
    }
}