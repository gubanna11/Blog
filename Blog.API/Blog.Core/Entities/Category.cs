using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Blog.Core.Entities;

public sealed class Category
{
    [DataMember(Name = "categoryId")]
    public Guid CategoryId { get; set; }

    [StringLength(200)]
    [DataMember(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [DataMember(Name = "posts")]
    public IEnumerable<Post>? Posts { get; set; }
}
