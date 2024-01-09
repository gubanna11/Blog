﻿using System.Collections.Generic;
using Blog.Core.Contracts.Controllers.Categories;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCategoriesQuery : IRequest<IEnumerable<CategoryResponse>>;