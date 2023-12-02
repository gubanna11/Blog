using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;

    public CreateCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryResponse?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var responseCategory = await _categoryService.CreateCategory(request.Category);
        return responseCategory;
    }
}