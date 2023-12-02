using System.Collections.Generic;
using Blog.Core.Contracts.ResponseDtos;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoriesQuery : IRequest<IEnumerable<CategoryResponse>>;