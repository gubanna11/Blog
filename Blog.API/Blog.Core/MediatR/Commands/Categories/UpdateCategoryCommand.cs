using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record UpdateCategoryCommand(UpdateCategoryRequest Category) : IRequest<Category?>;