using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Blog.Core.Entities;

public sealed class Post
{
    [Key]
    [DataMember(Name = "postId")]
    public Guid PostId { get; set; }

    [StringLength(maximumLength: 500)]
    [DataMember(Name = "title")]
    public string Title { get; set; } = string.Empty;

    [StringLength(120000, MinimumLength = 10)]
    [DataMember(Name = "content")]
    public string Content { get; set; } = string.Empty;

    [ForeignKey(nameof(UserId))]
    [DataMember(Name = "userId")]
    public string UserId { get; set; } = string.Empty;
    [DataMember(Name = "user")]
    public User? User { get; set; }

    [DataMember(Name = "publishDate")]
    public DateTime PublishDate { get; set; }

    [DataMember(Name = "isActive")]
    public bool IsActive { get; set; }

    [ForeignKey(nameof(CategoryId))]
    [DataMember(Name = "categoryId")]
    public Guid CategoryId { get; set; }
    [DataMember(Name = "Category")]
    public Category? Category { get; set; }
}
