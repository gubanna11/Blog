using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Entities;

public sealed class Post
{
    [Key]
    public Guid PostId { get; set; }

    [StringLength(maximumLength: 500)]
    public string Title { get; set; } = string.Empty;

    [StringLength(120000, MinimumLength = 10)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    public DateTime PublishDate { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
}
