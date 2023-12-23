using Blog.Core.Contracts.Controllers;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Core.MediatR.Queries.Comments;

public sealed record GetPagedCommentsQuery(string? SearchTerm, string? SortColumn, string? SortOrder, int Page, int PageSize) : IRequest<PagedResponse<Comment>>;