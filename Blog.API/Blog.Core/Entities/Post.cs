using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Blog.Core.Entities;

[ProtoContract]
public sealed class Post
{
    [Key]
    [ProtoMember(1)]
    [DataMember(Name = "postId")]
    public Guid PostId { get; set; }

    [StringLength(maximumLength: 500)]
    [DataMember(Name = "title")]
    [ProtoMember(2)]
    public string Title { get; set; } = string.Empty;

    [StringLength(120000, MinimumLength = 10)]
    [DataMember(Name = "content")]
    [ProtoMember(3)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    [DataMember(Name = "userId")]
    [ProtoMember(4)]
    public string UserId { get; set; } = string.Empty;
    [DataMember(Name = "user")]
    [ProtoMember(5)]
    public User? User { get; set; }

    [DataMember(Name = "publishDate")]
    [ProtoMember(6)]
    public DateTime PublishDate { get; set; }

    [DataMember(Name = "isActive")]
    [ProtoMember(7)]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [DataMember(Name = "categoryId")]
    [ProtoMember(8)]
    public Guid CategoryId { get; set; }
    [DataMember(Name = "Category")]
    [ProtoMember(9)]
    public Category? Category { get; set; }
}
