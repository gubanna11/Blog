using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Blog.Core.Entities;

public sealed class Comment
{
    [Key]
    [DataMember(Name = "commentId")]
    public Guid CommentId { get; set; }

    [StringLength(50000)]
    [DataMember(Name = "content")]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(PostId))]
    [DataMember(Name = "postId")]
    public Guid PostId { get; set; }

    [ForeignKey(nameof(UserId))]
    [DataMember(Name = "userId")]
    public string UserId { get; set; } = string.Empty;
    [DataMember(Name = "user")]
    public User? User { get; set; }

    [DataMember(Name = "publishDate")]
    public DateTime PublishDate { get; set; }
    [DataMember(Name = "parentCommentId")]
    public Guid? ParentCommentId { get; set; }
}
