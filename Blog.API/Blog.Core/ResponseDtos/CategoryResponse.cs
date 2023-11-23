using Blog.Core.Entities;
using System;
using System.Collections.Generic;

namespace Blog.Core.ResponseDtos;

public sealed class CategoryResponse
{
    public Guid CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<Post>? Posts { get; set; }
}
