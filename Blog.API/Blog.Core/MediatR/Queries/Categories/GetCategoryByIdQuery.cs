using System;
using Blog.Core.Contracts.Controllers.Categories;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryResponse?>;