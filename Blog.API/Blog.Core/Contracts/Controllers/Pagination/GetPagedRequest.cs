namespace Blog.Core.Contracts.Controllers.Pagination;

public sealed record GetPagedRequest(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize);