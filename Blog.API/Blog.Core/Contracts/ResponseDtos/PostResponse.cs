using Blog.Core.Entities;
using System;

namespace Blog.Core.Contracts.ResponseDtos;

public sealed record PostResponse
(
    Guid PostId,
    string Title,
    string Content,

    string UserId,
    User? User,

    DateTime PublishDate,
    bool IsActive,
    Guid CategoryId,
    Category? Category
);
