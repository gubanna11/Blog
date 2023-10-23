using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Entities;

public sealed class Post
{
    [Key]
    public int PostId { get; set; }

    [StringLength(maximumLength: 500)]
    public string Title { get; set; }

    [StringLength(120000, MinimumLength = 50)]
    public string Content { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public DateTime PublishDate { get; set; }

    public bool IsActive { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List<Comment> Comments { get; set; }
}
