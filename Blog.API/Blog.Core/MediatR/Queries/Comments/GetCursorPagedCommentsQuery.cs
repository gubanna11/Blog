using System;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Contracts.Controllers.Comments;
using MediatR;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetCursorPagedCommentsQuery(
    Guid Cursor,
    int PageSize,
    string? SearchTerm,
    string? SortColumn,
    string? SortOrder,
    bool IsIncludePost,
    bool IsIncludeUser) : IRequest<CursorPagedResponse<CommentResponse>>;