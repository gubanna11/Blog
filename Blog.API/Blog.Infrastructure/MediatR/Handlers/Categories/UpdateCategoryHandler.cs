using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Categories;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category?>
{
    private readonly ICategoryService _categoryService;

    public UpdateCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryService.UpdateCategory(request.Category);

        return category;
    }
}