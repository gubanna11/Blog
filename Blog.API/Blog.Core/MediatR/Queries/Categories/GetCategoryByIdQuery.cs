using System;
using Blog.Core.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryResponse?>;