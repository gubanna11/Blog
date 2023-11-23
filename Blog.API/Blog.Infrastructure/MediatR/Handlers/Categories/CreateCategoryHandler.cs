using Blog.Core.MediatR.Commands.Categories;
using Blog.Core.ResponseDtos;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse?>
{
    private readonly ICategoryService _categoryService;

    public CreateCategoryHandler(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
    }

    public async Task<CategoryResponse?> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var responseCategory = await _categoryService.CreateCategory(request.Category);

        return responseCategory;
    }
}