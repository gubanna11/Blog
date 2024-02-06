using System.Runtime.Serialization;

namespace Blog.Core.Contracts.Controllers.Categories;

public sealed class CreateCategoryRequest
{
    [DataMember(Name = "name")]
    public required string Name { get; init; }
}