using Blog.Core.Entities;
using System;

namespace Blog.Core.ResponseDtos;

public class PostResponse
{
    public Guid PostId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    public DateTime PublishDate { get; set; }

    public bool IsActive { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
