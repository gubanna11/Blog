using Blog.Core.Entities;
using System;
using System.Collections.Generic;

namespace Blog.Core.Contracts.Controllers.Categories;

public sealed record CategoryResponse(
    Guid CategoryId,
    string Name,
    IEnumerable<Post>? Posts
);
