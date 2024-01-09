using System;

namespace Blog.Core.Contracts.Controllers.Comments;

public sealed record CreateCommentRequest(string Content, Guid PostId, string UserId, Guid? ParentCommentId);