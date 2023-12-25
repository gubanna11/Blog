using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using MediatR;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetPagedCommentsQuery(
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    int Page,
    int PageSize,
    bool IsIncludePost,
    bool IsIncludeUser) : IRequest<PagedResponse<CommentResponse>>;