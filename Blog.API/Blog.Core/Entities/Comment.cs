using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Blog.Core.Entities;

[ProtoContract]
public sealed class Comment
{
    [Key]
    [DataMember(Name = "commentId")]
    [ProtoMember(1)]
    public Guid CommentId { get; set; }

    [StringLength(50000)]
    [DataMember(Name = "content")]
    [ProtoMember(2)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(PostId))]
    [DataMember(Name = "postId")]
    [ProtoMember(3)]
    public Guid PostId { get; set; }
    public Post? Post { get; set; }

    [ForeignKey(nameof(UserId))]
    [ProtoMember(4)]
    [DataMember(Name = "userId")]
    public string UserId { get; set; } = string.Empty;
    [DataMember(Name = "user")]
    [ProtoMember(5)]
    public User? User { get; set; }

    [DataMember(Name = "publishDate")]
    [ProtoMember(6)]
    public DateTime PublishDate { get; set; }
    [DataMember(Name = "parentCommentId")]
    [ProtoMember(7)]
    public Guid? ParentCommentId { get; set; }
}
