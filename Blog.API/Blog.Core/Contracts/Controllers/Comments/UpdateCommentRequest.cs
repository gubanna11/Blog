using System;

namespace Blog.Core.Contracts.Controllers.Comments;

public sealed record UpdateCommentRequest(Guid CommentId, string Content, Guid PostId, string UserId, Guid? ParentCommentId);
