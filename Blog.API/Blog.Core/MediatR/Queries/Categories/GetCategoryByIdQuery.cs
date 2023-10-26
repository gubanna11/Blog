using System;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<Category?>;