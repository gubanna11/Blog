using System;
using System.Runtime.Serialization;

namespace Blog.Core.Contracts.Controllers.Posts;

public sealed class UpdatePostRequest
{
    [DataMember(Name = "postId")]
    public required Guid PostId { get; init; }
    [DataMember(Name = "title")]
    public required string Title { get; init; }
    [DataMember(Name = "content")]
    public required string Content { get; init; }
    [DataMember(Name = "userId")]
    public required string UserId { get; init; }
    [DataMember(Name = "publishDate")]
    public required DateTime PublishDate { get; init; }
    [DataMember(Name = "isActive")]
    public required bool IsActive { get; init; }
    [DataMember(Name = "categoryId")]
    public required Guid CategoryId { get; init; }
}