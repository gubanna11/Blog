using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Core.MediatR.Commands.Categories;
using Blog.Infrastructure.Services.Interfaces;
using MapsterMapper;
using MediatR;

namespace Blog.Infrastructure.MediatR.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryService categoryService, IMapper mapper)
    {
        _categoryService = categoryService;
        _mapper = mapper;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request.Category);
        var responseCategory = await _categoryService.CreateCategory(category);

        return responseCategory;
    }
}