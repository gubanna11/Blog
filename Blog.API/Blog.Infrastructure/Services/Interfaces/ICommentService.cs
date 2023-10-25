using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;

namespace Blog.Infrastructure.Services.Interfaces;

public interface ICommentService
{
    IEnumerable<Comment> GetComments();
    Task<Comment?> GetCommentById(Guid id);
    Task<Comment> CreateComment(Comment createComment);
    Task<Comment?> UpdateComment(Comment updateComment);
    Task<Comment?> DeleteComment(Guid id);
}