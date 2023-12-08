using System;

namespace Blog.Core.Contracts.Controllers.Posts;

public sealed record CreatePostRequest(string Title, string Content, string UserId, bool IsActive, Guid CategoryId);
