using Blog.Core.Entities;
using System;

namespace Blog.Core.Contracts.Controllers.Posts;

public sealed class PostResponse
{
    public required Guid PostId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    
    public required string UserId { get; init; }
    public required User? User { get; init; }
    
    public required DateTime PublishDate { get; init; }
    public required bool IsActive { get; init; }
    public required Guid CategoryId { get; init; }
    public required Category? Category { get; init; }
}
