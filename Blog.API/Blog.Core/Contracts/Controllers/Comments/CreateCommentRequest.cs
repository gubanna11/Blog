﻿using System;
using System.Runtime.Serialization;

namespace Blog.Core.Contracts.Controllers.Comments;

public sealed class CreateCommentRequest
{
    [DataMember(Name = "content")]
    public required string Content { get; init; }
    [DataMember(Name = "postId")]
    public required Guid PostId { get; init; }
    [DataMember(Name = "userId")]
    public required string UserId { get; init; }
    [DataMember(Name = "parentCommentId")]
    public required Guid? ParentCommentId { get; init; }
}