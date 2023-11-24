using Blog.Core.Contracts.Controllers.Comments;
using Blog.Core.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetComments();
    Task<CommentResponse?> GetCommentById(Guid id);
    Task<CommentResponse?> CreateComment(CreateCommentRequest createComment);
    Task<CommentResponse?> UpdateComment(UpdateCommentRequest updateComment);
    Task<CommentResponse?> DeleteComment(Guid id);
}