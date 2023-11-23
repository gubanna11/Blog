using Blog.Core.ResponseDtos;
using MediatR;
using System;

namespace Blog.Core.MediatR.Commands.Categories;

public sealed record DeleteCategoryCommand(Guid Id) : IRequest<CategoryResponse?>;