using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Categories;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoryService _categoryService;

    public CreateCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryService.CreateCategory(request.Category);

        return category;
    }
}