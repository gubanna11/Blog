using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Core.Entities;
using Blog.Infrastructure.Services.Interfaces;

namespace Blog.Infrastructure.Services;

//TODO:Implement methods
public sealed class CommentService : ICommentService
{
    public IEnumerable<Comment> GetComments()
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> GetCommentById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Comment> CreateComment(Comment createComment)
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> UpdateComment(Comment updateComment)
    {
        throw new NotImplementedException();
    }

    public Task<Comment?> DeleteComment(Guid id)
    {
        throw new NotImplementedException();
    }
}