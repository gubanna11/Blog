﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace Blog.Core.Entities;

[ProtoContract]
public sealed class Comment
{
    [Key]
    [ProtoMember(1)]
    public Guid CommentId { get; set; }

    [StringLength(50000)]
    [ProtoMember(2)]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(PostId))]
    [ProtoMember(3)]
    public Guid PostId { get; set; }

    [ForeignKey(nameof(UserId))]
    [ProtoMember(4)]
    public string UserId { get; set; } = string.Empty;
    [ProtoMember(5)]
    public User? User { get; set; }

    [ProtoMember(6)]
    public DateTime PublishDate { get; set; }

    [ProtoMember(7)]
    public Guid? ParentCommentId { get; set; }
}
