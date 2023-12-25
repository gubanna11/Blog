using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Core.Entities;

public sealed class Comment
{
    [Key]
    public Guid CommentId { get; set; }

    [StringLength(50000)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(PostId))]
    public Guid PostId { get; set; }
    public Post? Post { get; set; }

    [ForeignKey(nameof(UserId))]
    public string UserId { get; set; } = string.Empty;
    public User? User { get; set; }

    public DateTime PublishDate { get; set; }

    public Guid? ParentCommentId { get; set; }
}
