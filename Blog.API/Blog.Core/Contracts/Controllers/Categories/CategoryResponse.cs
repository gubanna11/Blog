using Blog.Core.Entities;
using System;
using System.Collections.Generic;

namespace Blog.Core.Contracts.Controllers.Categories;

public sealed class CategoryResponse
{
    public required Guid CategoryId { get; init; }
    public required string Name { get; init; }
    public IEnumerable<Post>? Posts { get; init; } 
}
