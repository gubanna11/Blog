using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace Blog.Core.Entities;

[ProtoContract]
public sealed class Post
{
    [Key]
    [ProtoMember(1)]
    public Guid PostId { get; set; }

    [StringLength(maximumLength: 500)]
    [ProtoMember(2)]
    public string Title { get; set; } = string.Empty;

    [StringLength(120000, MinimumLength = 10)]
    [ProtoMember(3)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    [ProtoMember(4)]
    public string UserId { get; set; } = string.Empty;
    [ProtoMember(5)]
    public User? User { get; set; }

    [ProtoMember(6)]
    public DateTime PublishDate { get; set; }

    [ProtoMember(7)]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [ProtoMember(8)]
    public Guid CategoryId { get; set; }
    [ProtoMember(9)]
    public Category? Category { get; set; }
}
