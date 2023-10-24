using System.Collections.Generic;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoriesQuery : IRequest<IEnumerable<Category>>;