using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly ICategoryService _categoryService;

    public GetCategoryByIdHandler(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Category? category = await _categoryService.GetCategoryById(request.Id);

        return category;
    }
}