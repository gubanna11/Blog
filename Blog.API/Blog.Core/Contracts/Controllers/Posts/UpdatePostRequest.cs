using System;

namespace Blog.Core.Contracts.Controllers.Posts;

public sealed record UpdatePostRequest(Guid PostId, string Title, string Content, string UserId, DateTime PublishDate, bool IsActive, Guid CategoryId);