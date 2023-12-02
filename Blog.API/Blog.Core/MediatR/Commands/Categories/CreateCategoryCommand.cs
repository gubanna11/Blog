using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.Contracts.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record CreateCategoryCommand(CreateCategoryRequest Category) : IRequest<CategoryResponse?>;