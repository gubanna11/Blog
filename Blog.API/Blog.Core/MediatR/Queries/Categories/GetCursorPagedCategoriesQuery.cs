using System;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Categories;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetCursorPagedCategoriesQuery(
    Guid Cursor,
    int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    bool IsIncludePosts) : IRequest<CursorPagedResponse<CategoryResponse>>;