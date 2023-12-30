using Blog.Core.Contracts.Controllers.Comments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetComments(CancellationToken cancellationToken);
    Task<CommentResponse?> GetCommentById(Guid id, CancellationToken cancellationToken);
    Task<CommentResponse?> CreateComment(CreateCommentRequest createComment, CancellationToken cancellationToken);
    Task<CommentResponse?> UpdateComment(UpdateCommentRequest updateComment, CancellationToken cancellationToken);
    Task<CommentResponse?> DeleteComment(Guid id, CancellationToken cancellationToken);
}