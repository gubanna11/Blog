using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;

    public DeleteCategoryHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryResponse?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        CategoryResponse? category = await _categoryService.DeleteCategory(request.Id);

        return category;
    }
}