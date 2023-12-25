using System;
using System.Runtime.Serialization;

namespace Blog.Core.Contracts.Controllers.Categories;

public sealed class UpdateCategoryRequest
{
    [DataMember(Name = "categoryId")]
    public required Guid CategoryId { get; init; }
    [DataMember(Name = "name")]
    public required string Name { get; init; }
};