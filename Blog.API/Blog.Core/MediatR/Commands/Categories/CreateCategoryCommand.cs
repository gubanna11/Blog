using Blog.Core.Contracts.Controllers.Categories;
using Blog.Core.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record CreateCategoryCommand(CreateCategoryRequest Category) : IRequest<CategoryResponse?>;