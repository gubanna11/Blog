using Blog.Core.Entities;
using System;

namespace Blog.Core.ResponseDtos;

public sealed class CommentResponse
{
    public Guid CommentId { get; set; }

    public string Content { get; set; } = string.Empty;

    public Guid PostId { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    public DateTime PublishDate { get; set; }

    public Guid? ParentCommentId { get; set; }
}
