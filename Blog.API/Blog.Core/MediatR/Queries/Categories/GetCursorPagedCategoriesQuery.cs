﻿using System;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCursorPagedCategoriesQuery(
    Guid Cursor,
    int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder) : IRequest<CursorPagedResponse<Category>>;