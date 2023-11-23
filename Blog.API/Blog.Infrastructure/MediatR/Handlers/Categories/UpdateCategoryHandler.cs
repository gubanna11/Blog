using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;

    public UpdateCategoryHandler(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryResponse?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        CategoryResponse? responseCategory = await _categoryService.UpdateCategory(request.Category);

        return responseCategory;
    }
}