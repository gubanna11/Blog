using System;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.Contracts.Controllers.Posts;
using MediatR;

namespace Blog.Core.MediatR.Queries.Posts;

public sealed record GetCursorPagedPostsQuery(
    Guid Cursor,
    int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    bool IsIncludeUser,
    bool IsIncludeCategory) : IRequest<CursorPagedResponse<PostResponse>>;