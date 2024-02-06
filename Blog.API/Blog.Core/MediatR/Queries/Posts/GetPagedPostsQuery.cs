using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Posts;
using MediatR;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetPagedPostsQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize,
    bool IsIncludeUser,
    bool IsIncludeCategory) : IRequest<PagedResponse<PostResponse>>;
