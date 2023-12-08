using Blog.Core.Entities;
using System;

namespace Blog.Core.Contracts.Controllers.Comments;

public sealed record CommentResponse
(
    Guid CommentId,
    string Content,
    Guid PostId,

    string UserId,
    User? User,

    DateTime PublishDate,
    Guid? ParentCommentId
);
