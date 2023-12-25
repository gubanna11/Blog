using System;

namespace Blog.Core.Contracts.Controllers.Pagination;

public sealed record GetCursorPagedRequest(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int PageSize,
    Guid Cursor = default);