using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using ProtoBuf;

namespace Blog.Core.Entities;

[ProtoContract]
public sealed class Category
{
    [DataMember(Name = "categoryId")]
    [ProtoMember(1)]
    public Guid CategoryId { get; set; }

    [ProtoMember(2)]
    [StringLength(200)]
    [DataMember(Name = "name")]
    public string Name { get; set; } = string.Empty;

    [DataMember(Name = "posts")]
    [ProtoMember(3)]
    public IEnumerable<Post>? Posts { get; set; }
}
