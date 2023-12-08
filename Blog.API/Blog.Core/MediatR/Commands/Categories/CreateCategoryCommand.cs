using Blog.Core.Contracts.Controllers.Categories;
using MediatR;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record CreateCategoryCommand(CreateCategoryRequest Category) : IRequest<CategoryResponse?>;