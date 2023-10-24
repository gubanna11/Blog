using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Categories;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Category?>
{
    private readonly ICategoryService _categoryService;

    public DeleteCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<Category?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryService.DeleteCategory(request.Id);

        return category;
    }
}