using Blog.Core.Entities;
using System;

namespace Blog.Core.Contracts.Controllers.Comments;

public sealed class CommentResponse
{
    public required Guid CommentId { get; init; }
    public required string Content { get; init; }
    public required Guid PostId { get; init; }
    public required Post? Post { get; init; }
    
    public required string UserId { get; init; }
    public required User? User { get; init; }
    
    public required DateTime PublishDate { get; init; }
    public required Guid? ParentCommentId { get; init; }
}
