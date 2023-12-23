using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Categories;
using MediatR;

namespace Blog.Core.MediatR.Queries.Categories;

public sealed record GetPagedCategoriesQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize) : IRequest<PagedResponse<CategoryResponse>>;