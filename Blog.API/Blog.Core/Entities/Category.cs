using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Core.Entities;

public sealed class Category
{
    public Guid CategoryId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    public IEnumerable<Post>? Posts { get; set; }
}
