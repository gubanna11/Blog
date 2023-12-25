using Blog.Core.Contracts.Controllers.Comments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Contracts.Controllers;
using Blog.Core.Entities;
using Blog.Core.MediatR.Queries.Comments;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetComments();
    Task<PagedResponse<CommentResponse>> GetPagedComments(GetPagedCommentsQuery request, CancellationToken cancellationToken);
    Task<CursorPagedResponse<CommentResponse>> GetCursorPagedComments(GetCursorPagedCommentsQuery request, CancellationToken cancellationToken);
    Task<CommentResponse?> GetCommentById(Guid id);
    Task<CommentResponse?> CreateComment(CreateCommentRequest createComment);
    Task<CommentResponse?> UpdateComment(UpdateCommentRequest updateComment);
    Task<CommentResponse?> DeleteComment(Guid id);
}